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

		static void Main()
		{
			LogOutput.LogInformation("----------------------");
			LogOutput.LogInformation("ArenaBot");
			LogOutput.LogInformation("----------------------");

			//Increase build version
			var buildService = new BuildService();
			buildService.IncreaseBuildVersion();

			var twitchBot = new TwitchBot("Skei7");

			bool checkInput = true;
			while (checkInput)
			{
				var typed = Console.ReadLine();

				if (typed.ToLower() == "exit")
				{
					checkInput = false;
				}
			}

			twitchBot.Disconnect();
		}
	}
}