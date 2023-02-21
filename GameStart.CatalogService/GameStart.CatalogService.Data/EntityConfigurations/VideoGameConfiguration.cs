using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.CatalogService.Data.EntityConfigurations
{
    public class VideoGameConfiguration : IEntityTypeConfiguration<VideoGame>
    {
        public void Configure(EntityTypeBuilder<VideoGame> builder)
        {
            builder.HasKey(key => key.Id);

            builder.Property(videoGame => videoGame.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(videoGame => videoGame.Title)
                .IsUnique(true);

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
                .HasColumnType("timestamp with time zone")
                .IsRequired(true);

            builder.Property(videoGame => videoGame.Price)
                .HasColumnType("money")
                .IsRequired(true);
        }
    }
}
