using ArenaServer.Data;
using ArenaServer.Data.Models;
using ArenaServer.Data.Transfer;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ArenaServer.Services
{
    public class UserService
    {
        #region Fields

        private readonly PokemonService pokemonService;
        private readonly AppDbContext db;

        #endregion

        #region Constructor

        public UserService()
        {
            this.pokemonService = new PokemonService();
            this.db = new AppDbContextFactory().Create();
        }

        public UserService(AppDbContext db)
        {
            this.pokemonService = new PokemonService(db);
            this.db = db;
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

        public async Task<bool> RegisterUser(string userid, string displayname, string starter)
        {
            if (!await IsUserRegistered(userid))
            {
                db.Twitchuser.Add(new Twitchuser() { Twitchuser_Id = userid, DisplayName = displayname, Kz_Log_Enabled = true });

                AddPokemon(userid, pokemonService.GetTransferPokemonFromName(starter));
                await db.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async void DeleteUser(string userid)
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
            TransferTwitchuser returnUser;

            if (await IsUserRegistered(userid))
            {
                    var dbUser = await db.Twitchuser.Where(tu => tu.Twitchuser_Id == userid).FirstOrDefaultAsync();

                    returnUser = new TransferTwitchuser()
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

        #endregion

        #region Pokemon-Handling

        public async void AddPokemon(string userId, TransferPokemon pokemon)
        {
            if (!await IsUserRegistered(userId))
            {
                return;
            }

            var catchedPokemon = await db.CatchedPokemon.Where(cp => cp.Twitchuser_Id == userId && cp.SdPokemon.Name == pokemon.Name).FirstOrDefaultAsync();

            if (null == catchedPokemon)
            {
                var sdPokemonId = pokemonService.GetSdPokemonIdFromName(pokemon.Name);

                if (null == sdPokemonId) return;

                db.CatchedPokemon.Add(new CatchedPokemon() { SdPokemon_Id = sdPokemonId.Value, Pokemon_AmountCatched = 1, Pokemon_AmountOnFightingTeam = 0 });
            }
            else
            {
                catchedPokemon.Pokemon_AmountCatched += 1;
            }

            await db.SaveChangesAsync();
        }

        public async void RemovePokemon(string userId, TransferPokemon pokemon)
        {
            if (!await IsUserRegistered(userId))
            {
                return;
            }

            var catchedPokemon = await db.CatchedPokemon.Where(cp => cp.Twitchuser_Id == userId && cp.SdPokemon.Name == pokemon.Name).FirstOrDefaultAsync();

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
                    Type = entity.SdPokemon.Type
                }
            };
        }

        #endregion

        #endregion
    }
}