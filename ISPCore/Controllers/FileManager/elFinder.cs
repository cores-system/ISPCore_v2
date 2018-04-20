using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using elFinder.NetCore;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.IO;
using ISPCore.Engine.Base;

namespace ISPCore.Controllers
{
    public class FileManager : Controller
    {
        public IActionResult Index(bool ajax)
        {
            ViewData["ajax"] = ajax;
            return View("/Views/FileManager/Base.cshtml");
        }

        public virtual async Task<IActionResult> Сonnector()
        {
            var connector = GetConnector();
            return await connector.Process(HttpContext.Request);
        }
        
        public IActionResult Thumbs(string hash)
        {
            var connector = GetConnector();
            return connector.GetThumbnail(HttpContext.Request, HttpContext.Response, hash);
        }

        private Connector GetConnector()
        {
            var driver = new FileSystemDriver();

            string absoluteUrl = UriHelper.BuildAbsolute(Request.Scheme, Request.Host);
            var uri = new Uri(absoluteUrl);

            var root = new Root(
                new DirectoryInfo("C:/Users/htc/Desktop/test"),
                $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/file-manager/target/",
                $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/file-manager/thumb/")
            {
                IsReadOnly = Platform.IsDemo, // Can be readonly according to user's membership permission
                //Alias = "elFinder", // Beautiful name given to the root/home folder
                MaxUploadSizeInKb = 200000, // Limit imposed to user uploaded file <= 200000 KB / 2GB
                //LockedFolders = new List<string>(new string[] { "Folder1" })
            };

            driver.AddRoot(root);

            return new Connector(driver);
        }


        public IActionResult Target(string targetfile)
        {
            try
            {
                return File(System.IO.File.OpenRead($"C:/Users/htc/Desktop/test/{targetfile}"), MimeTypeMap.GetMimeType(Path.GetExtension(targetfile)));
            }
            catch (Exception ex) { return Content(ex.ToString()); }
        }
    }
}
