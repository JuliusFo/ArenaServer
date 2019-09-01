using ArenaServer.Data.Common.Models;

namespace ArenaServer.Services
{
    public class UserRegistrationResponse
    {
        #region Fields



        #endregion

        #region Constructor

        public UserRegistrationResponse()
        {

        }

        #endregion

        #region Properties

        public bool RegistrationSuccessfull { get; set; }

        public bool UserAlreadyRegistered { get; set; }

        #endregion
    }
}