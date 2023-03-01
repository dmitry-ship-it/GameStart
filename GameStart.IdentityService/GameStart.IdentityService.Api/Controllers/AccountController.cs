using GameStart.IdentityService.Common;
using GameStart.IdentityService.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GameStart.IdentityService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountManager accountManager;

        public AccountController(AccountManager accountManager)
        {
            this.accountManager = accountManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel loginViewModel,
            CancellationToken cancellationToken = default)
        {
            await accountManager.LoginAsync(loginViewModel, HttpContext, cancellationToken);

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel registerViewModel,
            CancellationToken cancellationToken = default)
        {
            await accountManager.RegisterAsync(registerViewModel, HttpContext, cancellationToken);

            return Ok();
        }

        [HttpGet("send-verification-email")]
        public async Task<IActionResult> SendVerificationEmailAsync(CancellationToken cancellationToken = default)
        {
            var confirmationActionUri = Url.Action("verifyEmail");
            await accountManager.SendEmailVerificationRequestAsync(
                HttpContext.User, confirmationActionUri, cancellationToken);

            return Accepted();
        }

        [HttpGet("verifyEmail")]
        public async Task<IActionResult> VerifyEmailAsync([FromQuery] string token, CancellationToken cancellationToken = default)
        {
            var isSuccess = await accountManager.VerifyEmailAsync(token, HttpContext, cancellationToken);

            return isSuccess ? Ok() : BadRequest();
        }

        [HttpGet("challenge")]
        public IActionResult Challenge([FromQuery] string scheme, [FromQuery] string returnUrl)
        {
            var authenticationProperties = accountManager.CreateAuthenticationProperties(
                scheme, returnUrl, Url.Action("callback"), HttpContext);

            return Challenge(authenticationProperties, scheme);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> CallbackAsync(CancellationToken cancellationToken = default)
        {
            var returnUrl = await accountManager.ExternalAuthenticateAsync(
                HttpContext, cancellationToken);

            return Redirect(returnUrl.ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("logout")]
        public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken = default)
        {
            await accountManager.ClearCookiesAsync(HttpContext, cancellationToken);

            return Ok();
        }
    }
}
