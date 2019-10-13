using ArenaServer.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ArenaServer.Data.Configurations
{
    public class TwitchuserConfiguration : IEntityTypeConfiguration<Twitchuser>
    {
        public void Configure(EntityTypeBuilder<Twitchuser> builder)
        {
            builder.ToTable("Twitchuser").HasKey(u => u.Twitchuser_Id);

            builder.Property(u => u.Kz_Log_Enabled).HasConversion(new BoolToZeroOneConverter<int>());
        }
    }
}