using GameStart.IdentityService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.IdentityService.Data.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(user => user.ExternalProviderUserId);

            builder.Property(user => user.ExternalProviderUserId)
                .HasMaxLength(255)
                .IsRequired(false);
        }
    }
}
