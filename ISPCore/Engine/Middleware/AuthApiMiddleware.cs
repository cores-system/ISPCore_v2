using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ISPCore.Models.Databases.json;
using System.Text;
using ISPCore.Engine.Auth;
using ISPCore.Models.RequestsFilter.Domains;
using Microsoft.Extensions.Caching.Memory;
using ISPCore.Engine.Base.SqlAndCache;

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
                // IP-адрес пользователя
                string IP = httpContext.Connection.RemoteIpAddress.ToString();

                #region Локальный запрос в API
                if (httpContext.Request.Headers.TryGetValue("ApiKey", out var apiKey) && !string.IsNullOrWhiteSpace(apiKey))
                {
                    var memoryCache = Service.Get<IMemoryCache>();
                    if (memoryCache.TryGetValue(KeyToMemoryCache.ApiToLocalKey(apiKey), out _))
                    {
                        LimitLogin.SuccessAuthorization(IP);
                        return _next(httpContext);
                    }
                    else { LimitLogin.FailAuthorization(IP, TypeBlockIP.global); }
                }
                #endregion

                // База
                var jsonDB = Service.Get<JsonDB>();

                // Если API выключен
                if (!jsonDB.API.Enabled)
                    return httpContext.Response.WriteAsync($"API disabled");

                // Белый IP
                if (jsonDB.API.WhiteIP == IP)
                    return _next(httpContext);

                // Проверяем авторизацию
                if (httpContext.Request.Headers.TryGetValue("Authorization", out var auth))
                {
                    // Проверка авторизации
                    if (auth.ToString().Replace("Basic ", "") == Convert.ToBase64String(Encoding.ASCII.GetBytes($"{jsonDB.API.Login}:{jsonDB.API.Password}")))
                    {
                        // Авторизован
                        LimitLogin.SuccessAuthorization(IP);
                        return _next(httpContext);
                    }
                    else
                    {
                        // Пароль или логин не совпадает 
                        LimitLogin.FailAuthorization(IP, TypeBlockIP.global);
                        return httpContext.Response.WriteAsync("Login or password does not match");
                    }
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
