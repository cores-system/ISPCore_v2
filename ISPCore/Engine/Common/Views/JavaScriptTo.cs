using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace ISPCore.Engine.Common.Views
{
    public static class JavaScriptTo
    {
        public static IHtmlContent pushState(HttpContext context)
        {
            string uri = context.Request.Path.Value + Regex.Replace(context.Request.QueryString.Value, @"(&|\?)ajax=(true|false)", "");
            return new HtmlString($"<script>window.history.pushState('', '', '{uri}');</script>");
        }
    }
}
