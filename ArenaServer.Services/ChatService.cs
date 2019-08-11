using ArenaServer.Data.Common.Models;
using ArenaServer.Utils;
using System.Threading.Tasks;

namespace ArenaServer.Services
{
    public class ChatService
    {
        #region Fields

        //Bot data
        private const string name = "ArenaBot";
        private const string version = "Version 0.1.2 Alpha";

        //Services
        private readonly UserService userService;

        #endregion

        #region Constructor

        public ChatService()
        {
            this.userService = new UserService();
        }

        #endregion

        #region Properties



        #endregion

        #region Methods

        #region Recieve input commands

        public async Task<TwitchChatReplyMessage> HandleCommand(TwitchChatMessage twitchChatMessage)
        {
            #region Basic commands 

            if (twitchChatMessage.Message == TwitchChatCommands.GET_VERSION)
            {
                return GetBotVersion(twitchChatMessage);
            }

            //Status
            if (twitchChatMessage.Message == TwitchChatCommands.GET_STATUS)
            {
                return GetBotState(twitchChatMessage);
            }

            //Arena-Info
            if (twitchChatMessage.Message == TwitchChatCommands.GET_INFO)
            {
                return GetInfo(twitchChatMessage);
            }

            #endregion

            #region Account commands

            if (twitchChatMessage.Message.StartsWith(TwitchChatCommands.REGISTER))
            {
                return await RegisterUser(twitchChatMessage);
            }

            #endregion

            return null;
        }

        #endregion

        #region Handle input commands

        private TwitchChatReplyMessage GetBotVersion(TwitchChatMessage twitchChatMessage)
        {
            LogOutput.LogInformation("User requested actual bot version.");

            return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, name + " " + version);
        }

        private TwitchChatReplyMessage GetBotState(TwitchChatMessage twitchChatMessage)
        {
            LogOutput.LogInformation("User requested actual bot state.");

            return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, TwitchChatResponse.GET_STATE_ONLINE);
        }

        private TwitchChatReplyMessage GetInfo(TwitchChatMessage twitchChatMessage)
        {
            LogOutput.LogInformation("User requested actual bot information.");

            return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, TwitchChatResponse.GET_ARENA_INFO);
        }

        private async Task<TwitchChatReplyMessage> RegisterUser(TwitchChatMessage twitchChatMessage)
        {
            var registerDetails = twitchChatMessage.Message.Replace(TwitchChatCommands.REGISTER, "");
            if (!(registerDetails.Equals("Glumanda")
                || registerDetails.Equals("Schiggy")
                || registerDetails.Equals("Bisasam")
                || registerDetails.Equals("Pikachu")
                || registerDetails.Equals("Evoli")))
            {
                return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, TwitchChatResponse.REG_ERROR_NOTVALIDMSG);
            }
            else
            {
                var success = await userService.RegisterUser(twitchChatMessage.TwitchUserId, twitchChatMessage.TwitchUsername, registerDetails);

                if (success)
                {
                    return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, TwitchChatResponse.REG_SUCCESS);
                }

                return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, TwitchChatResponse.REG_ERROR_UNKNOWN);
            }
        }

        #endregion

        #endregion
    }
}