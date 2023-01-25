using IdentityModel;
using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameStart.IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityServerInteractionService interaction;
        private readonly TestUserStore users;

        public AccountController(IIdentityServerInteractionService interaction, TestUserStore users)
        {
            this.interaction = interaction;
            this.users = users;
        }

        [HttpGet(nameof(Challenge))]
        public IActionResult Challenge(string scheme, string returnUrl)
        {
            // validate returnUrl - either it is a valid OIDC URL or back to a local page
            //if (!Url.IsLocalUrl(returnUrl) && !interaction.IsValidReturnUrl(returnUrl))
            //{
            //    // user might have clicked on a malicious link
            //    throw new ArgumentException("Invalid return URL", nameof(returnUrl));
            //}

            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Url.Action("callback"),
                Items =
                {
                    [nameof(returnUrl)] = returnUrl,
                    [nameof(scheme)] = scheme,
                }
            }, scheme);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> CallbackAsync()
        {
            // https://localhost:7153/api/account/challenge?scheme=Google&returnUrl=https://google.com
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new ArgumentException("External authentication error");
            }

            // lookup our user and external provider info
            var (provider, providerUserId, claims) = GetUserClaims(result);
            var user = users.AutoProvisionUser(provider, providerUserId, claims);

            // collect additional claims and sign in props
            var (additionalLocalClaims, localSignInProps) = ProcessLoginCallback(result);

            // issue authentication cookie for user
            var issuer = new IdentityServerUser(user.SubjectId)
            {
                DisplayName = user.Username,
                IdentityProvider = provider,
                AdditionalClaims = additionalLocalClaims
            };

            await HttpContext.SignInAsync(issuer, localSignInProps);

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            // retrieve return URL
            var returnUrl = result.Properties?.Items["returnUrl"];

            return Redirect(returnUrl!);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        [HttpGet("ok")]
        public IActionResult OkR()
        {
            return Ok("ok");
        }

        private static (List<Claim>, AuthenticationProperties) ProcessLoginCallback(AuthenticateResult externalResult)
        {
            var additionalLocalClaims = new List<Claim>();
            var localSignInProps = new AuthenticationProperties();

            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            var sid = externalResult.Principal?.Claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.SessionId);
            if (sid is not null)
            {
                additionalLocalClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            var idToken = externalResult.Properties?.GetTokenValue("id_token");
            if (idToken is not null)
            {
                localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
            }

            return (additionalLocalClaims, localSignInProps);
        }

        private (string providerUserId, string provider, List<Claim> claims) GetUserClaims(AuthenticateResult result)
        {
            var externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var userIdClaim = externalUser?.FindFirst(JwtClaimTypes.Subject)
                ?? externalUser?.FindFirst(ClaimTypes.NameIdentifier)
                ?? throw new ArgumentException("Unknown user_id", nameof(result));

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            var claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);

            var provider = result.Properties?.Items["scheme"];
            var providerUserId = userIdClaim.Value;

            return (provider!, providerUserId, claims);
        }
    }
}
