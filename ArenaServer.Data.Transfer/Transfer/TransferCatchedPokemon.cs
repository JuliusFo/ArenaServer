namespace ArenaServer.Data.Transfer
{
    public class TransferCatchedPokemon
    {
        public TransferPokemon Pokemon { get; set; }

        public int AmountCatched { get; set; }

        public int AmountOnFightingTeam { get; set; }
    }
}