using ArenaServer.Data.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArenaServer.Services
{
    public class UserfightService
    {
        #region Fields

        private readonly List<UserfightRound> onGoingFights;
        private readonly List<UserfightRound> expiredFights;

        private readonly UserService userService;
        private readonly ChatOutputService chatOutputService;

        private readonly int timeUntilStartSec = 30;
        private readonly int expiredTimeoutSec = 30;

        #endregion

        #region Constructor

        public UserfightService(UserService userService, ChatOutputService chatOutputService)
        {
            this.userService = userService;
            this.chatOutputService = chatOutputService;

            onGoingFights = new List<UserfightRound>();
            expiredFights = new List<UserfightRound>();

            //Start timeout check thread
            new Thread(() => { CheckForTimeouts(); }).Start();
        }

        #endregion

        #region Properties



        #endregion

        #region Methods

        private void CheckForTimeouts()
        {
            while (true)
            {
                List<UserfightRound> expiredFightToRemoveList = new List<UserfightRound>();

                foreach(var expiredFight in expiredFights)
                {
                    if(expiredFight.CreatedDt.AddSeconds(timeUntilStartSec + expiredTimeoutSec) >= DateTimeOffset.Now)
                    {
                        //Monsters are ready to fight again
                        //TODO: Chat output!
                        expiredFightToRemoveList.Add(expiredFight);
                    }
                }

                //Find expired rounds
                foreach(var onGoingFight in onGoingFights)
                {
                    if ((onGoingFight.CreatedDt - DateTimeOffset.Now).TotalSeconds > timeUntilStartSec)
                    {
                        chatOutputService.SendMessage("@" + onGoingFight.Defender.DisplayName + " hat die Herausforderung von @" + onGoingFight.Attacker.DisplayName + " nicht angenommen. Die Runde wurde abgebrochen.");
                        expiredFights.Add(onGoingFight);
                    }
                }

                //Remove expired rounds
                foreach(var expiredFight in expiredFightToRemoveList)
                {
                    expiredFights.Remove(expiredFight);
                }

                //Wait for one second
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Start a fight between two users.
        /// </summary>
        /// <param name="challenger_user">Challenging twitch user.</param>
        /// <param name="challanged_user">Challenged twitch user.</param>
        public async Task StartFight(TransferTwitchuser challengerUser, TransferTwitchuser challengedUser)
        {
            var fightround = onGoingFights.Where(arf => arf.Defender.Equals(challengerUser) && arf.Attacker.Equals(challengedUser)).FirstOrDefault();

            if (null != fightround)
            {
                var result = await fightround.Fight();

                //Twitch output
                chatOutputService.SendMessage("Aus dem Kampf zwischen @" + challengerUser.DisplayName + " und @" + challengedUser.DisplayName + " konnte @" + result.Winner + " erfolgreich hervorgehen. @" + result.Winner + " erhält als Gewinn " + result.ExchangePokemon.Name + "!");

                //Clean up
                onGoingFights.Remove(fightround);
                await userService.SetLastFightDt(challengerUser.Id);
                await userService.SetLastFightDt(challengedUser.Id);
            }
        }

        public void CreateFightRound(TransferTwitchuser challenger_user, TransferTwitchuser challenged_user, bool isSelectedFight)
        {
            onGoingFights.Add(new UserfightRound(userService, challenger_user, challenged_user, isSelectedFight));
        }

        #region Helper

        public bool User_IsChallengedBy(TransferTwitchuser user_isChallenged, TransferTwitchuser user_isChallenging)
        {
            return onGoingFights.Where(ar => ar.Attacker.Equals(user_isChallenging) && ar.Defender.Equals(user_isChallenged)).FirstOrDefault() != null;
        }

        public bool User_IsChallenger(TransferTwitchuser challenger)
        {
            return onGoingFights.Where(ar => ar.Attacker.Equals(challenger)).FirstOrDefault() != null;
        }

        #endregion

        #endregion
    }
}