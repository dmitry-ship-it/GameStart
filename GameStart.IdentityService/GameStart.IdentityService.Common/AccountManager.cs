using AutoMapper;
using GameStart.IdentityService.Data.Models;
using GameStart.Shared;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;

namespace GameStart.IdentityService.Common
{
    public class AccountManager
    {
        private const int TokenLifetime = 3600;

        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IdentityServerTools tools;

        public AccountManager(IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IdentityServerTools tools)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tools = tools;
        }

        public virtual async Task LoginAsync(string username, string password, HttpContext httpContext, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user is null)
            {
                throw new ArgumentException(Constants.IdentityService.ExceptionMessages.UserNotFound);
            }

            var checkResult = await signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!checkResult.Succeeded)
            {
                throw new ArgumentException(Constants.IdentityService.ExceptionMessages.InvalidCredentials);
            }

            await signInManager.SignInAsync(user, true);
            await GenerateJwtAsync(httpContext, cancellationToken);
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

        public virtual async Task<Uri> ExternalAuthenticateAsync(HttpContext httpContext, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await httpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new ArgumentException(Constants.IdentityService.ExceptionMessages.ExternalAuthenticationError);
            }

            await CreateUserAsync(result.Principal, httpContext, cancellationToken);

            return new Uri(result.Properties?.Items["returnUrl"]);
        }

        public virtual async Task ClearCookiesAsync(HttpContext httpContext, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await httpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await httpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);
        }

        /// <summary>
        ///     This method is used after external authentication (Google).
        ///     All required claims are mapping into <c>User</c> object.
        ///     If user with given email does not exist then will be created new user
        ///     without password, and with email as username.
        /// </summary>
        private async Task CreateUserAsync(ClaimsPrincipal principal, HttpContext httpContext, CancellationToken cancellationToken = default)
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

            // delete temporary cookie used during external authentication
            await httpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            await signInManager.SignInAsync(user, true);
            await GenerateJwtAsync(httpContext, cancellationToken);
        }

        /// <summary>
        ///     Generate JSON Web Token from identity which stored inside cookies, and write it to Authorization header of the response.
        /// </summary>
        public async Task GenerateJwtAsync(HttpContext httpContext, CancellationToken cancellationToken = default)
        {
            var principal = new ClaimsPrincipal(httpContext.User.Identity);
            var token = await tools.IssueJwtAsync(TokenLifetime, principal.Claims);

            cancellationToken.ThrowIfCancellationRequested();

            httpContext.Response.Headers.Add(HeaderNames.Authorization, $"Bearer {token}");
        }
    }
}
