using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.CatalogService.Data.EntityConfigurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.HasKey(language => language.Id)
                .IsClustered(true);

            builder.Property(language => language.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(language => language.Name);

            builder.Property(language => language.Name)
                .HasMaxLength(256)
                .IsUnicode(true)
                .IsRequired(true);
        }
    }
}
