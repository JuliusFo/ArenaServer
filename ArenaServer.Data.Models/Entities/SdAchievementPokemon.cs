namespace ArenaServer.Data
{
    public class SdAchievementPokemon
    {
        public decimal SdAchievement_Id { get; set; }

        public decimal SdPokemon_Id { get; set; }

        public virtual SdAchievement SdAchievement { get; set; }

        public virtual SdPokemon SdPokemon { get; set; }
    }
}