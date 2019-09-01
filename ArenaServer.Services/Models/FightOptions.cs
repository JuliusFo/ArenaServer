using ArenaServer.Data.Common.Models;

namespace ArenaServer.Services
{
    public class FightOptions
    {
        #region Constructor

        public FightOptions(FightParticipant challenger, FightParticipant defender, bool kz_WithPokemonExchange)
        {
            this.Challenger = challenger;
            this.Defender = defender;
            this.Kz_WithPokemonExchange = kz_WithPokemonExchange;
        }

        #endregion

        #region Properties

        public FightParticipant Challenger { get; }

        public FightParticipant Defender { get; }

        public bool Kz_WithPokemonExchange { get; }

        #endregion
    }
}