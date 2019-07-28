using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ArenaServer.Data.Tests
{
    [TestClass]
    public class EntityTests
    {
        [TestMethod]
        public void TwitchuserTest()
        {
            var db = new AppDbContext();

            db.Twitchuser.Add(new Twitchuser()
            {
                DisplayName = "UNIT TEST",
                Kz_Log_Enabled = false,
                Twitchuser_Id = "00000"
            });


            var existing_users = db.Twitchuser.Local.ToList();

            Assert.IsTrue(existing_users.Any());
            Assert.IsTrue(existing_users[0].DisplayName == "UNIT TEST");
        }

        [TestMethod]
        public void SdPokemonTest()
        {
            var db = new AppDbContext();

            db.SdPokemon.Add(new SdPokemon()
            {
                Name = "UNIT TEST",
                ATK = 100,
                Description = "UNIT TEST",
                HP = 100,
                Rarity = PokemonRarity.Common,
                Type = PokemonType.Bug
            });


            var existing_pokemon = db.SdPokemon.Local.ToList();

            Assert.IsTrue(existing_pokemon.Any());
            Assert.IsTrue(existing_pokemon[0].Name == "UNIT TEST");
        }

        [TestMethod]
        public void SdAchievementTest()
        {
            var db = new AppDbContext();

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

            var existing_achievements = db.SdAchievement.Local.ToList();

            Assert.IsTrue(existing_achievements.Any());
            Assert.IsTrue(existing_achievements[0].Name == "UNIT TEST");
        }

        [TestMethod]
        public void CatchedPokemonTest()
        {
            var db = new AppDbContext();

            db.CatchedPokemon.Add(new CatchedPokemon()
            {
                Pokemon_AmountCatched = 1,
                Pokemon_AmountOnFightingTeam = 1,
                SdPokemon_Id = 0,
                Twitchuser_Id = "0"
            });

            var catched_pokemon = db.CatchedPokemon.Local.ToList();

            Assert.IsTrue(catched_pokemon.Any());
        }

        [TestMethod]
        public void AchievementsTest()
        {
            var db = new AppDbContext();

            db.Achievements.Add(new Achievements()
            {
                Twitchuser_Id = "",
                SdAchievment_Id = 0,
                 LastFight = DateTime.Now
            });

            var avs = db.Achievements.Local.ToList();

            Assert.IsTrue(avs.Any());
        }
    }
}