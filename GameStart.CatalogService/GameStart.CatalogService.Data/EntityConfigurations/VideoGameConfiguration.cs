using GameStart.CatalogService.Data.EntityConfigurations.ValueConverters;
using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.CatalogService.Data.EntityConfigurations
{
    public class VideoGameConfiguration : IEntityTypeConfiguration<VideoGame>
    {
        public void Configure(EntityTypeBuilder<VideoGame> builder)
        {
            builder.HasKey(key => key.Id)
                .IsClustered(true);

            builder.Property(videoGame => videoGame.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(videoGame => videoGame.Title)
                .IsUnique(false);

            builder.Property(videoGame => videoGame.Title)
                .HasMaxLength(256)
                .IsUnicode(true)
                .IsRequired(true);

            builder.Property(videoGame => videoGame.Description)
                .IsUnicode(true)
                .IsRequired(false);

            builder.Property(videoGame => videoGame.Copyright)
                .IsUnicode(true)
                .IsRequired(false);

            builder.Property(videoGame => videoGame.ReleaseDate)
                .HasConversion<DateOnlyConverter>()
                .HasColumnType("date")
                .IsRequired(true);

            builder.Property(videoGame => videoGame.Price)
                .HasColumnType("money")
                .IsRequired(true);

            builder.HasMany(videoGame => videoGame.InterfaceLanguages)
                .WithMany(language => language.VideoGames)
                .UsingEntity<Dictionary<string, object>>("VideoGameInterfaceLanguage",
                    right => right.HasOne<Language>()
                        .WithMany()
                        .HasForeignKey("LanguageId"),
                    left => left.HasOne<VideoGame>()
                        .WithMany()
                        .HasForeignKey("VideoGameId"));

            builder.HasMany(videoGame => videoGame.AudioLanguages)
                .WithMany(language => language.VideoGames)
                .UsingEntity<Dictionary<string, object>>("VideoGameAudioLanguage",
                    right => right.HasOne<Language>()
                        .WithMany()
                        .HasForeignKey("LanguageId"),
                    left => left.HasOne<VideoGame>()
                        .WithMany()
                        .HasForeignKey("VideoGameId"));

            builder.HasMany(videoGame => videoGame.SubtitlesLanguages)
                .WithMany(language => language.VideoGames)
                .UsingEntity<Dictionary<string, object>>("VideoGameSubtitlesLanguage",
                    right => right.HasOne<Language>()
                        .WithMany()
                        .HasForeignKey("LanguageId"),
                    left => left.HasOne<VideoGame>()
                        .WithMany()
                        .HasForeignKey("VideoGameId"));
        }
    }
}
