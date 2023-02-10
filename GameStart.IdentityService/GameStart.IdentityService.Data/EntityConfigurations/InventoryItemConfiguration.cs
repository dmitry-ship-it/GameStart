using GameStart.IdentityService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.IdentityService.Data.EntityConfigurations
{
    public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
    {
        public void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            builder.HasKey(key => key.Id)
                .IsClustered(true);

            builder.HasIndex(entity => entity.GameId)
                .IsUnique(true);

            builder.Property(entity => entity.GameId)
                .IsRequired(true);

            builder.Property(entity => entity.GameKey)
                .HasMaxLength(17)
                .IsFixedLength(true)
                .IsRequired(true);

            builder.Property(entity => entity.PurchaseDateTime)
                .IsRequired(true);

            builder.HasOne(entity => entity.User)
                .WithMany(user => user.Inventory)
                .IsRequired(true);
        }
    }
}
