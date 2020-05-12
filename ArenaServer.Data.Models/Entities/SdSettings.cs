using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaServer.Data.Models
{
    public class SdSettings
    {
        #region Boss-Settings

        public int Boss_PauseSecondsBetweenRounds { get; set; }

        public int Boss_PauseSecondsNotEnoughParticipants { get; set; }

        public int Boss_WaitingSecondsToJoin { get; set; }

        public int Boss_MinimumParticipants { get; set; }

        #endregion

        #region Userfight-Settings



        #endregion

        #region Command-Settings



        #endregion
    }
}