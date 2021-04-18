using ArenaServer.Data;
using ArenaServer.Data.Common.Models;
using ArenaServer.Data.Common.Models.Extensions;
using ArenaServer.Data.Transfer;
using ArenaServer.Services.UserFightService;
using ArenaServer.Utils;
using System;
using System.Threading.Tasks;

namespace ArenaServer.Services
{
	public class ChatInputService
	{
		#region Fields

		//Bot data
		private const string name = "ArenaBot";
		private const string version = "Version 0.2 Alpha";

		//Services
		private readonly UserService userService;
		private readonly ChatOutputService chatOutputService;
		private readonly BossService bossService;
		private readonly UserfightService userfightService;
		private readonly AchievementService achievementService;

		#endregion

		#region Constructor

		public ChatInputService(
			UserService userService,
			ChatOutputService chatOutputService,
			BossService bossService,
			UserfightService userfightService,
			AchievementService achievementService)
		{
			this.userService = userService;
			this.chatOutputService = chatOutputService;
			this.bossService = bossService;
			this.userfightService = userfightService;
			this.achievementService = achievementService;
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
			if (twitchChatMessage.Message.ToLower() == TwitchChatCommands.GET_VERSION)
			{
				return GetBotVersion(twitchChatMessage);
			}

			//Status
			if (twitchChatMessage.Message.ToLower() == TwitchChatCommands.GET_STATUS)
			{
				return GetBotState(twitchChatMessage);
			}

			//Arena-Info
			if (twitchChatMessage.Message.ToLower() == TwitchChatCommands.GET_INFO)
			{
				return GetInfo(twitchChatMessage);
			}

			//Commands
			if (twitchChatMessage.Message.ToLower() == TwitchChatCommands.GET_COMMANDS)
			{
				return GetCommands(twitchChatMessage);
			}

			#endregion

			#region Account commands

			if (twitchChatMessage.Message.ToLower().StartsWith(TwitchChatCommands.REGISTER))
			{
				return await RegisterUser(twitchChatMessage);
			}

			#endregion

			#region Fight commands

			if (twitchChatMessage.Message.ToLower().StartsWith(TwitchChatCommands.BOSS))
			{
				return await ParticipateInBossFight(twitchChatMessage);
			}

			if (twitchChatMessage.Message.ToLower().StartsWith(TwitchChatCommands.USERFIGHT))
			{
				return await ParticipateInUserFight(twitchChatMessage, false);
			}

			if (twitchChatMessage.Message.ToLower().StartsWith(TwitchChatCommands.TEAMUSERFIGHT))
			{
				return await ParticipateInUserFight(twitchChatMessage, true);
			}

			if (twitchChatMessage.Message.ToLower().StartsWith(TwitchChatCommands.AVFIGHT))
			{
				return await ChallengeNPC(twitchChatMessage);
			}

			#endregion

			if (twitchChatMessage.Message.ToLower().StartsWith("!"))
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
			LogOutput.LogInformation($"[Registration] User requested registration: {twitchChatMessage.TwitchUsername}, ID {twitchChatMessage.TwitchUserId}");

			var registerDetails = twitchChatMessage.Message.ToLower().Replace(TwitchChatCommands.REGISTER, "");
			if (!(registerDetails.Equals("glumanda")
				|| registerDetails.Equals("schiggy")
				|| registerDetails.Equals("bisasam")
				|| registerDetails.Equals("pikachu")
				|| registerDetails.Equals("evoli")))
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
				return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, "Aktuell streift kein Pokemon durch die Gegend. Versuche es in " + bossService.GetRemainingCoolDown().Minutes + " Minute(n) und " + bossService.GetRemainingCoolDown().Seconds + " Sekunde(n) erneut."); ;
			}

			if (!bossService.IsBattleWaiting())
			{
				LogOutput.LogInformation($"[Bossfight] Creating a new boss round: {twitchChatMessage.TwitchUsername}, ID {twitchChatMessage.TwitchUserId}");
				bossService.StartNewBattleRound();
				chatOutputService.SendMessage(twitchChatMessage.TwitchUsername + " hat ein wildes Pokemon entdeckt! Schreibe !boss in den Chat, um ihm im Kampf beizustehen.");
			}

			bossService.AddUserToCurrentRound(user_entered_battle);
			LogOutput.LogInformation($"[Bossfight] User entered bossfight: {twitchChatMessage.TwitchUsername}, ID {twitchChatMessage.TwitchUserId}");

			return null;
		}

		private async Task<TwitchChatReplyMessage> ParticipateInUserFight(TwitchChatMessage twitchChatMessage, bool teamFight)
		{
			LogOutput.LogInformation($"[Userfight] User requested to participate in an user fight: {twitchChatMessage.TwitchUsername}, ID {twitchChatMessage.TwitchUserId}");

			var challengingUser = await userService.GetUser(twitchChatMessage.TwitchUserId);
			var challengingUserName = twitchChatMessage.TwitchUsername;
			var challengedUser = await userService.GetUserByName(twitchChatMessage);
			var challengedUserName = twitchChatMessage.GetTargetUserName();

			//Not registered
			var usersExisting = UserfightCheckService.CheckUsersExisting(challengingUser, challengingUserName, challengedUser, challengedUserName);
			if (null != usersExisting)
			{
				return usersExisting;
			}

			//Full fighting team
			var fullFightingTeam = UserfightCheckService.CheckUserHasTeam(challengingUser, challengedUser, teamFight);
			if (null != fullFightingTeam)
			{
				return fullFightingTeam;
			}

			//Cooldown
			var usersOnCooldown = UserfightCheckService.CheckUserTimeout(challengingUser, challengedUser, new TimeSpan(0, 10, 0));
			if (null != usersOnCooldown)
			{
				return usersOnCooldown;
			}

			//Challenger already challenging another person
			if (userfightService.User_IsChallenger(challengingUser))
			{
				return new TwitchChatReplyMessage(challengedUser.DisplayName, " du hast bereits einen anderen Kampf gestartet. Bitte warte, bis dieser Kampf zu Ende ist.");
			}

			//Check if the challenged player already is challenged by the challenger
			if (userfightService.User_IsChallengedBy(challengingUser, challengedUser))
			{
				chatOutputService.SendMessage(challengingUser.DisplayName + " hat die Herausforderung von " + challengedUser.DisplayName + " angenommen!");
				await userfightService.StartFight(challengingUser, challengedUser);
			}
			else
			{
				chatOutputService.SendMessage(challengingUser.DisplayName + " fordert " + challengedUser.DisplayName + " zu einem Duell heraus! Schreibe !fight " + challengingUser.DisplayName + ", um den Kampf anzunehmen!");

				//None of the users are in a fight or waiting for each other. Start a new fight round.
				userfightService.CreateFightRound(challengingUser, challengedUser, teamFight);
			}

			return null;
		}

		private async Task<TwitchChatReplyMessage> ChallengeNPC(TwitchChatMessage twitchChatMessage)
		{
			LogOutput.LogInformation($"[AV-Fight] User requested to challenge a npc: {twitchChatMessage.TwitchUsername}, ID {twitchChatMessage.TwitchUserId}");

			var user = await userService.GetUser(twitchChatMessage.TwitchUserId);
			var avName = twitchChatMessage.GetTargetUserName();

			if (null == user)
			{
				return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, "Du bist noch nicht registriert. Schreibe !registrieren [Glumanda/Schiggy/Bisasam/Pikachu/Evoli] in den Chat, um dich zu registrieren.");
			}

			if (!user.HasFullFightingTeam())
			{
				return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, "Du hast noch kein vollständiges Team, fange weitere Pokemon!");
			}

			if (await userService.CanFightAcievement(user.Id, avName))
			{
				var virtualNPC = await achievementService.CreateVirtualNPC(avName);
				chatOutputService.SendMessage($"{user.DisplayName} fordert {virtualNPC.DisplayName} heraus!");

				await userfightService.StartNPCFight(user, virtualNPC, avName);
			}
			else
			{
				return new TwitchChatReplyMessage(twitchChatMessage.TwitchUsername, "Du hast dieses Achievement noch nicht freigeschaltet oder musst noch etwas bis zur nächsten Herausforderung warten!");
			}

			return null;
		}

		#endregion

		#endregion

		#endregion
	}
}