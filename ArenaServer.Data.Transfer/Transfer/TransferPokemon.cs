using ArenaServer.Data.Models;

namespace ArenaServer.Data.Transfer
{
    public class TransferPokemon
    {
        #region Properties

        public string Name { get; set; }

        public string Description { get; set; }

        public double HP { get; set; }

        public PokemonRarity Rarity { get; set; }

        public PokemonType Type { get; set; }

        public double ATK { get; set; }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            var item = obj as TransferPokemon;

            if (item == null)
            {
                return false;
            }

            return this.Name.Equals(item.Name);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        #endregion
    }
}