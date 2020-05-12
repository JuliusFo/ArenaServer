using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArenaServer.Data.Models
{
    public class Achievements
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public decimal Achievement_Id { get; set; }

        public string Twitchuser_Id { get; set; }

        public DateTime? LastFight { get; set; }

        public decimal SdAchievment_Id { get; set; }

        public virtual SdAchievement SdAchievement { get; set; }

        public virtual Twitchuser Twitchuser { get; set; }
    }
}