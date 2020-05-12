using System.Collections.Generic;

namespace ArenaServer.Data.Models
{
    public class SdAchievement
    {
        public decimal SdAchievement_Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string NPCName { get; set; }

        public short? UnlockedOnCount { get; set; }

        public virtual ICollection<SdAchievementPokemon> SdAchievementPokemon { get; set; }
    }
}