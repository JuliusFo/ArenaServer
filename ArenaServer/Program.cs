using ArenaServer.Bots;
using ArenaServer.Utils;
using System;

namespace ArenaServer
{
    class Program
    {
        #region Fields



        #endregion

        static void Main(string[] args)
        {
            LogOutput.LogInformation("----------------------");
            LogOutput.LogInformation("ArenaBot");
            LogOutput.LogInformation("----------------------");

            var twitchBot = new TwitchBot("Skei7");

            string typed = "";
            while (typed != "Exit")
            {
                typed = Console.ReadLine();
            }
            
            twitchBot.Disconnect();
        }
    }
}
