using GameStart.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.IdentityService.Data.EntityConfigurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(Enum.GetNames<Roles>().Select(role => new IdentityRole
            {
                Name = role,
                NormalizedName = role.ToUpper()
            }));
        }
    }
}
