using ISPCore.Models.FileManager;
using ISPCore.Models.FileManager.Response;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;

namespace ISPCore.Engine.FileManager
{
    public class FileSystemDriver : elFinder.NetCore.FileSystemDriver
    {
        #region private
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        private JsonResult Json(object ob, string charset = null) => new JsonResult(ob) { ContentType = $"text/html{(charset != null ? $"; charset={charset}" : "" )}" };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conv"></param>
        /// <returns></returns>
        private Encoding GetEncoding(string conv)
        {
            if (conv.ToLower() == "utf-8")
                return Encoding.UTF8;

            return CodePagesEncodingProvider.Instance.GetEncoding(conv);
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
    }
}
