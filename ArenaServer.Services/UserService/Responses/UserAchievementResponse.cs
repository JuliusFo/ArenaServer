using System.Collections.Generic;

namespace ArenaServer.Services
{
    public class UserAchievementResponse
    {
        public UserAchievementResponse(bool achievementUnlocked, IEnumerable<UserAchievementResponseItem> unlockedAchievements)
        {
            this.AchievementUnlocked = achievementUnlocked;
            this.UnlockedAchievements = unlockedAchievements;
        }


        public bool AchievementUnlocked { get; set; }

        public IEnumerable<UserAchievementResponseItem> UnlockedAchievements { get; set; }
    }
}