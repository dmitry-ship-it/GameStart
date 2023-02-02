using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GameStart.Shared.Middlewares
{
    public class ExceptionLoggerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionLoggerMiddleware> logger;
        private readonly bool isProduction;

        public ExceptionLoggerMiddleware(
            RequestDelegate next,
            ILogger<ExceptionLoggerMiddleware> logger,
            IWebHostEnvironment environment)
        {
            this.next = next;
            this.logger = logger;
            isProduction = environment.IsProduction();
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
            if (!isProduction)
            {
                logger.LogWarning("{Exception}: {Message}, Source: {Source}, StackTrace: {StackTrace}, InnerException: {InnerException}, InnerExceptionMessage {InnerExceptionMessage}",
                ex.GetType().Name, ex.Message, ex.Source, ex.StackTrace, ex.InnerException?.GetType().Name, ex.InnerException?.Message);
            }
            else
            {
                logger.LogWarning("{Exception}: {Message}, Source: {Source}",
                    ex.GetType().Name, ex.Message, ex.Source);
            }
        }
    }
}
