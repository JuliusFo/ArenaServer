using ArenaServer.Data;
using ArenaServer.Data.Common.Models;
using ArenaServer.Data.Common.Models.Extensions;
using ArenaServer.Data.Models;
using ArenaServer.Data.Transfer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Api;

namespace ArenaServer.Services
{
    public class UserService
    {
        #region Fields

        private readonly PokemonService pokemonService;
        private readonly AppDbContext db;
        private readonly TwitchAPI twitchAPI;

        #endregion

        #region Constructor

        public UserService(AppDbContext db, TwitchAPI api)
        {
            this.pokemonService = new PokemonService(db);
            this.db = db;
            this.twitchAPI = api;
        }

        #endregion

        #region Properties



        #endregion

        #region Methods

        #region User-Handling

        public async Task<bool> IsUserRegistered(string userid)
        {
            return (await db.Twitchuser.Where(tu => tu.Twitchuser_Id == userid).FirstOrDefaultAsync()) != null;
        }

        public async Task<UserRegistrationResponse> RegisterUser(string userid, string displayname, string starter)
        {
            if (!await IsUserRegistered(userid))
            {
                db.Twitchuser.Add(new Twitchuser() { Twitchuser_Id = userid, DisplayName = displayname, Kz_Log_Enabled = true });
                await db.SaveChangesAsync();

                await AddPokemon(userid, pokemonService.GetTransferPokemonFromName(starter),true);

                return new UserRegistrationResponse()
                {
                    RegistrationSuccessfull = true,
                    UserAlreadyRegistered = false
                };
            }
            else
            {
                return new UserRegistrationResponse()
                {
                    RegistrationSuccessfull = false,
                    UserAlreadyRegistered = true
                };
            }
        }

        public async Task DeleteUser(string userid)
        {
            if (await IsUserRegistered(userid))
            {
                    db.Twitchuser.Remove(db.Twitchuser.Where(tu => tu.Twitchuser_Id == userid).FirstOrDefault());
                    db.CatchedPokemon.RemoveRange(db.CatchedPokemon.Where(cp => cp.Twitchuser_Id == userid));

                    await db.SaveChangesAsync();
                }
        }

        public async Task<TransferTwitchuser> GetUser(string userid)
        {
            if (await IsUserRegistered(userid))
            {
                var dbUser = await db.Twitchuser.Where(tu => tu.Twitchuser_Id == userid)
                .Include(u => u.CatchedPokemon)
                .FirstOrDefaultAsync();

                var returnUser = new TransferTwitchuser()
                {
                    Id = dbUser.Twitchuser_Id,
                    DisplayName = dbUser.DisplayName,
                    KzLogEnabled = dbUser.Kz_Log_Enabled
                };

                returnUser.CatchedPokemonList = dbUser.CatchedPokemon.Select(cp => ConvertCatchedPokemonToTransfer(cp)).ToList();
                
                return returnUser;
            }
            else
            {
                return null;
            }
        }

        public async Task<TransferTwitchuser> GetUserByName(TwitchChatMessage twitchChatMessage)
        {
            var userName = twitchChatMessage.GetTargetUserName();

            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }

            //Get user id from twitch
            var reply = await twitchAPI.V5.Users.GetUserByNameAsync(userName);

            string id;

            try
            {
                id = reply.Matches[0].Id;
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }

            var user = await GetUser(id);

            return user;
        }

        public async Task SetLastFightDt(string userId)
        {
            var user = await db.Twitchuser.Where(u => u.Twitchuser_Id == userId).FirstOrDefaultAsync();

            if(null != user)
            {
                user.Dt_Last_Userfight = DateTime.Now;
                await db.SaveChangesAsync();
            }
        }

        public async Task SetLastAVFightDt(string userId, string avName)
        {
            var user = await db.Twitchuser.Where(u => u.Twitchuser_Id == userId).FirstOrDefaultAsync();

            if(null != user)
            {
                var av = user.Achievements.Where(a => a.SdAchievement.Name == avName).FirstOrDefault();

                if(null != av)
                {
                    av.LastFight = DateTime.Now;
                    await db.SaveChangesAsync();
                }
            }
        }

        #endregion

        #region Pokemon-Handling

        public async Task AddPokemon(string userId, TransferPokemon pokemon, bool isRegistration)
        {
            if (!await IsUserRegistered(userId) && !isRegistration)
            {
                return;
            }

            var catchedPokemon = await db.CatchedPokemon.Where(cp => cp.Twitchuser_Id == userId && cp.SdPokemon.SdPokemon_Id == pokemon.ID).FirstOrDefaultAsync();

            if (null == catchedPokemon)
            {
                db.CatchedPokemon.Add(new CatchedPokemon() { SdPokemon_Id = pokemon.ID, Pokemon_AmountCatched = 1, Pokemon_AmountOnFightingTeam = 0, Twitchuser_Id = userId });
            }
            else
            {
                catchedPokemon.Pokemon_AmountCatched += 1;
            }

            await db.SaveChangesAsync();
        }

        public async Task RemovePokemon(string userId, TransferPokemon pokemon)
        {
            if (!await IsUserRegistered(userId))
            {
                return;
            }

            var catchedPokemon = await db.CatchedPokemon.Where(cp => cp.Twitchuser_Id == userId && cp.SdPokemon.SdPokemon_Id == pokemon.ID).FirstOrDefaultAsync();

            if (null == catchedPokemon) return;

            if (catchedPokemon.Pokemon_AmountCatched == 1)
            {
                db.CatchedPokemon.Remove(catchedPokemon);
            }
            else
            {
                catchedPokemon.Pokemon_AmountCatched -= 1;
            }

            await db.SaveChangesAsync();
        }

        #endregion

        #region AV-Handling

        public async Task<UserAchievementResponse> CheckAndAddAchievement(string userId)
        {
            bool result = false;
            var dbUser = await db.Twitchuser.Where(tu => tu.Twitchuser_Id == userId).FirstOrDefaultAsync();
            var sdAchievements = await db.SdAchievement.ToListAsync();
            var amountCatched = dbUser.CatchedPokemon.Sum(c => c.Pokemon_AmountCatched);
            var unlockedAchievements = sdAchievements.Where(sdA => !dbUser.Achievements.Any(a => a.SdAchievment_Id == sdA.SdAchievement_Id) && amountCatched >= sdA.UnlockedOnCount).ToList();

            foreach(var unlockedAchievement in unlockedAchievements ?? Enumerable.Empty<SdAchievement>())
            {
                db.Achievements.Add(new Achievements()
                {
                    SdAchievment_Id = unlockedAchievement.SdAchievement_Id,
                    Twitchuser_Id = dbUser.Twitchuser_Id
                });
                result = true;
            }

            await db.SaveChangesAsync();

            return new UserAchievementResponse(result, unlockedAchievements.Select(u => new UserAchievementResponseItem(u.Name, u.NPCName)));
        }

        public async Task<bool> CanFightAcievement(string userId, string avName)
        {
            var dbUser = await db.Twitchuser.Where(tu => tu.Twitchuser_Id == userId).FirstOrDefaultAsync();
            var listFightTS = new TimeSpan(0, 10, 0);
            
            if(null != dbUser)
            {
                return dbUser.Achievements.Any(a => a.SdAchievement.NPCName == avName && ((a.LastFight.HasValue && (DateTime.Now - a.LastFight) > listFightTS) || a.LastFight == null));
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Converting

        private TransferCatchedPokemon ConvertCatchedPokemonToTransfer(CatchedPokemon entity)
        {
            return new TransferCatchedPokemon()
            {
                AmountCatched = entity.Pokemon_AmountCatched,
                AmountOnFightingTeam = entity.Pokemon_AmountOnFightingTeam,
                Pokemon = new TransferPokemon()
                {
                    ATK = entity.SdPokemon.ATK,
                    Description = entity.SdPokemon.Description,
                    HP = entity.SdPokemon.HP,
                    Name = entity.SdPokemon.Name,
                    Rarity = entity.SdPokemon.Rarity,
                    Type = entity.SdPokemon.Type,
                    ID = entity.SdPokemon_Id
                }
            };
        }

        #endregion

        #endregion
    }
}