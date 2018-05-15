using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ISPCore.Engine.Middleware
{
    public class CoreMiddleware
    {
        private readonly RequestDelegate _next;

        public CoreMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            // Доступ запрещен с порта панели 8793
            if (httpContext.Request.Host.Port == 8793 && httpContext.Request.Path.Value.StartsWith("/core/"))
            {
                httpContext.Response.ContentType = "text/plain; charset=utf-8";
                return httpContext.Response.WriteAsync("Локальный доступ 127.0.0.1:4538");
            }

            return _next(httpContext);
        }
    }
    

    public static class CoreMiddlewareExtensions
    {
        public static IApplicationBuilder UseCoreMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CoreMiddleware>();
        }
    }
}
