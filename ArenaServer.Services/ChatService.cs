using ArenaServer.Data.Common.Models;
using ArenaServer.Utils;

namespace ArenaServer.Services
{
    public class ChatService
    {
        #region Fields

        //Bot data
        private const string name = "ArenaBot";
        private const string version = "Version 0.1.2 Alpha";

        #endregion

        #region Constructor

        public ChatService()
        {

        }

        #endregion

        #region Properties



        #endregion

        #region Methods

        #region Recieve input commands

        public string HandleCommand(TwitchChatMessage twitchChatMessage)
        {
            #region Basic commands 

            if (twitchChatMessage.Message == TwitchChatCommands.GET_VERSION)
            {
                return GetBotVersion();
            }

            //Status
            if (twitchChatMessage.Message == TwitchChatCommands.GET_STATUS)
            {
                return GetBotState();
            }

            //Arena-Info
            if (twitchChatMessage.Message == TwitchChatCommands.GET_INFO)
            {
                return GetInfo();
            }

            #endregion

            return string.Empty;
        }

        #endregion

        #region Handle input commands

        private string GetBotVersion()
        {
            LogOutput.LogInformation("User requested actual bot version.");

            return name + " " + version;
        }

        private string GetBotState()
        {
            LogOutput.LogInformation("User requested actual bot state.");

            return "Der ArenaBot ist online!";
        }

        private string GetInfo()
        {
            LogOutput.LogInformation("User requested actual bot information.");

            return "In der Poke-Arena kannst du Pokemon fangen und gegeneinander kämpfen. Registriere dich mit !registrieren Pikachu/Glumanda/Schiggy/Bisasam/Evoli oder auf arena.catmozo.de";
        }

        #endregion

        #endregion
    }
}