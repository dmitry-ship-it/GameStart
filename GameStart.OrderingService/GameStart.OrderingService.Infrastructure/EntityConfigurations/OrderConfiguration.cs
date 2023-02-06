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

            builder.Property(order => order.DateTime)
                .IsRequired(true);

            builder.HasOne(order => order.Address)
                .WithOne(address => address.Order)
                .IsRequired(false);

            builder.HasMany(order => order.Items)
                .WithOne(item => item.Order)
                .IsRequired(true);
        }
    }
}
