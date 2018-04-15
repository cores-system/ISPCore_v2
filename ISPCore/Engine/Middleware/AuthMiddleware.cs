using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ISPCore.Engine.Auth;
using ISPCore.Engine.Base;

namespace ISPCore.Engine.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            #region Заголовок X-Forwarded-For запрещен
            if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out _))
            {
                httpContext.Response.ContentType = "text/plain; charset=utf-8";
                return httpContext.Response.WriteAsync("Заголовок X-Forwarded-For запрещен");
            }
            #endregion

            // IP адрес пользователя
            string IP = httpContext.Connection.RemoteIpAddress.ToString();

            // Проверка кук для прохождения авторизации
            if (IsAuth.Auth(httpContext.Request.Cookies, IP, out bool IsConfirm2FA))
            {
                // Авторизация 2FA
#warning Авторизация 2FA

                // Успех
                return _next(httpContext);
            }

            // Редикт на страницу авторизациии
            return RewriteTo.Local(httpContext, "auth");
        }
    }


    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
