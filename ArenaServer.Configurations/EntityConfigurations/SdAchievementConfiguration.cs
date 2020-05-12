using ArenaServer.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArenaServer.Data.Configurations
{
    public class SdAchievementConfiguration : IEntityTypeConfiguration<SdAchievement>
    {
        public void Configure(EntityTypeBuilder<SdAchievement> builder)
        {
            builder.ToTable("SdAchievement").HasKey(u => u.SdAchievement_Id);
        }
    }
}