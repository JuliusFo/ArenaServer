using ArenaServer.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaServer.Tests.Common
{
    public static class CommonTestData
    {
        public static Twitchuser GetRandomTwitchUser(string id)
        {
            return new Twitchuser()
            {
                Twitchuser_Id = id,
                DisplayName = "UNIT TEST USER",
                Kz_Log_Enabled = false,
                Dt_Last_Userfight = null
            };
        }
    }
}