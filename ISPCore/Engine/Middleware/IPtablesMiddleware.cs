using System;
using System.Threading.Tasks;
using ISPCore.Engine.Security;
using ISPCore.Models.RequestsFilter.Monitoring;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using ModelIPtables = ISPCore.Models.Security.IPtables;

namespace ISPCore.Engine.Middleware
{
    public class IPtablesMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IMemoryCache memoryCache;

        public IPtablesMiddleware(RequestDelegate _next, IMemoryCache memCache)
        {
            memoryCache = memCache;
            next = _next;
        }


        public Task Invoke(HttpContext httpContext)
        {
            // Поиск IP в кеше для блокировки пользователя
            if (IPtables.CheckIP(httpContext.Connection.RemoteIpAddress.ToString(), out ModelIPtables data))
            {
                // Статистика
                Engine.core.Check.Request.SetCountRequestToHour(TypeRequest.IPtables, "global", true);

                httpContext.Response.StatusCode = 401;
                if (Startup.cmd.StatusCode.IPtables)
                    return Task.FromResult(true);

                httpContext.Response.ContentType = "text/html";
                return httpContext.Response.WriteAsync(IPtables.BlockedToHtml(httpContext.Connection.RemoteIpAddress.ToString(), data.Description, data.TimeExpires));
            }

            // Идем дальше
            return next(httpContext);
        }
    }
    

    public static class IPtablesMiddlewareExtensions
    {
        public static IApplicationBuilder UseIPtablesMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IPtablesMiddleware>();
        }
    }
}
