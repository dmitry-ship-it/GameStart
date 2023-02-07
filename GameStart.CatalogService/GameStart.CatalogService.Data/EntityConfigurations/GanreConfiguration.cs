using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.CatalogService.Data.EntityConfigurations
{
    public class GanreConfiguration : IEntityTypeConfiguration<Ganre>
    {
        public void Configure(EntityTypeBuilder<Ganre> builder)
        {
            builder.HasKey(ganre => ganre.Id);

            builder.Property(ganre => ganre.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(ganre => ganre.Name)
                .IsUnique(true);

            builder.Property(ganre => ganre.Name)
                .HasMaxLength(256)
                .IsUnicode(true)
                .IsRequired(true);

            builder.HasMany(entity => entity.VideoGames)
                .WithMany(entity => entity.Ganres)
                .UsingEntity<Dictionary<string, object>>(
                    right => right.HasOne<VideoGame>()
                        .WithMany()
                        .HasForeignKey("VideoGameId"),
                    left => left.HasOne<Ganre>()
                        .WithMany()
                        .HasForeignKey("GanreId"),
                    joining => joining.ToTable("VideoGameGanre")
                        .HasKey("VideoGameId", "GanreId"));
        }
    }
}
