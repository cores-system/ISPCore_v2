using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ISPCore.Models.Databases.json;
using System.Text;

namespace ISPCore.Engine.Middleware
{
    public class AuthApiMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthApiMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Value != "/api/faq" && httpContext.Request.Path.Value.StartsWith("/api"))
            {
                var jsonDB = Service.Get<JsonDB>();

                // Если API выключен
                if (!jsonDB.API.Enabled)
                    return httpContext.Response.WriteAsync($"API disabled");

                // Белый IP
                if (jsonDB.API.WhiteIP == httpContext.Connection.RemoteIpAddress.ToString())
                    return _next(httpContext);

                // Проверяем авторизацию
                if (httpContext.Request.Headers.TryGetValue("Authorization", out var auth))
                {
                    // Авторизован
                    if (auth.ToString().Replace("Basic ", "") == Convert.ToBase64String(Encoding.ASCII.GetBytes($"{jsonDB.API.Login}:{jsonDB.API.Password}")))
                        return _next(httpContext);
                }

                // Пользователь не авторизован
                return httpContext.Response.WriteAsync("Not authorized");
            }

            return _next(httpContext);
        }
    }
    

    public static class AuthApiMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthApiMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthApiMiddleware>();
        }
    }
}
