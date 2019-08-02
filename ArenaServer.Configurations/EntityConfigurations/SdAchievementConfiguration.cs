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

            builder.HasOne(a => a.Pokemon_1).WithMany().HasForeignKey(a => a.Pokemon_Id_1);
            builder.HasOne(a => a.Pokemon_2).WithMany().HasForeignKey(a => a.Pokemon_Id_2);
            builder.HasOne(a => a.Pokemon_3).WithMany().HasForeignKey(a => a.Pokemon_Id_3);
            builder.HasOne(a => a.Pokemon_4).WithMany().HasForeignKey(a => a.Pokemon_Id_4);
            builder.HasOne(a => a.Pokemon_5).WithMany().HasForeignKey(a => a.Pokemon_Id_5);
            builder.HasOne(a => a.Pokemon_6).WithMany().HasForeignKey(a => a.Pokemon_Id_6);
        }
    }
}