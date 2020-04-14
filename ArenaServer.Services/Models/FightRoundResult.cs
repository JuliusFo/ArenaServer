using ArenaServer.Data.Common.Models;

namespace ArenaServer.Services.Models
{
    public class FightRoundResult
    {
        #region Constructor

        public FightRoundResult(FightParticipant winner)
        {
            this.Winner = winner;
        }

        #endregion

        #region Properties

        public FightParticipant Winner { get; }

        #endregion
    }
}