using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.CatalogService.Data.EntityConfigurations
{
    public class PlatformConfiguration : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> builder)
        {
            builder.HasKey(platfrom => platfrom.Id)
                .IsClustered(true);

            builder.Property(platfrom => platfrom.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(platfrom => platfrom.Name);

            builder.Property(platfrom => platfrom.Name)
                .HasMaxLength(256)
                .IsUnicode(true)
                .IsRequired(true);

            builder.HasMany(entity => entity.VideoGames)
                .WithMany(entity => entity.Platforms)
                .UsingEntity<Dictionary<string, object>>(
                    right => right.HasOne<VideoGame>()
                        .WithMany()
                        .HasForeignKey("VideoGameId"),
                    left => left.HasOne<Platform>()
                        .WithMany()
                        .HasForeignKey("PlatformId"),
                    joining => joining.ToTable("VideoGamePlatform")
                        .HasKey("VideoGameId", "PlatformId")
                        .IsClustered(true));

            builder.HasMany(platform => platform.MinimalSystemRequirements)
                .WithMany(requirements => requirements.Platforms)
                .UsingEntity<Dictionary<string, object>>("PlatformMinimalSystemRequirements",
                    right => right.HasOne<SystemRequirements>()
                        .WithMany()
                        .HasForeignKey("MinimalSystemRequirementsId"),
                    left => left.HasOne<Platform>()
                        .WithMany()
                        .HasForeignKey("PlatformId"));

            builder.HasMany(platform => platform.RecommendedSystemRequirements)
                .WithMany(requirements => requirements.Platforms)
                .UsingEntity<Dictionary<string, object>>("PlatformRecommendedSystemRequirements",
                    right => right.HasOne<SystemRequirements>()
                        .WithMany()
                        .HasForeignKey("RecommendedSystemRequirementsId"),
                    left => left.HasOne<Platform>()
                        .WithMany()
                        .HasForeignKey("PlatformId"));
        }
    }
}
