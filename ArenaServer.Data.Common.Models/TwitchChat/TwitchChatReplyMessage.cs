using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaServer.Data.Common.Models
{
    public class TwitchChatReplyMessage
    {
        #region Fields

        private readonly List<string> targetUserNames;
        private readonly string reply;

        #endregion

        #region Constructor

        public TwitchChatReplyMessage(List<string> targetUserNames, string reply)
        {
            this.targetUserNames = targetUserNames ?? new List<string>();
            this.reply = string.IsNullOrWhiteSpace(reply) ? "" : reply;
        }

        public TwitchChatReplyMessage(string targetUserName, string reply)
        {
            this.targetUserNames = new List<string>() { targetUserName };
            this.reply = string.IsNullOrWhiteSpace(reply) ? "" : reply;
        }

        #endregion

        #region Properties



        #endregion

        #region Methods

        public string ToReplyMessage()
        {
            string output = "Hallo ";

            foreach(var user in targetUserNames)
            {
                output += user;
            }

            return output += ": " + reply;
        }

        #endregion
    }
}