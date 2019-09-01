using System.Collections.Generic;
using System.Numerics;

namespace ArenaServer.Data.Models
{
    public class SdAchievement
    {
        public decimal SdAchievement_Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string NPCName { get; set; }

        public decimal Pokemon_Id_1 { get; set; }

        public decimal Pokemon_Id_2 { get; set; }

        public decimal Pokemon_Id_3 { get; set; }

        public decimal Pokemon_Id_4 { get; set; }

        public decimal Pokemon_Id_5 { get; set; }

        public decimal Pokemon_Id_6 { get; set; }

        public short? UnlockedOnCount { get; set; }

        public virtual SdPokemon Pokemon_1 { get; set; }

        public virtual SdPokemon Pokemon_2 { get; set; }

        public virtual SdPokemon Pokemon_3 { get; set; }

        public virtual SdPokemon Pokemon_4 { get; set; }

        public virtual SdPokemon Pokemon_5 { get; set; }

        public virtual SdPokemon Pokemon_6 { get; set; }

        public virtual ICollection<SdAchievementPokemon> SdAchievementPokemon { get; set; }
    }
}