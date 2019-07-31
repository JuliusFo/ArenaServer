using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ArenaServer.Data.Tests
{
    [TestClass]
    public class EntityTests
    {
        #region Fields

        private TestingDBContext context;

        #endregion

        #region Initialize

        [TestInitialize]
        public void Initialize()
        {
            context = new TestingDBContext();
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }

        #endregion

        #region Tesrs

        [TestMethod]
        public async Task TwitchuserTest()
        {
            var db = context.Db;

            db.Twitchuser.Add(new Twitchuser()
            {
                DisplayName = "UNIT TEST",
                Kz_Log_Enabled = false,
                Twitchuser_Id = "00000"
            });

            await db.SaveChangesAsync();
            var existing_users = db.Twitchuser.ToList();

            Assert.IsTrue(existing_users.Any());
            Assert.IsTrue(existing_users[0].DisplayName == "UNIT TEST");
        }

        [TestMethod]
        public async Task SdPokemonTest()
        {
            var db = context.Db;

            db.SdPokemon.Add(new SdPokemon()
            {
                Name = "UNIT TEST",
                ATK = 100,
                Description = "UNIT TEST",
                HP = 100,
                Rarity = PokemonRarity.Common,
                Type = PokemonType.Bug
            });

            await db.SaveChangesAsync();
            var existing_pokemon = db.SdPokemon.ToList();

            Assert.IsTrue(existing_pokemon.Any());
            Assert.IsTrue(existing_pokemon[0].Name == "UNIT TEST");
        }

        [TestMethod]
        public async Task SdAchievementTest()
        {
            var db = context.Db;

            db.SdAchievement.Add(new SdAchievement()
            {
                Name = "UNIT TEST",
                Description = "UNIT TEST",
                NPCName = "UNIT TEST",
                UnlockedOnCount = 0,
                Pokemon_1 = new SdPokemon()
                {
                    Name = "UNIT TEST",
                    ATK = 100,
                    Description = "UNIT TEST",
                    HP = 100,
                    Rarity = PokemonRarity.Common,
                    Type = PokemonType.Bug
                }
            });

            await db.SaveChangesAsync();
            var existing_achievements = db.SdAchievement.ToList();

            Assert.IsTrue(existing_achievements.Any());
            Assert.IsTrue(existing_achievements[0].Name == "UNIT TEST");
        }

        [TestMethod]
        public async Task CatchedPokemonTest()
        {
            var db = context.Db;

            db.CatchedPokemon.Add(new CatchedPokemon()
            {
                Pokemon_AmountCatched = 1,
                Pokemon_AmountOnFightingTeam = 1,
                SdPokemon_Id = 0,
                Twitchuser_Id = "0"
            });

            await db.SaveChangesAsync();
            var catched_pokemon = db.CatchedPokemon.ToList();

            Assert.IsTrue(catched_pokemon.Any());
        }

        [TestMethod]
        public async Task AchievementsTest()
        {
            var db = context.Db;

            db.Achievements.Add(new Achievements()
            {
                Twitchuser_Id = "",
                SdAchievment_Id = 0,
                LastFight = DateTime.Now
            });

            await db.SaveChangesAsync();
            var avs = db.Achievements.ToList();

            Assert.IsTrue(avs.Any());
        }

        [TestMethod]
        public async Task SdAchievementPokemonTest()
        {
            var db = context.Db;

            db.SdAchievementPokemon.Add(new SdAchievementPokemon()
            {

            });

            await db.SaveChangesAsync();
            var avs = db.SdAchievementPokemon.ToList();

            Assert.IsTrue(avs.Any());
        }

        #endregion
    }
}