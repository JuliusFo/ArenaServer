using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaServer.Data.Common.Models.Extensions
{
    public static class TwitchChatMessageExtensions
    {
        public static string GetTargetUserName(this TwitchChatMessage message)
        {
            string result;
            var filteredMessage = message.Message.Replace("@", "");
            
            try
            {
                result = filteredMessage.Split(new string[] { " " }, StringSplitOptions.None)[1];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }

            return result;
        }
    }
}