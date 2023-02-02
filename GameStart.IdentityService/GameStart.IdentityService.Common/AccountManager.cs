using AutoMapper;
using GameStart.IdentityService.Data.Models;
using GameStart.Shared;
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

        public virtual async Task LoginAsync(string email, string password, HttpContext httpContext, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                throw new ArgumentException(Constants.IdentityService.ExceptionMessages.UserNotFound);
            }

            var checkResult = await signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!checkResult.Succeeded)
            {
                throw new ArgumentException(Constants.IdentityService.ExceptionMessages.InvalidCredentials);
            }

            cancellationToken.ThrowIfCancellationRequested();

            await signInManager.SignInAsync(user, true);
        }

        public virtual async Task RegisterAsync(string username, string email, string password, CancellationToken cancellationToken = default)
        {
            var user = new User
            {
                UserName = username,
                Email = email,
            };

            cancellationToken.ThrowIfCancellationRequested();

            await userManager.CreateAsync(user, password);
        }

        public virtual AuthenticationProperties CreateAuthenticationProperties(string scheme, string returnUrl, string callbackUrl)
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

        public virtual async Task<Uri> AuthenticateAndCreateUserIfNotExistsAsync(HttpContext httpContext, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await httpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new ArgumentException(Constants.IdentityService.ExceptionMessages.ExternalAuthenticationError);
            }

            await CreateUserAndCookieAsync(result.Principal, httpContext, cancellationToken);

            return new Uri(result.Properties?.Items["returnUrl"]);
        }

        public virtual async Task LogoutAsync(HttpContext httpContext, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await httpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await httpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);
            httpContext.Session.Clear();
        }

        private virtual async Task CreateUserAndCookieAsync(ClaimsPrincipal principal, HttpContext httpContext, CancellationToken cancellationToken = default)
        {
            // lookup our user and external provider info
            var claims = principal.Claims.ToList();

            var email = claims.Find(claim => claim.Type == ClaimTypes.Email).Value;

            cancellationToken.ThrowIfCancellationRequested();

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
