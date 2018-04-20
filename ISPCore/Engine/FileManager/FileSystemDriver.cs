using ISPCore.Models.FileManager;
using ISPCore.Models.FileManager.Response;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ISPCore.Engine.FileManager
{
    public class FileSystemDriver : elFinder.NetCore.FileSystemDriver
    {
        #region Json/GetEncoding
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        private JsonResult Json(object ob, string charset = null) => new JsonResult(ob) { ContentType = $"text/html{(charset != null ? $"; charset={charset}" : "" )}" };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conv"></param>
        private Encoding GetEncoding(string conv)
        {
            if (conv.ToLower() == "utf-8")
                return Encoding.UTF8;

            return CodePagesEncodingProvider.Instance.GetEncoding(conv);
        }
        #endregion

        #region DirectoryCopy
        private void DirectoryCopy(DirectoryInfo sourceDir, string destDirName, bool copySubDirs)
        {
            DirectoryInfo[] dirs = sourceDir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!sourceDir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDir.FullName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the file contents of the directory to copy.
            FileInfo[] files = sourceDir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, false);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir, temppath, copySubDirs);
                }
            }
        }
        #endregion

        #region GetAsync
        public JsonResult GetAsync(string target, string conv)
        {
            var fullPath = ParsePath(target);
            var response = new GetResponseModel();
            
            using (var reader = new StreamReader(fullPath.File.OpenRead(), GetEncoding(conv)))
            {
                response.Encoding = conv;
                response.Content = reader.ReadToEnd();
            }

            return Json(response, "utf-8");
        }
        #endregion

        #region PutAsync
        public JsonResult PutAsync(string target, string content, string conv)
        {
            var fullPath = ParsePath(target);
            var response = new ChangedResponseModel();

            using (var fileStream = new FileStream(fullPath.File.FullName, FileMode.Create))
            {
                using (var writer = new StreamWriter(fileStream, GetEncoding(conv)))
                {
                    writer.Write(content);
                }
            }

            response.Changed.Add((FileModel)BaseModel.Create(fullPath.File, fullPath.Root));
            return Json(response);
        }
        #endregion

        #region DuplicateAsync
        public new JsonResult DuplicateAsync(IEnumerable<string> targets)
        {
            var response = new AddResponseModel();
            foreach (var target in targets)
            {
                var fullPath = ParsePath(target);
                if (fullPath.Directory != null)
                {
                    var parentPath = fullPath.Directory.Parent.FullName;
                    var name = fullPath.Directory.Name;
                    var newName = string.Format(@"{0}/{1} copy", parentPath, name);
                    if (!Directory.Exists(newName))
                    {
                        DirectoryCopy(fullPath.Directory, newName, true);
                    }
                    else
                    {
                        for (int i = 1; i < 100; i++)
                        {
                            newName = string.Format(@"{0}/{1} copy {2}", parentPath, name, i);
                            if (!Directory.Exists(newName))
                            {
                                DirectoryCopy(fullPath.Directory, newName, true);
                                break;
                            }
                        }
                    }
                    response.Added.Add(BaseModel.Create(new DirectoryInfo(newName), fullPath.Root));
                }
                else
                {
                    var parentPath = fullPath.File.Directory.FullName;
                    var name = fullPath.File.Name.Substring(0, fullPath.File.Name.Length - fullPath.File.Extension.Length);
                    var ext = fullPath.File.Extension;

                    var newName = string.Format(@"{0}/{1} copy{2}", parentPath, name, ext);

                    if (!File.Exists(newName))
                    {
                        fullPath.File.CopyTo(newName);
                    }
                    else
                    {
                        for (int i = 1; i < 100; i++)
                        {
                            newName = string.Format(@"{0}/{1} copy {2}{3}", parentPath, name, i, ext);
                            if (!File.Exists(newName))
                            {
                                fullPath.File.CopyTo(newName);
                                break;
                            }
                        }
                    }
                    response.Added.Add(BaseModel.Create(new FileInfo(newName), fullPath.Root));
                }
            }
            return Json(response);
        }
        #endregion
    }
}
