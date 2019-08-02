using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaServer.Data.Common.Models
{
    public class TwitchChatMessage
    {
        public string Message { get; set; }

        public string TwitchUsername { get; set; }

        public string TwitchUserId { get; set; }
    }
}