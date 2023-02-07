using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.CatalogService.Data.EntityConfigurations
{
    public class LanguageAvailabilityConfiguration : IEntityTypeConfiguration<LanguageAvailability>
    {
        public void Configure(EntityTypeBuilder<LanguageAvailability> builder)
        {
            builder.HasKey(key => key.Id);

            builder.Property(entity => entity.AvailableForInterface)
                .HasDefaultValue(false);

            builder.Property(entity => entity.AvailableForAudio)
                .HasDefaultValue(false);

            builder.Property(entity => entity.AvailableForSubtitles)
                .HasDefaultValue(false);

            builder.HasMany(entity => entity.VideoGames)
                .WithMany(videoGame => videoGame.LanguageAvailabilities)
                .UsingEntity<Dictionary<string, object>>(
                    right => right.HasOne<VideoGame>()
                        .WithMany()
                        .HasForeignKey("VideoGameId"),
                    left => left.HasOne<LanguageAvailability>()
                        .WithMany()
                        .HasForeignKey("LanguageAvailabilityId"),
                    joining => joining.ToTable("VideoGameLanguage")
                        .HasKey("VideoGameId", "LanguageAvailabilityId"));

            builder.HasOne(entity => entity.Language)
                .WithMany(entity => entity.LanguageAvailabilities)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
