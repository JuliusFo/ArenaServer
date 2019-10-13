using ArenaServer.Data;
using ArenaServer.Data.Common.Models;
using ArenaServer.Data.Transfer;
using ArenaServer.Utils;
using System.Threading.Tasks;

namespace ArenaServer.Services
{
    public class ChatInputService
    {
        #region Fields

        //Bot data
        private const string name = "ArenaBot";
        private const string version = "Version 0.1.2 Alpha";

        //Services
        private readonly UserService userService;
        private readonly ChatOutputService chatOutputService;
        private readonly BossService bossService;

        #endregion

        #region Constructor

        public ChatInputService(
            UserService userService,
            ChatOutputService chatOutputService,
            BossService bossService)
        {
            this.userService = userService;
            this.chatOutputService = chatOutputService;
            this.bossService = bossService;
        }

        #endregion

        #region Properties



        #endregion

        #region Methods

        #region Recieve input commands

        public async Task<TwitchChatReplyMessage> HandleCommand(TwitchChatMessage twitchChatMessage)
        {
            #region Basic commands 

            //Version
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

            //Commands
            if(twitchChatMessage.Message == TwitchChatCommands.GET_COMMANDS)
            {
                return GetCommands(twitchChatMessage);
            }

            #endregion

            #region Account commands

            if (twitchChatMessage.Message.StartsWith(TwitchChatCommands.REGISTER))
            {
                return await RegisterUser(twitchChatMessage);
            }

            #endregion

            #region Fight commands

            if (twitchChatMessage.Message.StartsWith(TwitchChatCommands.BOSS))
            {
                return await ParticipateInBossFight(twitchChatMessage);
            }

            #endregion

            if (twitchChatMessage.Message.StartsWith("!"))
            {
                LogOutput.LogInformation("User requested unknown command: " + twitchChatMessage.Message);

                return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, TwitchChatResponse.UNKNOWN_COMMAND);
            }
            return null;
        }

        #endregion

        #region Handle input commands

        #region Standard

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

        private TwitchChatReplyMessage GetCommands(TwitchChatMessage twitchChatMessage)
        {
            LogOutput.LogInformation("User requested command information.");

            return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, TwitchChatResponse.GET_COMMANDS);
        }

        #endregion

        #region User account

        private async Task<TwitchChatReplyMessage> RegisterUser(TwitchChatMessage twitchChatMessage)
        {
            LogOutput.LogInformation($"User requested registration: {twitchChatMessage.TwitchUsername}, ID {twitchChatMessage.TwitchUserId}");

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
                var response = await userService.RegisterUser(twitchChatMessage.TwitchUserId, twitchChatMessage.TwitchUsername, registerDetails);

                if (response.RegistrationSuccessfull)
                {
                    return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, TwitchChatResponse.REG_SUCCESS);
                }
                else
                {
                    if (response.UserAlreadyRegistered)
                    {
                        return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, TwitchChatResponse.REG_ALREADY_REG);
                    }
                }

                return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, TwitchChatResponse.REG_ERROR_UNKNOWN);
            }
        }

        #endregion

        #region Fighting

        private async Task<TwitchChatReplyMessage> ParticipateInBossFight(TwitchChatMessage twitchChatMessage)
        {
            LogOutput.LogInformation($"[Bossfight] User requested to particpate in boss fight: {twitchChatMessage.TwitchUsername}, ID {twitchChatMessage.TwitchUserId}");

            TransferTwitchuser user_entered_battle = await userService.GetUser(twitchChatMessage.TwitchUserId);

            if (user_entered_battle == null)
            {
                LogOutput.LogInformation($"[Bossfight] User is not registered: {twitchChatMessage.TwitchUsername}, ID {twitchChatMessage.TwitchUserId}");
                return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, "Du bist noch nicht registriert. Schreibe !registrieren [Glumanda/Schiggy/Bisasam/Pikachu/Evoli] in den Chat, um dich zu registrieren.");
            }

            if (!bossService.IsBattleReady())
            {
                LogOutput.LogInformation($"[Bossfight] Battle is not ready: {twitchChatMessage.TwitchUsername}, ID {twitchChatMessage.TwitchUserId}");
                return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, "Aktuell streift kein Pokemon durch die Gegend. Versuche es in " + bossService.GetRemainingCoolDown().Minutes + " Minute(n) und " + bossService.GetRemainingCoolDown().Seconds + " Sekunde(n) erneut.");;
            }

            if (!bossService.IsBattleWaiting())
            {
                LogOutput.LogInformation($"[Bossfight] Creating a new boss round: {twitchChatMessage.TwitchUsername}, ID {twitchChatMessage.TwitchUserId}");
                bossService.StartNewBattleRound();
                chatOutputService.SendMessage("@" + twitchChatMessage.TwitchUsername + " hat ein wildes Pokemon entdeckt! Schreibe !boss in den Chat, um ihm im Kampf beizustehen.");
            }

            bossService.AddUserToCurrentRound(user_entered_battle);

            return null;
        }

        #endregion

        #endregion

        #endregion
    }
}