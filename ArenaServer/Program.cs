using ArenaServer.Bots;
using System;

namespace ArenaServer
{
    class Program
    {
        #region Fields



        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("ArenaBot");
            Console.WriteLine("----------------------");

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
