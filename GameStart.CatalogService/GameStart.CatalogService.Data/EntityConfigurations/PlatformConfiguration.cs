using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.CatalogService.Data.EntityConfigurations
{
    public class PlatformConfiguration : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> builder)
        {
            builder.HasKey(platfrom => platfrom.Id);

            builder.Property(platfrom => platfrom.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(platfrom => platfrom.Name)
                .IsUnique(true);

            builder.Property(platfrom => platfrom.Name)
                .HasMaxLength(256)
                .IsUnicode(true)
                .IsRequired(true);
        }
    }
}
