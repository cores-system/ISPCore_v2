using ISPCore.Engine.Base;
using ISPCore.Engine.Base.SqlAndCache;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ISPCore.Engine.Triggers
{
    public class Invoke
    {
        #region Bash
        /// <summary>
        /// Выполнить Bash команду
        /// </summary>
        /// <param name="s">Команда</param>
        public string Bash(string s)
        {
            return new Bash().Run(s);
        }
        #endregion

        #region API
        /// <summary>
        /// Выполнить запрос в API
        /// </summary>
        /// <param name="uriPath">/add/requests-filter/aliases</param>
        /// <param name="args">"DomainId=1", "aliases[1].host=test.com"</param>
        public string API(string uriPath, params string[] args)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Уникальный ключ
                    string apiKey = Generate.Passwd(24);

                    // Добавляем ключ в память
                    var memoryCache = Service.Get<IMemoryCache>();
                    memoryCache.Set(KeyToMemoryCache.ApiToLocalKey(apiKey), (byte)0, TimeSpan.FromSeconds(90));

                    // Отправляем запрос в API
                    client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                    string arg = args.Length > 0 ? $"?{String.Join('&', args)}" : string.Empty;
                    return client.GetStringAsync($"http://127.0.0.1:4538/api{uriPath}{arg}").Result;
                }
            }
            catch (Exception ex) { return ex.ToString(); }
        }
        #endregion

        #region BrowserGet
        /// <summary>
        /// Отправить GET запрос
        /// </summary>
        /// <param name="url">http://site.com/</param>
        /// <param name="args">"IP=127.0.0.1", "arg=test"</param>
        public string BrowserGet(string url, params string[] args)
        {
            try
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                using (HttpClient client = new HttpClient(handler))
                {
                    // Отправляем GET запрос
                    client.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36");
                    return client.GetStringAsync($"{url}{(args.Length > 0 ? $"?{String.Join('&', args)}" : string.Empty)}").Result;
                }
            }
            catch (Exception ex) { return ex.ToString(); }
        }
        #endregion

        #region BrowserPost
        /// <summary>
        /// Отправить POST запрос
        /// </summary>
        /// <param name="url">http://site.com/</param>
        /// <param name="args">"IP=127.0.0.1", "arg=test"</param>
        public string BrowserPost(string url, params string[] args)
        {
            try
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                using (HttpClient client = new HttpClient(handler))
                {
                    // Пользовательские параметры
                    var postParams = new Dictionary<string, string>();
                    foreach (string item in args)
                    {
                        postParams.Add(item.Split('=')[0], item.Split('=')[1]);
                    }

                    // User-Agent
                    client.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36");

                    // Отправляем POST запрос
                    using (var postContent = new FormUrlEncodedContent(postParams))
                    {
                        using (HttpResponseMessage response = client.PostAsync(url, postContent).Result)
                        {
                            return response.RequestMessage.Content.ReadAsStringAsync().Result;
                        }
                    }
                }
            }
            catch (Exception ex) { return ex.ToString(); }
        }
        #endregion
    }
}
