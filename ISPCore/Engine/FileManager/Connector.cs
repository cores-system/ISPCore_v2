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

        #region Process
        public new async Task<IActionResult> ProcessAsync(HttpRequest request)
        {
            var parameters = request.Query.Any()
                ? request.Query.ToDictionary(k => k.Key, v => v.Value)
                : request.Form.ToDictionary(k => k.Key, v => v.Value);

            // Переопределяем логику
            string cmd = parameters.GetValueOrDefault("cmd");
            if (!string.IsNullOrWhiteSpace(cmd))
            {
                string target = parameters.GetValueOrDefault("target");
                if (string.IsNullOrEmpty(target) || target.ToLower() == "null")
                    target = null;

                switch (cmd)
                {
                    case "get":
                        {
                            if (target == null)
                                return MissedParameter(cmd);

                            var path = await driver.ParsePathAsync(target);

                            // Content Encoding
                            if (parameters.TryGetValue("conv", out var conv) && conv != "0")
                                return await driver.GetAsync(path, conv.ToString());

                            // Оригинал файла
                            return await driver.GetAsync(path);
                        }

                    case "put":
                        {
                            if (target == null)
                                return MissedParameter(cmd);

                            var path = await driver.ParsePathAsync(target);
                            string content = parameters.GetValueOrDefault("content");

                            // Content Encoding
                            if (parameters.TryGetValue("encoding", out var conv))
                                return await driver.PutAsync(path, content, conv.ToString());

                            // Оригинал
                            return await driver.PutAsync(path, content);
                        }
                }
            }

            // Базовая логика
            return await base.ProcessAsync(request);
        }
        #endregion

        #region MissedParameter
        private JsonResult MissedParameter(string command)
        {
            return new JsonResult(new { error = new string[] { "errCmdParams", command } });
        }
        #endregion
    }
}
