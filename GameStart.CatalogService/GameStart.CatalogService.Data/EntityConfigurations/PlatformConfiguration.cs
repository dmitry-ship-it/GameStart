using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.CatalogService.Data.EntityConfigurations
{
    public class PlatformConfiguration : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> builder)
        {
            builder.HasKey(platform => platform.Id);

            builder.Property(platform => platform.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(platform => platform.Name)
                .IsUnique(true);

            builder.Property(platform => platform.Name)
                .HasMaxLength(256)
                .IsUnicode(true)
                .IsRequired(true);
        }
    }
}
