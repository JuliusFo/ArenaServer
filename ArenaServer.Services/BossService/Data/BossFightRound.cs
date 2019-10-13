using ArenaServer.Data.Transfer;
using System.Collections.Generic;

namespace ArenaServer.Services
{
    public class BossFightRound
    {
        #region Fields


        #endregion

        #region Constructor

        public BossFightRound()
        {
            this.Participants = new List<TransferTwitchuser>();
        }

        #endregion

        #region Properies

        public List<TransferTwitchuser> Participants { get;}

        public TransferPokemon BossEnemy { get; set; }

        #endregion

        #region Methods



        #endregion
    }
}