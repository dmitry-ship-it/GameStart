using GameStart.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.IdentityService.Data.EntityConfigurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            builder.HasData(Enum.GetNames<Roles>().Select(role => new IdentityRole<Guid>
            {
                Name = role,
                NormalizedName = role.ToUpper()
            }));
        }
    }
}
