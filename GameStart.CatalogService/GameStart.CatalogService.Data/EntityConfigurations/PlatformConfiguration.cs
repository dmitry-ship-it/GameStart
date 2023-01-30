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

            builder.HasIndex(platfrom => platfrom.Name)
                .IsUnique(true);

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

            builder.HasMany(entity => entity.SystemRequirements)
                .WithMany(entity => entity.Platforms)
                .UsingEntity<Dictionary<string, object>>(
                    right => right.HasOne<SystemRequirements>()
                        .WithMany()
                        .HasForeignKey("SystemRequirementsId"),
                    left => left.HasOne<Platform>()
                        .WithMany()
                        .HasForeignKey("PlatformId"),
                    joining => joining.ToTable("VideoGamePlatformSystemRequirements")
                        .HasKey("SystemRequirementsId", "PlatformId")
                        .IsClustered(true));
        }
    }
}
