using ArenaServer.Data.Common.Models;
using ArenaServer.Data.Common.Models.Constants;
using ArenaServer.Data.Transfer;
using ArenaServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArenaServer.Services
{
	public class BossService
	{
		#region Fields

		//Services
		private readonly UserService userService;
		private readonly PokemonService pokemonService;
		private readonly ChatOutputService chatOutputService;
		private readonly BossOutputFormatter bossOutputFormatter;
		private readonly SettingsService settingsService;

		//Settings
		private TimeSpan cooldownTime;
		private BossFightRound currentRound;

		#endregion

		#region Constructor

		public BossService(UserService userService, PokemonService pokemonService, ChatOutputService chatOutputService, SettingsService settingsService)
		{
			this.userService = userService;
			this.pokemonService = pokemonService;
			this.chatOutputService = chatOutputService;
			this.settingsService = settingsService;

			bossOutputFormatter = new BossOutputFormatter();
			cooldownTime = new TimeSpan(0, 0, 0);
		}

		#endregion

		#region Properties


		#endregion

		#region Methods   

		#region Pre-round methods

		public bool IsBattleReady()
		{
			return cooldownTime.TotalSeconds <= 0;
		}

		public void StartNewBattleRound()
		{
			currentRound = new BossFightRound();
			Task.Run(WaitForBattleStart);
		}

		public void AddUserToCurrentRound(TransferTwitchuser enteredUser)
		{
			if (currentRound == null || currentRound.Participants.Contains(enteredUser))
			{
				return;
			}

			currentRound.Participants.Add(enteredUser);
		}

		public TimeSpan GetRemainingCoolDown()
		{
			return cooldownTime;
		}

		public bool IsBattleWaiting()
		{
			return currentRound != null;
		}

		private async void WaitForBattleStart()
		{
			var waitedSeconds = 0;
			var waitingSecondsToJoin = await settingsService.GetIntegerSetting(SettingNames.BOSS_WAITING_SECONDS_TO_JOIN);

			while (waitedSeconds <= waitingSecondsToJoin)
			{
				waitedSeconds += 1;
				await Task.Delay(1000);
			}
			StartFight();
		}

		#endregion Pre-round methods

		private async void StartFight()
		{
			var minimumParticipants = await settingsService.GetIntegerSetting(SettingNames.BOSS_PARTICIPANT_MINIMUM);
			var pauseSecondsNotEnoughParticipants = await settingsService.GetIntegerSetting(SettingNames.BOSS_WAITING_SECONDS_AFTER_FAILED_ROUND);

			if (currentRound.Participants.Count < minimumParticipants)
			{
				LogOutput.LogInformation("[Bossfight] Abort boss round as not enough participants were found.");
				chatOutputService.SendMessage(bossOutputFormatter.GetOutput_NotEnoughParticipants(pauseSecondsNotEnoughParticipants / 60));
				cooldownTime = TimeSpan.FromSeconds(pauseSecondsNotEnoughParticipants);
				currentRound = null;
			}
			else
			{
				LogOutput.LogInformation("[Bossfight] Creating a new boss round.");
				currentRound.BossEnemy = await pokemonService.GetRandomPokemonWithParticipantCount(currentRound.Participants.Count);

				await CalculateFight();
				cooldownTime = TimeSpan.FromSeconds(await settingsService.GetIntegerSetting(SettingNames.BOSS_WAITING_SECONDS_BETWEEN_ROUNDS));
			}

			new Thread(() =>
			{
				DecreaseCooldownTime();
			}).Start();
		}

		private void DecreaseCooldownTime()
		{
			while (cooldownTime.TotalSeconds > 0)
			{
				cooldownTime = cooldownTime.Subtract(TimeSpan.FromSeconds(1));
				Thread.Sleep(1000);
			}

			chatOutputService.SendMessage(bossOutputFormatter.GetOutput_BossAppears());
		}

		private async Task CalculateFight()
		{
			List<TransferTwitchuser> winners = new List<TransferTwitchuser>();
			var bossOriginalHP = currentRound.BossEnemy.HP;
			double atk_participants_bonus;

			if (currentRound.Participants.Count >= 10)
			{
				atk_participants_bonus = await settingsService.GetFloatSetting(SettingNames.BOSS_PARTICIPANT_BONUS);
			}
			else
			{
				atk_participants_bonus = (await settingsService.GetFloatSetting(SettingNames.BOSS_PARTICIPANT_BONUS_U10)) * currentRound.Participants.Count;
			}

			LogOutput.LogInformation("[Bossfight] Calculating boss fight. Boss Pokemon:" + currentRound.BossEnemy.Name + ", HP:" + currentRound.BossEnemy.HP);

			//Fight
			for (int i = 0; i < currentRound.Participants.Count; i++)
			{
				//Reset fight values
				currentRound.BossEnemy.HP = bossOriginalHP / 2;

				var fightOptions = new FightOptions(currentRound.Participants[i].ToFightParticipantRandomPokemon(), CreateBossParticipant(currentRound.BossEnemy), false, atk_participants_bonus);
				var result = new Fight().CalculateUnlimited(fightOptions);

				if (result.Winner.Equals(currentRound.Participants[i]))
				{
					winners.Add(currentRound.Participants[i]);
				}
			}

			//Register all winner pokemon and write chat msg
			#region Register all winner pokemon and write chat msg

			string output_message;
			if (winners.Any())
			{
				output_message = bossOutputFormatter.GetOutput_BossWinners(currentRound.BossEnemy.Name);

				for (int w = 0; w < winners.Count; w++)
				{
					await userService.AddPokemon(winners[w].Id, currentRound.BossEnemy, false);

					var avResult = await userService.CheckAndAddAchievement(winners[w].Id);

					if (avResult.AchievementUnlocked == true)
					{
						foreach (var unlockedAV in avResult.UnlockedAchievements)
						{
							chatOutputService.SendMessage(bossOutputFormatter.GetOutput_AvUnlocked(winners[w].DisplayName, unlockedAV.AchievementName, unlockedAV.NPCName));
						}
					}

					output_message += winners[w].DisplayName;
					if (!(w + 1).Equals(winners.Count)) output_message += ", ";
				}
				output_message += ".";
			}
			else
			{
				output_message = bossOutputFormatter.GetOuput_BossNoWinners(currentRound.BossEnemy.Name, await settingsService.GetIntegerSetting(SettingNames.BOSS_WAITING_SECONDS_BETWEEN_ROUNDS) / 60);
			}

			//Chat output
			chatOutputService.SendMessage(output_message);
			LogOutput.LogInformation("[Bossfight] Boss fight ended. Winners:" + string.Join(",", winners.Select(w => w.DisplayName)));

			#endregion Register all winner pokemon and write chat msg

			//Clean up
			currentRound = null;
		}

		#region Helpers

		private FightParticipant CreateBossParticipant(TransferPokemon transferPokemon)
		{
			return new FightParticipant()
			{
				Pokemon = new List<TransferPokemon>() { transferPokemon },
				User = new TransferTwitchuser()
				{
					Id = "Boss",
					DisplayName = "Boss",
					CatchedPokemonList = new List<TransferCatchedPokemon>(),
					KzLogEnabled = false
				}
			};
		}

		#endregion

		#endregion
	}
}