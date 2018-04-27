using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using ISPCore.Engine.Base;
using ISPCore.Engine.FileManager;
using ISPCore.Models.Base;

namespace ISPCore.Controllers
{
    public class FileManager : Controller
    {
        public IActionResult Index(bool ajax)
        {
            ViewData["ajax"] = ajax;
            return View("/Views/FileManager/elFinder.cshtml");
        }

        public virtual IActionResult Сonnector()
        {
            var connector = GetConnector();
            return connector.Process(HttpContext.Request);
        }
        
        public IActionResult Thumbs(string hash)
        {
            var connector = GetConnector();
            return connector.GetThumbnail(HttpContext.Request, HttpContext.Response, hash);
        }

        public IActionResult Target(string targetfile)
        {
            try
            {
                return File(System.IO.File.OpenRead($"{DirectoryRoot}/{targetfile}"), MimeTypeMap.GetMimeType(Path.GetExtension(targetfile)));
            }
            catch (Exception ex) { return Content(ex.ToString()); }
        }


        private Connector GetConnector()
        {
            var driver = new FileSystemDriver();
            driver.IsUnix = Platform.Get == PlatformOS.Unix;

            var root = new elFinder.NetCore.Root(
                new DirectoryInfo(DirectoryRoot),
                $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/file-manager/target/",
                $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/file-manager/thumb/")
            {
                IsReadOnly = Platform.IsDemo, // Can be readonly according to user's membership permission
                //Alias = "elFinder", // Beautiful name given to the root/home folder
                MaxUploadSizeInKb = 200000, // Limit imposed to user uploaded file <= 200000 KB / 2GB
                //LockedFolders = new List<string>(new string[] { "Folder1" })
                IsUnix = Platform.Get == PlatformOS.Unix
            };

            driver.AddRoot(root);

            return new Connector(driver);
        }


        private string DirectoryRoot
        {
            get
            {
                if (Platform.IsDebug)
                    return "C:/Users/htc/Desktop/test";

                if (Platform.IsDemo)
                    return "/home/demo/elFinder";

                // Linux/Docker
                return "/";
            }
        }
    }
}
