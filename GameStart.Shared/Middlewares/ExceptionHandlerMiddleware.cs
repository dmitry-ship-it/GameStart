using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GameStart.Shared.Middlewares
{
    public class ExceptionLoggerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionLoggerMiddleware> logger;

        public ExceptionLoggerMiddleware(
            RequestDelegate next,
            ILogger<ExceptionLoggerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                LogException(ex);
            }
        }

        private void LogException(Exception ex)
        {
            logger.LogWarning("{Exception}: {Message}, Source: {Source}",
                ex.GetType().Name, ex.Message, ex.Source);
        }
    }
}
