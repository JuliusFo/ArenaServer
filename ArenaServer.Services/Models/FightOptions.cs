using ArenaServer.Data.Common.Models;

namespace ArenaServer.Services
{
    public class FightOptions
    {
        #region Constructor

        public FightOptions(FightParticipant challenger, FightParticipant defender, bool kz_WithPokemonExchange, double participantBonus = 0)
        {
            this.Challenger = challenger;
            this.Defender = defender;
            this.Kz_WithPokemonExchange = kz_WithPokemonExchange;
            this.ParticipantBonus = participantBonus;
        }

        #endregion

        #region Properties

        public FightParticipant Challenger { get; }

        public FightParticipant Defender { get; }

        public bool Kz_WithPokemonExchange { get; }

        public double ParticipantBonus { get; }

        #endregion
    }
}