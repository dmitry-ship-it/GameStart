using IdentityModel;
using System.Security.Claims;

namespace GameStart.Shared.Extensions
{
    public static class EnumerableExtension
    {
        public static Guid GetUserId(this IEnumerable<Claim> claims)
        {
            var found = claims.First(claim =>
                claim.Type == ClaimTypes.NameIdentifier || claim.Type == JwtClaimTypes.Subject);

            return Guid.Parse(found.Value);
        }
    }
}
