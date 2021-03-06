﻿using ArenaServer.Data;
using ArenaServer.Data.Common.Models;
using ArenaServer.Data.Models;
using ArenaServer.Data.Transfer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchLib.Api;

namespace ArenaServer.Services.Tests
{
    [TestClass]
    public class FightServiceTests
    {
        #region Fields

        private TestingDBContext context;
        private PokemonService pokemonService;
        private AccessService accessService;
        private TwitchAPI api;
        private UserService userService;

        #endregion

        #region Initialize

        [TestInitialize]
        public void Initialize()
        {
            context = new TestingDBContext();
            this.pokemonService = new PokemonService(context.Db);

            //Init access
            accessService = new AccessService();

            //Init Twitch API
            api = new TwitchAPI();
            api.Settings.ClientId = accessService.GetTwitchClientID();
            api.Settings.AccessToken = accessService.GetTwitchAccessToken();

            this.userService = new UserService(context.Db,api);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public async Task FightOneVSOne()
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
                SdPokemon_Id = 1,
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
                SdPokemon_Id = 2,
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

            var fight_WinnerCounter = 0;
            var fight_Counter = 100;

            for(int i = 0; i < fight_Counter; i++)
            {
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

                challenger.Pokemon[0].HP = 100;
                defender.Pokemon[0].HP = 100;

                //Kampf ausführen
                var fight = new Fight();
                var input = new FightOptions(challenger, defender, false);
                var result = fight.CalculateUnlimited(input);

                if(result.Winner.DisplayName == "Ash")
                {
                    fight_WinnerCounter += 1;
                }
            }

            //Kampf auswerten
            Assert.AreEqual(fight_WinnerCounter, 100);
        }

        [TestMethod]
        public async Task FightTeamVsTeam()
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
                SdPokemon_Id = 1,
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
                    Pokemon_AmountCatched = 6,
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
                SdPokemon_Id = 2,
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
                    Pokemon_AmountCatched = 6,
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

            var fight_WinnerCounter = 0;
            var fight_Counter = 100;

            for (int i = 0; i < fight_Counter; i++)
            {
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

                challenger.Pokemon[0].HP = 100;
                defender.Pokemon[0].HP = 100;

                //Kampf ausführen
                var fight = new Fight();
                var input = new FightOptions(challenger, defender, false);
                var result = fight.CalculateUnlimited(input);

                if (result.Winner.DisplayName == "Ash")
                {
                    fight_WinnerCounter += 1;
                }
            }

            //Kampf auswerten
            Assert.AreEqual(fight_WinnerCounter, 100);
        }

        [TestMethod]
        public async Task FightRandomTeamVSTeam()
        {
            //Ausgangspunkt dieses Tests sind zwei Teams mit gleichen Pokemon.
            //Erwartungshaltung bei 100 Kämpfen wäre eine ausgeglichene Quote von etwa 50%.
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
            var pokemon_raupy = new SdPokemon()
            {
                SdPokemon_Id = 1,
                ATK = 5,
                Name = "Raupy",
                HP = 100,
                Rarity = PokemonRarity.Common,
                Description = "Hehe Low",
                Type = PokemonType.Grass
            };
            user_challenger.CatchedPokemon = new List<CatchedPokemon>() {
                new CatchedPokemon()
                {
                    Pokemon_AmountCatched = 6,
                    Pokemon_AmountOnFightingTeam = 1,
                    SdPokemon = pokemon_raupy,
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

            user_defender.CatchedPokemon = new List<CatchedPokemon>() {
                new CatchedPokemon()
                {
                    Pokemon_AmountCatched = 6,
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

            var fight_WinnerCounter = 0;
            var fight_Counter = 1000;

            for (int i = 0; i < fight_Counter; i++)
            {
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

                challenger.Pokemon[0].HP = 100;
                defender.Pokemon[0].HP = 100;

                //Kampf ausführen
                var fight = new Fight();
                var input = new FightOptions(challenger, defender, false);
                var result = fight.CalculateUnlimited(input);

                if (result.Winner.DisplayName == "Ash")
                {
                    fight_WinnerCounter += 1;
                }
            }

            //Kampf auswerten
            Assert.IsTrue(fight_WinnerCounter > 450);
            Assert.IsTrue(fight_WinnerCounter < 550);
        }

        #endregion
    }
}