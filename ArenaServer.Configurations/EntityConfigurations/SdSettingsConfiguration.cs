using ArenaServer.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArenaServer.Data.Configurations.EntityConfigurations
{
    public class SdSettingsConfiguration : IEntityTypeConfiguration<SdSettings>
    {
        public void Configure(EntityTypeBuilder<SdSettings> builder)
        {
            builder.ToTable("SdSettings").HasKey(s => s.SdSettingsId);
        }
    }
}