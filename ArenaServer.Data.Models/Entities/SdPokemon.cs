namespace ArenaServer.Data.Models
{
    public class SdPokemon
    {
        #region Properties

        public decimal SdPokemon_Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PokemonType Type { get; set; }

        public double HP { get; set; }

        public PokemonRarity Rarity { get; set; }

        public double ATK { get; set; }

        #endregion
    }
}