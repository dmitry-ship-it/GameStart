using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.CatalogService.Data.EntityConfigurations
{
    public class SystemRequirementsConfiguration : IEntityTypeConfiguration<SystemRequirements>
    {
        public void Configure(EntityTypeBuilder<SystemRequirements> builder)
        {
            builder.HasKey(key => key.Id);

            builder.Property(requirements => requirements.Id)
                .ValueGeneratedOnAdd();

            builder.Property(requirements => requirements.OS)
                .IsUnicode(true)
                .IsRequired(false);

            builder.Property(requirements => requirements.Processor)
                .IsUnicode(true)
                .IsRequired(false);

            builder.Property(requirements => requirements.Memory)
                .IsUnicode(true)
                .IsRequired(false);

            builder.Property(requirements => requirements.Graphics)
                .IsUnicode(true)
                .IsRequired(false);

            builder.Property(requirements => requirements.Network)
                .IsUnicode(true)
                .IsRequired(false);

            builder.Property(requirements => requirements.Storage)
                .IsUnicode(true)
                .IsRequired(false);

            builder.HasOne(requirements => requirements.VideoGame)
                .WithMany(videoGame => videoGame.SystemRequirements)
                .HasForeignKey("VideoGameId");

            builder.HasOne(requirements => requirements.Platform)
                .WithMany(platform => platform.SystemRequirements)
                .HasForeignKey("PlatformId");

            builder.HasIndex("VideoGameId", "PlatformId")
                .IsUnique(true);
        }
    }
}
