using System.Text.RegularExpressions;

namespace ISPCore.Models.Response
{
    public class Html
    {
        public Html(string _html)
        {
            html = Regex.Replace(_html, "[\n\r\t]+", "");
        }

        public string html { get; private set; }
    }
}
