namespace ArenaServer.Data.Transfer
{
    public class TransferPokemon
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public float HP { get; set; }

        public PokemonRarity Rarity { get; set; }

        public PokemonType Type { get; set; }

        public float ATK { get; set; }
    }
}