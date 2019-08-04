using ArenaServer.Bots;
using ArenaServer.Services;
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

            //Increase build version
            var buildService = new BuildService();
            buildService.IncreaseBuildVersion();

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