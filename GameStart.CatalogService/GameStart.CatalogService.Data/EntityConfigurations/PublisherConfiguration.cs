using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.CatalogService.Data.EntityConfigurations
{
    public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.HasKey(publisher => publisher.Id)
                .IsClustered(true);

            builder.Property(publisher => publisher.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(publisher => publisher.Name)
                .IsUnique(true);

            builder.Property(publisher => publisher.Name)
                .HasMaxLength(256)
                .IsUnicode(true)
                .IsRequired(true);

            builder.HasMany(publisher => publisher.VideoGames)
                .WithOne(videoGame => videoGame.Publisher);
        }
    }
}
