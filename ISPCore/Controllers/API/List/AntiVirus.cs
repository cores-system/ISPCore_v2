using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using ISPCore.Engine.Base;
using ISPCore.Models.Security;
using System.Text.RegularExpressions;

namespace ISPCore.Controllers
{
    public class ApiListAntiVirus : Controller
    {
        public JsonResult Report(int page = 1, int pageSize = 20, string search = null)
        {
            List<dynamic> mass = new List<dynamic>();

            #region Активные задания
            if (search == null)
            {
                foreach (var intFile in Directory.GetFiles(Folders.AV, "progress_id-*.json", SearchOption.TopDirectoryOnly))
                {
                    mass.Add(new
                    {
                        @Report = $"{Path.GetFileNameWithoutExtension(intFile)} - {new Regex("\"progress\":\"([0-9\\.]+)\"").Match(System.IO.File.ReadAllText(intFile)).Groups[1].Value}%",
                        @av = AntiVirus.name.Replace("-", "/"),
                        @VersAV = AntiVirus.vers
                    });
                }
            }
            #endregion

            #region Завершеные задания
            var ReportsAV = new DirectoryInfo(Folders.ReportsAV).GetFiles("*.html").
                            Where(i => search == null || i.Name.Contains(search.Replace("/", "_-_"))).
                            OrderBy(b => b.LastWriteTime).
                            Select(f => f.Name).
                            Reverse().Skip((page * pageSize) - pageSize).Take(pageSize);

            foreach (var FileName in ReportsAV.Take(pageSize))
            {
                var gr = new Regex(@"([^_]+)_([0-9\-]+)_([0-9\-]+)_([0-9\-]+)(_.*)\.html$", RegexOptions.IgnoreCase).Match(FileName).Groups;
                mass.Add(new
                {
                    @Report = gr[5].Value.Replace("_-_", "/"),
                    @av = gr[1].Value,
                    @VersAV = gr[2].Value.Replace("-", "/"),
                    @Time = gr[3].Value.Replace("-", ":"),
                    @Date = gr[4].Value.Replace("-", ".")
                });
            }
            #endregion

            return Json(mass);
        }
    }
}
