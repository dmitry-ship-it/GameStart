using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.CatalogService.Data.EntityConfigurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(genre => genre.Id);

            builder.Property(genre => genre.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(genre => genre.Name)
                .IsUnique(true);

            builder.Property(genre => genre.Name)
                .HasMaxLength(256)
                .IsUnicode(true)
                .IsRequired(true);

            builder.HasMany(entity => entity.VideoGames)
                .WithMany(entity => entity.Genres)
                .UsingEntity<Dictionary<string, object>>(
                    right => right.HasOne<VideoGame>()
                        .WithMany()
                        .HasForeignKey("VideoGameId"),
                    left => left.HasOne<Genre>()
                        .WithMany()
                        .HasForeignKey("GenreId"),
                    joining => joining.ToTable("VideoGameGenre")
                        .HasKey("VideoGameId", "GenreId"));
        }
    }
}
