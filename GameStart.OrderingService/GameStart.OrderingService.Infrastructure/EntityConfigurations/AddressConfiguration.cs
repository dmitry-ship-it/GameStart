using GameStart.OrderingService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.OrderingService.Infrastructure.EntityConfigurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(key => key.Id);

            builder.Property(address => address.Сountry)
                .HasMaxLength(60)
                .IsUnicode(true)
                .IsRequired(true);

            builder.Property(address => address.State)
                .HasMaxLength(60)
                .IsUnicode(true)
                .IsRequired(false);

            builder.Property(address => address.City)
                .HasMaxLength(100)
                .IsUnicode(true)
                .IsRequired(true);

            builder.Property(address => address.Street)
                .HasMaxLength(60)
                .IsUnicode(true)
                .IsRequired(true);

            builder.Property(address => address.House)
                .HasMaxLength(10)
                .IsUnicode(true)
                .IsRequired(true);

            builder.Property(address => address.Flat)
                .HasMaxLength(10)
                .IsUnicode(true)
                .IsRequired(false);

            builder.Property(address => address.PostCode)
                .HasMaxLength(12)
                .IsUnicode(true)
                .IsRequired(true);
        }
    }
}
