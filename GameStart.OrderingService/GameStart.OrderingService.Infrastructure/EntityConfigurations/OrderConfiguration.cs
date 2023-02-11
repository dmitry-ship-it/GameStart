using GameStart.OrderingService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.OrderingService.Infrastructure.EntityConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(key => key.Id);

            builder.Property(order => order.UserId)
                .IsRequired(true);

            builder.Property(order => order.TotalPrice)
                .HasColumnType("DECIMAL(13, 4)")
                .HasDefaultValue(0)
                .IsRequired(true);

            builder.Property(order => order.State)
                .HasMaxLength(32)
                .IsUnicode(true)
                .IsRequired(true);

            builder.Property(order => order.DateTime)
                .IsRequired(true);

            builder.HasOne(order => order.Address)
                .WithMany(address => address.Orders);

            builder.HasMany(order => order.Items)
                .WithOne(item => item.Order)
                .IsRequired(true);
        }
    }
}
