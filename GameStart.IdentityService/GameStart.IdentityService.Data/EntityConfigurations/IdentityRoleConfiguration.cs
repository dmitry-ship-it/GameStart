using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStart.IdentityService.Data.EntityConfigurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(new IdentityRole[]
            {
                new()
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
                new()
                {
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                }
            });
        }
    }
}
