using GameStart.IdentityService.Common;
using GameStart.IdentityService.Common.ViewModels;
using GameStart.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GameStart.IdentityService.Controllers
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

        [HttpPost(Constants.IdentityServiceEndpoints.LoginEndpointName)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel loginViewModel)
        {
            await accountManager.LoginAsync(loginViewModel.Email, loginViewModel.Password);
            return Ok();
        }

        [HttpPost(Constants.IdentityServiceEndpoints.RegisterEndpointName)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel registerViewModel)
        {
            await accountManager.RegisterAsync(registerViewModel.Username, registerViewModel.Email, registerViewModel.Password);
            return Ok();
        }

        [HttpGet(Constants.IdentityServiceEndpoints.ChallengeEndpointName)]
        public IActionResult Challenge([FromQuery] string scheme, [FromQuery] string returnUrl)
        {
            // example: https://localhost:7153/api/account/challenge?scheme=Google&returnUrl=https://google.com
            var authenticationProperties = accountManager.CreateAuthenticationProperties(scheme, returnUrl, Url.Action("callback"));
            return Challenge(authenticationProperties, scheme);
        }

        [HttpGet(Constants.IdentityServiceEndpoints.CallbackEndpointName)]
        public async Task<IActionResult> CallbackAsync()
        {
            var returnUrl = await accountManager.AuthenticateAndCreateUserIfNotExistsAsync(HttpContext);
            return Redirect(returnUrl.ToString());
        }

        [HttpGet(Constants.IdentityServiceEndpoints.LogoutEndpointName)]
        public async Task<IActionResult> LogoutAsync()
        {
            await accountManager.LogoutAsync(HttpContext);
            return Ok();
        }
    }
}
