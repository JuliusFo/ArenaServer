using ArenaServer.Data.Transfer;
using System.Collections.Generic;

namespace ArenaServer.Data.Common.Models
{
    public class FightParticipant
    {
        public TransferTwitchuser User { get; set; }

        public List<TransferPokemon> Pokemon { get; set; } = new List<TransferPokemon>();
    }
}