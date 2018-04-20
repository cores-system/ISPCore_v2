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
                if (string.IsNullOrWhiteSpace(target) || target.ToLower() == "null")
                    target = null;

                switch (cmd)
                {
                    case "get":
                        {
                            if (target == null)
                                return MissedParameter(cmd);

                            // Content Encoding
                            if (parameters.TryGetValue("conv", out string conv) && conv != "0")
                                return driver.GetAsync(target, conv);

                            // Оригинал файла
                            return driver.GetAsync(target).Result;
                        }

                    case "put":
                        {
                            if (target == null)
                                return MissedParameter(cmd);

                            string content = parameters.GetValueOrDefault("content");
                            if (string.IsNullOrWhiteSpace(target))
                                return MissedParameter("content");

                            // Content Encoding
                            if (parameters.TryGetValue("encoding", out string conv))
                                return driver.PutAsync(target, content, conv);

                            // Оригинал
                            return driver.PutAsync(target, content).Result;
                        }

                    case "paste":
                        {
                            IEnumerable<string> targets = GetTargetsArray(request);
                            if (targets == null)
                                return MissedParameter("targets");

                            string dst = parameters.GetValueOrDefault("dst");
                            if (string.IsNullOrEmpty(dst))
                                return MissedParameter("dst");

                            return driver.PasteAsync(null, dst, targets, parameters.GetValueOrDefault("cut") == "1").Result;
                        }
                }
            }

            // Базовая логика
            return base.Process(request).Result;
        }
        #endregion

        #region GetTargetsArray
        private IEnumerable<string> GetTargetsArray(HttpRequest request)
        {
            IEnumerable<string> targets = null;
            // At the moment, request.Form is throwing an InvalidOperationException...
            //if (request.Form.ContainsKey("targets"))
            //{
            //    targets = request.Form["targets"];
            //}

            IDictionary<string, string> parameters = request.Query.Count > 0
                ? request.Query.ToDictionary(k => k.Key, v => string.Join(";", v.Value))
                : request.Form.ToDictionary(k => k.Key, v => string.Join(";", v.Value));

            if (targets == null)
            {
                string t = parameters.GetValueOrDefault("targets[]");
                if (string.IsNullOrEmpty(t))
                {
                    t = parameters.GetValueOrDefault("targets");
                }
                if (string.IsNullOrEmpty(t))
                {
                    return null;
                }
                targets = t.Split(';'); // 2018.02.23: Bug Fix Issue #3
            }
            return targets;
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
