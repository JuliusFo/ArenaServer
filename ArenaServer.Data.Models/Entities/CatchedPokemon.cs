namespace ArenaServer.Data
{
    public class CatchedPokemon
    {
        public decimal CatchedPokemon_Id { get; set; }

        public int Pokemon_AmountCatched { get; set; }

        public int Pokemon_AmountOnFightingTeam { get; set; }

        public decimal SdPokemon_Id { get; set; }

        public string Twitchuser_Id { get; set; }

        public virtual SdPokemon SdPokemon { get; set; }

        public virtual Twitchuser Twitchuser { get; set; }
    }
}