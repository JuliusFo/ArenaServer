using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaServer.Services
{
    public class BossOutputFormatter
    {
        #region Fields

        private const string notEnoughParticipants = "Es konnten nicht genug Pokemon für einen Kampf gefunden werden. Sammelt euch und versucht es in {0} Minuten erneut!";
        private const string bossAppears = "Ein wildes Pokemon streift durch die Gegend. Schreibe !boss, um den Kampf aufzunehmen!";
        private const string bossNoWinners = "Keiner der Trainer konnte {0} besiegen. Versucht es in {1} Minuten noch einmal!";
        private const string bossWinners = "Die Pokemon von folgenden Trainern haben den Kampf gegen {0} gewonnen und konnten es dadurch fangen: ";

        #endregion

        #region Constructor

        public BossOutputFormatter()
        {

        }

        #endregion

        #region Properties



        #endregion

        #region Methods

        public string GetOutput_NotEnoughParticipants(int pauseMinutes)
        {
            return string.Format(notEnoughParticipants, pauseMinutes);
        }

        public string GetOutput_BossAppears()
        {
            return string.Format(bossAppears);
        }

        public string GetOuput_BossNoWinners(string bossEnemyName, int bossCountdownTime)
        {
            return string.Format(bossNoWinners, bossEnemyName, bossCountdownTime);
        }

        public string GetOutput_BossWinners(string bossEnemyName)
        {
            return string.Format(bossWinners, bossEnemyName);
        }

        #endregion
    }
}