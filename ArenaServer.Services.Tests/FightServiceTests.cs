using ArenaServer.Data;
using ArenaServer.Data.Common.Models;
using ArenaServer.Data.Models;
using ArenaServer.Data.Transfer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArenaServer.Services.Tests
{
    [TestClass]
    public class FightServiceTests
    {
        #region Fields

        private TestingDBContext context;
        private Fight fightService;
        private PokemonService pokemonService;
        private UserService userService;

        #endregion

        #region Initialize

        [TestInitialize]
        public void Initialize()
        {
            context = new TestingDBContext();
            fightService = new Fight(context.Db);
            this.pokemonService = new PokemonService(context.Db);
            this.userService = new UserService(context.Db);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public async Task FightTestOneVSOne()
        {
            var db = context.Db;

            //User 1 erstellen
            var user_challenger = new Twitchuser()
            {
                Twitchuser_Id = "123",
                DisplayName = "Ash",
                Dt_Last_Userfight = null,
                Kz_Log_Enabled = false
            };

            //Pokemon 1 erstellen
            var pokemon_zapdos = new SdPokemon()
            {
                ATK = 50,
                Name = "Zapdos",
                HP = 999,
                Rarity = PokemonRarity.Legendary,
                Description = "Hehe Op",
                Type = PokemonType.Electric
            };
            user_challenger.CatchedPokemon = new List<CatchedPokemon>() {
                new CatchedPokemon()
                {
                    Pokemon_AmountCatched = 1,
                    Pokemon_AmountOnFightingTeam = 1,
                    SdPokemon = pokemon_zapdos,
                    Twitchuser = user_challenger,
                    CatchedPokemon_Id = 1
                }
            };

            db.Twitchuser.Add(user_challenger);
            await db.SaveChangesAsync();

            //User 2 erstellen
            var user_defender = new Twitchuser()
            {
                Twitchuser_Id = "456",
                DisplayName = "Gary",
                Dt_Last_Userfight = null,
                Kz_Log_Enabled = false
            };

            //Pokemon 2 erstellen
            var pokemon_raupy = new SdPokemon()
            {
                ATK = 5,
                Name = "Raupy",
                HP = 100,
                Rarity = PokemonRarity.Common,
                Description = "Hehe Low",
                Type = PokemonType.Grass
            };
            user_defender.CatchedPokemon = new List<CatchedPokemon>() {
                new CatchedPokemon()
                {
                    Pokemon_AmountCatched = 1,
                    Pokemon_AmountOnFightingTeam = 1,
                    SdPokemon = pokemon_raupy,
                    Twitchuser = user_defender,
                    CatchedPokemon_Id = 2
                }
            };

            db.Twitchuser.Add(user_defender);
            await db.SaveChangesAsync();

            //Kampf vorbereiten
            var tr_challenger = await userService.GetUser("123");
            var tr_defender = await userService.GetUser("456");

            FightParticipant challenger = new FightParticipant()
            {
                User = tr_challenger,
                Pokemon = new List<TransferPokemon>() { tr_challenger.GetRandomPokemon() }
            };

            FightParticipant defender = new FightParticipant()
            {
                User = tr_defender,
                Pokemon = new List<TransferPokemon>() { tr_defender.GetRandomPokemon() }
            };

            //Kampf ausführen
            var result = fightService.CalulateOneVSOne(challenger, defender);

            //Kampf auswerten
            Assert.IsTrue(result.User.DisplayName == "Ash");
        }

        #endregion
    }
}