using GameStart.OrderingService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.OrderingService.Data.EntityConfigurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(key => key.Id);

            builder.Property(item => item.GameId)
                .IsRequired(true);

            builder.Property(item => item.IsPhysicalCopy)
                .HasDefaultValue(false);
        }
    }
}
