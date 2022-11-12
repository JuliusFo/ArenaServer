using System;
using System.Collections.Generic;

namespace ArenaServer.Data.Transfer
{
    public class TransferTwitchuser
    {
        #region Properties

        public string Id { get; set; }

        public string DisplayName { get; set; }

        public bool KzLogEnabled { get; set; }

        public DateTime? LastUserFight;

        public List<TransferCatchedPokemon> CatchedPokemonList { get; set; } = new List<TransferCatchedPokemon>();

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            TransferTwitchuser other = obj as TransferTwitchuser;
            if (other == null)
            {
                return false;
            }
            return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}