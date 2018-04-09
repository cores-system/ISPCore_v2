using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace ISPCore.Engine.Common.Views
{
    public static class ButtonTo
    {
        public static IHtmlContent Refresh(HttpContext context)
        {
            string uri = context.Request.Path.Value + Regex.Replace(context.Request.QueryString.Value, @"(&|\?)ajax=(true|false)", "");
            return new HtmlString($"<a href=\"{uri}\" class=\"btn btn-info btn-fixed\" onclick=\"return loadPage(this)\"><i class=\"fa fa-refresh\" aria-hidden=\"true\"></i></a>");
        }
    }
}
