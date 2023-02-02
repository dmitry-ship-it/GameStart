using GameStart.IdentityService.Common;
using GameStart.IdentityService.Common.ViewModels;
using GameStart.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GameStart.IdentityService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountManager accountManager;

        public AccountController(AccountManager accountManager)
        {
            this.accountManager = accountManager;
        }

        [HttpPost(Constants.IdentityService.Endpoints.LoginEndpointName)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel loginViewModel, CancellationToken cancellationToken = default)
        {
            await accountManager.LoginAsync(loginViewModel.Username,
                loginViewModel.Password, HttpContext, cancellationToken);

            return Ok();
        }

        [HttpPost(Constants.IdentityService.Endpoints.RegisterEndpointName)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel registerViewModel, CancellationToken cancellationToken = default)
        {
            await accountManager.RegisterAsync(registerViewModel.Username,
                registerViewModel.Email, registerViewModel.Password, cancellationToken);

            return Ok();
        }

        [HttpGet(Constants.IdentityService.Endpoints.ChallengeEndpointName)]
        public IActionResult Challenge([FromQuery] string scheme, [FromQuery] string returnUrl)
        {
            var authenticationProperties = accountManager.CreateAuthenticationProperties(
                scheme, returnUrl, Url.Action(Constants.IdentityService.Endpoints.CallbackEndpointName));

            return Challenge(authenticationProperties, scheme);
        }

        [HttpGet(Constants.IdentityService.Endpoints.CallbackEndpointName)]
        public async Task<IActionResult> CallbackAsync(CancellationToken cancellationToken = default)
        {
            var returnUrl = await accountManager.ExternalAuthenticateAsync(
                HttpContext, cancellationToken);

            return Redirect(returnUrl.ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet(Constants.IdentityService.Endpoints.LogoutEndpointName)]
        public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken = default)
        {
            await accountManager.ClearCookiesAsync(HttpContext, cancellationToken);

            return Ok();
        }
    }
}
