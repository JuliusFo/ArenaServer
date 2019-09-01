using ArenaServer.Data.Transfer;

namespace ArenaServer.Services
{
    public class FightResult
    {
        #region Constructor

        public FightResult(TransferTwitchuser winner, TransferPokemon exchangePokemon)
        {
            this.Winner = winner;
            this.ExchangePokemon = exchangePokemon;
        }

        #endregion

        #region Properties

        public TransferTwitchuser Winner { get; }

        public TransferPokemon ExchangePokemon { get; }

        #endregion
    }
}