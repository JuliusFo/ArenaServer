using ArenaServer.Data.Transfer;
using System;
using System.Collections.Generic;

namespace ArenaServer.Services.BossService
{
    public class BossFightRound
    {
        #region Fields


        #endregion

        #region Constructor

        public BossFightRound(DateTime startTime, List<TransferTwitchuser> participants, TransferPokemon bossEnemy)
        {
            StartTime = DateTime.Now;
            Participants = new List<TransferTwitchuser>();
            BossEnemy = null;
        }

        #endregion

        #region Properies

        public DateTime? StartTime { get;  }

        public List<TransferTwitchuser> Participants { get; }

        public TransferPokemon BossEnemy { get; }

        #endregion

        #region Methods



        #endregion
    }
}