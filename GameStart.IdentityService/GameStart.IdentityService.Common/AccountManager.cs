using AutoMapper;
using GameStart.IdentityService.Common.ViewModels;
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

        public virtual async Task LoginAsync(
            LoginViewModel model,
            HttpContext httpContext,
            CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByNameAsync(model.Username);

            if (user is null)
            {
                throw new ArgumentException(Constants.IdentityService.ExceptionMessages.UserNotFound);
            }

            var checkResult = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!checkResult.Succeeded)
            {
                throw new ArgumentException(Constants.IdentityService.ExceptionMessages.InvalidCredentials);
            }

            await signInManager.SignInAsync(user, true);

            httpContext.User = await signInManager.ClaimsFactory.CreateAsync(user);
            await GenerateJwtAsync(httpContext, cancellationToken);
        }

        public virtual async Task RegisterAsync(
            RegisterViewModel model,
            CancellationToken cancellationToken = default)
        {
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email
            };

            cancellationToken.ThrowIfCancellationRequested();

            await userManager.CreateAsync(user, model.Password);
            await userManager.AddToRoleAsync(user, nameof(Roles.User));
        }

        public virtual AuthenticationProperties CreateAuthenticationProperties(
            string scheme, string returnUrl, string callbackUri, HttpContext httpContext)
        {
            // override docker's service name which used as hostname
            httpContext.Request.Host = new HostString(Environment.GetEnvironmentVariable("OUTSIDE_HOST"));

            return new()
            {
                RedirectUri = callbackUri,
                Items =
                {
                    [nameof(returnUrl)] = returnUrl,
                    [nameof(scheme)] = scheme
                }
            };
        }

        public virtual async Task<Uri> ExternalAuthenticateAsync(
            HttpContext httpContext, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await httpContext.AuthenticateAsync(
                IdentityServerConstants.ExternalCookieAuthenticationScheme);

            if (result?.Succeeded != true)
            {
                throw new ArgumentException(Constants.IdentityService.ExceptionMessages.ExternalAuthenticationError);
            }

            await CreateUserAsync(result.Principal, httpContext, cancellationToken);

            return new Uri(result.Properties?.Items["returnUrl"]);
        }

        public virtual async Task ClearCookiesAsync(HttpContext httpContext,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await httpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await httpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);

            httpContext.Response.Cookies.Delete(Constants.AuthCookieName);
        }

        /// <summary>
        ///     This method is used after external authentication (Google).
        ///     All required claims are mapping into <see cref="User"/> object.
        ///     If user with given email does not exist then will be created new user
        ///     without password, and with email as username.
        /// </summary>
        private async Task CreateUserAsync(
            ClaimsPrincipal principal,
            HttpContext httpContext,
            CancellationToken cancellationToken = default)
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
                await userManager.AddToRoleAsync(mapped, nameof(Roles.User));

                user = await userManager.FindByEmailAsync(email);
            }

            // delete temporary cookie used during external authentication
            await httpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            await signInManager.SignInAsync(user, true);

            httpContext.User = await signInManager.ClaimsFactory.CreateAsync(user);
            await GenerateJwtAsync(httpContext, cancellationToken);
        }

        /// <summary>
        ///     Generate JSON Web Token from identity which stored inside cookies,
        ///     and write it to Authorization header of the response.
        /// </summary>
        public async Task GenerateJwtAsync(HttpContext httpContext,
            CancellationToken cancellationToken = default)
        {
            var token = await tools.IssueJwtAsync(Constants.IdentityService.TokenLifetime, httpContext.User.Claims);

            cancellationToken.ThrowIfCancellationRequested();

            var cookieOptions = new CookieOptions()
            {
                IsEssential = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
                MaxAge = TimeSpan.FromSeconds(Constants.IdentityService.TokenLifetime)
            };

            httpContext.Response.Cookies.Append(HeaderNames.Authorization, token, cookieOptions);
        }
    }
}
