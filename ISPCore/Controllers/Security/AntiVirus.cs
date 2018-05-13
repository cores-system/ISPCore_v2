using System;
using Microsoft.AspNetCore.Mvc;
using ISPCore.Engine.Base;
using System.Text;
using ISPCore.Engine;
using ISPCore.Models.Security;
using ISPCore.Models.Response;
using ISPCore.Engine.Common.Views;
using System.IO;
using System.Linq;
using ISPCore.Models.Databases;
using Trigger = ISPCore.Models.Triggers.Events.Security.AntiVirus;

namespace ISPCore.Controllers
{
    public class SecurityToAntiVirusController : ControllerToDB
    {
        #region Index
        [HttpGet]
        public IActionResult Index(bool ajax, string ShowReportToFolderPath, int page = 1)
        {
            // Дополнительные переметры
            ViewBag.ShowProgres = page == 1 && ShowReportToFolderPath == null;
            ShowReportToFolderPath = ShowReportToFolderPath?.Replace("/", "_-_");

            // Получаем список файлов и сортируем по LastWriteTime
            var ReportsAV = new DirectoryInfo(Folders.ReportsAV).GetFiles("*.html").
                            Where(i => ShowReportToFolderPath == null || i.Name.Contains(ShowReportToFolderPath)).
                            OrderByDescending(b => b.LastWriteTime).
                            Select(f => f.Name);

            // Выводим контент
            var navPage = new NavPage<string>(ReportsAV, HttpContext, 20, page, reverse: false);
            return View("~/Views/Security/AntiVirus/Index.cshtml", navPage, ajax);
        }
        #endregion

        #region Save
        [HttpPost]
        public JsonResult Save(AntiVirus av, bool IsAPI = false)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            if (string.IsNullOrWhiteSpace(av.path))
                return Json(new Text("Укажите каталог для сканирования"));

            //Обновляем базу
            jsonDB.AntiVirus = av;
            jsonDB.Save();
            Trigger.OnChange((0, 0));

            // Ответ
            if (IsAPI)
                return Json(new TrueOrFalse(true));
            return Json(new Text("Настройки успешно сохранены"));
        }
        #endregion

        #region Remove
        [HttpPost]
        public JsonResult Remove(string FileName)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            System.IO.File.Delete($"{Folders.ReportsAV}/{FileName}");
            return Json(new TrueOrFalse(true));
        }
        #endregion

        #region Start
        [HttpPost]
        public JsonResult Start(AntiVirus av)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            #region Проверяем поля
            if (AntiVirus.IsRun(0))
                return Json(new Text("Антивирус уже запущен"));

            if (string.IsNullOrWhiteSpace(av.path))
                return Json(new Text("Укажите каталог для сканирования"));

            if (!System.IO.File.Exists(av.php))
                return Json(new Text($"Отсутствует исполняемый файл '{av.php}'"));
            #endregion

            #region Создаем команду
            StringBuilder comand = new StringBuilder();
            comand.Append($"--path={av.path} ");

            if (!string.IsNullOrWhiteSpace(av.skip))
                comand.Append($"--skip={av.skip} ");

            if (!string.IsNullOrWhiteSpace(av.scan))
                comand.Append($"--scan={av.scan} ");

            comand.Append($"--mode={(av.mode == 1 ? 1 : 2)} ");
            comand.Append($"--memory={av.memory}M ");
            comand.Append($"--size={av.size}K ");
            comand.Append($"--delay={av.delay} ");
            #endregion

            // Имя отчета
            string report = $"{AntiVirus.name}_{AntiVirus.vers}_{DateTime.Now.ToString("HH-mm_dd-MM-yyy")}{av.path.Replace("/", "_-_")}";

            // 
            Trigger.OnStart(("0", report));

            // Запускаем процесс bash
            Bash bash = new Bash();
            bash.Run($"{av.php} {Folders.AV}/ai-bolit.php {comand.ToString()} --progress={Folders.AV}/progress_id-0.json --report={Folders.ReportsAV}/{report}.html >/dev/null 2>/dev/null &");
            bash.Run("sleep 3");

            // Отдаем результат
            return Json(new TrueOrFalse(true));
        }
        #endregion

        #region Stop
        [HttpPost]
        public JsonResult Stop(int Id = 0)
        {
            #region Демо режим
            if (Platform.IsDemo)
                return Json(new Text("Операция недоступна в демо-режиме"));
            #endregion

            if (AntiVirus.IsRun(Id))
            {
                Bash bash = new Bash();
                string pid = bash.Run($"ps ux | grep \"/av/ai-bolit.php\" | grep \"/progress_id-{Id}.json\" " + " | grep -v \"grep\" | awk {'print $2'} | head -n 1");
                if (!string.IsNullOrWhiteSpace(pid))
                    bash.Run($"kill -9 {pid}");

                System.IO.File.Delete($"{Folders.AV}/progress_id-{Id}.json");
                Trigger.OnStop((Id.ToString(), null));
            }

            return Json(new TrueOrFalse(true));
        }
        #endregion
    }
}
