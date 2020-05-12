namespace ArenaServer.Services
{
    public class UserAchievementResponseItem
    {
        public UserAchievementResponseItem(string achievementName, string npcName)
        {
            this.AchievementName = achievementName;
            this.NPCName = npcName;
        }

        public string AchievementName { get; }

        public string NPCName { get; }
    }
}