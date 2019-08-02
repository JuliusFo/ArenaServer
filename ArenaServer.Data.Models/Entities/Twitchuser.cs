using System;

namespace ArenaServer.Data.Models
{
    public class Twitchuser
    {
        public string Twitchuser_Id { get; set; }

        public string DisplayName { get; set; }

        public bool Kz_Log_Enabled { get; set; }

        public DateTime Dt_Last_Userfight { get; set; }
    }
}