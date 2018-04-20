using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using elFinder.NetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISPCore.Engine.FileManager
{
    public class Connector : elFinder.NetCore.Connector
    {
        private FileSystemDriver driver;
        public Connector(FileSystemDriver driver) : base(driver)
        {
            this.driver = driver;
        }


        public new IActionResult Process(HttpRequest request)
        {
            IDictionary<string, string> parameters = request.Query.Count > 0
                ? request.Query.ToDictionary(k => k.Key, v => string.Join(";", v.Value))
                : request.Form.ToDictionary(k => k.Key, v => string.Join(";", v.Value));

            // Переопределяем логику
            string cmd = parameters["cmd"];
            if (!string.IsNullOrWhiteSpace(cmd))
            {
                string target = parameters.GetValueOrDefault("target");
                if (!string.IsNullOrWhiteSpace(target) && target.ToLower() != "null")
                {
                    switch (cmd)
                    {
                        case "get":
                            {
                                // Content Encoding
                                if (parameters.TryGetValue("conv", out string conv) && conv != "0")
                                    return driver.GetAsync(target, conv);

                                // Оригинал файла
                                return driver.GetAsync(target).Result;
                            }

                        case "put":
                            {
                                string content = parameters.GetValueOrDefault("content");
                                if (!string.IsNullOrWhiteSpace(target))
                                {
                                    // Content Encoding
                                    if (parameters.TryGetValue("encoding", out string conv))
                                        return driver.PutAsync(target, content, conv);

                                    // Оригинал
                                    return driver.PutAsync(target, content).Result;
                                }
                                break;
                            }
                    }
                }
            }

            // Базовая логика
            return base.Process(request).Result;
        }
    }
}
