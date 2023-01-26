using AutoMapper;
using GameStart.IdentityService.Data.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GameStart.IdentityService.Common
{
    public class AccountManager
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountManager(IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task LoginAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                throw new ArgumentException("User was not found");
            }

            var checkResult = await signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!checkResult.Succeeded)
            {
                throw new ArgumentException("User was not found or password is incorrect", nameof(password));
            }

            await signInManager.SignInAsync(user, true);
        }

        public async Task RegisterAsync(string username, string email, string password)
        {
            var user = new User
            {
                UserName = username,
                Email = email,
            };

            await userManager.CreateAsync(user, password);
        }

        public AuthenticationProperties CreateAuthenticationProperties(string scheme, string returnUrl, string callbackUrl)
        {
            return new AuthenticationProperties
            {
                RedirectUri = callbackUrl,
                Items =
                {
                    [nameof(returnUrl)] = returnUrl,
                    [nameof(scheme)] = scheme,
                }
            };
        }

        public async Task<Uri> AuthenticateAndCreateUserIfNotExistsAsync(HttpContext httpContext)
        {
            var result = await httpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new ArgumentException("External authentication error");
            }

            await CreateUserAndCookieAsync(result.Principal, httpContext);

            return new Uri(result.Properties?.Items["returnUrl"]);
        }

        public async Task LogoutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await httpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);
        }

        private async Task CreateUserAndCookieAsync(ClaimsPrincipal principal, HttpContext httpContext)
        {
            // lookup our user and external provider info
            var claims = principal.Claims.ToList();

            var email = claims.Find(claim => claim.Type == ClaimTypes.Email).Value;
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                var mapped = mapper.Map<List<Claim>, User>(claims);
                await userManager.CreateAsync(mapped);
                user = await userManager.FindByEmailAsync(email);
            }

            await signInManager.SignInAsync(user, true);
            await httpContext.SignInAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme, principal);

            // delete temporary cookie used during external authentication
            await httpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
        }
    }
}
