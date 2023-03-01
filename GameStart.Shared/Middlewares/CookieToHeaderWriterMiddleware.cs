using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace GameStart.Shared.Middlewares
{
    public class CookieToHeaderWriterMiddleware
    {
        private readonly RequestDelegate next;

        public CookieToHeaderWriterMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var cookie = Constants.AuthCookieName;

            if (context.Request.Cookies.ContainsKey(cookie))
            {
                context.Request.Headers[HeaderNames.Authorization] = $"Bearer {context.Request.Cookies[cookie]}";
            }

            await next(context);
        }
    }
}
