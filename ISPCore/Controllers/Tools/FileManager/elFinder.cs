using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using ISPCore.Engine.Base;
using System.Threading.Tasks;
using ISPCore.Engine.FileManager;

namespace ISPCore.Controllers
{
    public class ToolsToFileManager : Controller
    {
        public IActionResult Index(bool ajax)
        {
            ViewData["ajax"] = ajax;
            return View("~/Views/Tools/FileManager/elFinder.cshtml");
        }

        public async Task<IActionResult> Connector()
        {
            var connector = GetConnector();
            return await connector.ProcessAsync(Request);
        }

        async public Task<IActionResult> Thumbs(string hash)
        {
            var connector = GetConnector();
            return await connector.GetThumbnailAsync(HttpContext.Request, HttpContext.Response, hash);
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

            var root = new elFinder.NetCore.RootVolume(
                DirectoryRoot,
                $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/file-manager/target/",
                $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/file-manager/thumb/")
            {
                IsReadOnly = Platform.IsDemo,
                Alias = "elFinder", // Beautiful name given to the root/home folder
                MaxUploadSizeInKb = 200000, // Limit imposed to user uploaded file <= 200000 KB / 2GB
                //LockedFolders = new List<string>(new string[] { "Folder1" })
            };

            driver.AddRoot(root);

            return new Connector(driver);

        }


        private string DirectoryRoot
        {
            get
            {
                if (Platform.IsDebug)
                    return @"C:\Users\htc\Desktop\test";

                if (Platform.IsDemo)
                    return "/home/demo/elFinder";

                // Linux/Docker
                return "/";
            }
        }
    }
}
