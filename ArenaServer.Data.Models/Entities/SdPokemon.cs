namespace ArenaServer.Data.Models
{
    public class SdPokemon
    {
        public decimal SdPokemon_Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PokemonType Type { get; set; }

        public float HP { get; set; }

        public PokemonRarity Rarity { get; set; }

        public float ATK { get; set; }
    }
}