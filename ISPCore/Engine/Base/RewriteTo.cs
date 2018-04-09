using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ISPCore.Engine.Base
{
    public class RewriteTo
    {
        /// <summary>
        /// Редикт 302
        /// </summary>
        /// <param name="httpContext">Контекст запроса</param>
        /// <param name="path">Страница на которую делать редикт</param>
        public static Task Local(HttpContext httpContext, string path)
        {
            string RewriteToLink = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}/{Regex.Replace(path, "^/", "")}";
            httpContext.Response.StatusCode = 302;
            httpContext.Response.ContentType = "text/html";
            httpContext.Response.Headers[HeaderNames.Location] = RewriteToLink;
            return httpContext.Response.WriteAsync($"<!DOCTYPE HTML><html lang=\"ru-RU\"><head><meta charset=\"UTF-8\"><meta http-equiv=\"refresh\" content=\"1; url={RewriteToLink}\"><script type=\"text/javascript\">window.location.href=\"{RewriteToLink}\"</script><title>Page Redirection</title></head><body>If you are not redirected automatically, follow this <a href=\"{RewriteToLink}\">link</a>.</body></html>", httpContext.RequestAborted);
        }
    }
}
