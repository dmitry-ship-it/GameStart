using Microsoft.AspNetCore.Identity;

namespace GameStart.IdentityService.Data.Models
{
    public class User : IdentityUser
    {
        public string ExternalProviderUserId { get; set; }
    }
}
