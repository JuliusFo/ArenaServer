using System;

namespace ArenaServer.Data.Transfer
{
    public class TransferTwitchuser
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public bool KzLogEnabled { get; set; }

        public DateTime LastUserFight;

        //public List<TransferCatchedPokemon> CatchedPokemonList { get; set; } = new List<TransferCatchedPokemon>();
    }
}