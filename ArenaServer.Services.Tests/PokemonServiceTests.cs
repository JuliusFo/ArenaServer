using ArenaServer.Data;
using ArenaServer.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace ArenaServer.Services.Tests
{
    [TestClass]
    public class PokemonServiceTests
    {
        #region Fields

        private TestingDBContext context;
        private UserService userService;

        #endregion

        #region Initialize

        [TestInitialize]
        public void Initialize()
        {
            context = new TestingDBContext();
            userService = new UserService(context.Db);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public async Task UserRegistrationState_Unregistered()
        {
            var isUserRegistered = await userService.IsUserRegistered("UNIT TEST: NOPE");

            Assert.IsFalse(isUserRegistered);
        }

        [TestMethod]
        public async Task UserRegistrationState_Registered()
        {
            var testUser = new Twitchuser()
            {
                Twitchuser_Id = "UNIT TEST: YES",
                DisplayName = "XDMEMES",
                Kz_Log_Enabled = true
            };

            context.Db.Twitchuser.Add(testUser);
            await context.Db.SaveChangesAsync();

            var isUserRegistered = await userService.IsUserRegistered(testUser.Twitchuser_Id);

            Assert.IsTrue(isUserRegistered);
        }

        #endregion
    }
}