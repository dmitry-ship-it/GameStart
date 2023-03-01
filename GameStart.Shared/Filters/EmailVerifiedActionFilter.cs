using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameStart.Shared.Filters
{
    public class EmailVerifiedActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User?.Identity?.IsAuthenticated == true)
            {
                var emailVerifiedClaim = context.HttpContext.User.Claims.FirstOrDefault(
                    claim => claim.Type == JwtClaimTypes.EmailVerified);

                var isVerified = bool.Parse(emailVerifiedClaim?.Value ?? bool.FalseString);

                var containsAuthorizeAttribute = context.ActionDescriptor.EndpointMetadata.Any(
                    metadata => metadata is AuthorizeAttribute);

                if (!isVerified && containsAuthorizeAttribute)
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Result = new JsonResult(new { Message = Constants.Messages.VerifiedEmailIsRequired });
                    return;
                }
            }

            await next();
        }
    }
}
