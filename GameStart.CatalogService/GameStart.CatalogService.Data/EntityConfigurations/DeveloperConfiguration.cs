using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.CatalogService.Data.EntityConfigurations
{
    public class DeveloperConfiguration : IEntityTypeConfiguration<Developer>
    {
        public void Configure(EntityTypeBuilder<Developer> builder)
        {
            builder.HasKey(developer => developer.Id);

            builder.Property(developer => developer.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(developer => developer.Name)
                .IsUnique(true);

            builder.Property(developer => developer.Name)
                .HasMaxLength(256)
                .IsUnicode(true)
                .IsRequired(true);

            builder.HasMany(entity => entity.VideoGames)
                .WithMany(entity => entity.Developers)
                .UsingEntity<Dictionary<string, object>>(
                    right => right.HasOne<VideoGame>()
                        .WithMany()
                        .HasForeignKey("VideoGameId"),
                    left => left.HasOne<Developer>()
                        .WithMany()
                        .HasForeignKey("DeveloperId"),
                    joining => joining.ToTable("VideoGameDeveloper")
                        .HasKey("VideoGameId", "DeveloperId"));
        }
    }
}
