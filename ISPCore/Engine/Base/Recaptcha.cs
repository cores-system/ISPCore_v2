using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ISPCore.Engine.Base
{
    public class Recaptcha
    {
        async public static Task<bool> Verify(string response, string secret)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var postParams = new Dictionary<string, string>();

                    postParams.Add("secret", secret);
                    postParams.Add("response", response);

                    using (var postContent = new FormUrlEncodedContent(postParams))
                    {
                        using (HttpResponseMessage res = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", postContent))
                        {
                            string content = await res.Content.ReadAsStringAsync();
                            return content.Contains("\"success\": true,");
                        }
                    }
                }
            }
            catch { return false; }
        }
    }
}
