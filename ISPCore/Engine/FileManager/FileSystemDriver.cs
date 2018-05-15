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
        /// Получить json
        /// </summary>
        /// <param name="ob">Данные для конверта</param>
        /// <param name="charset">Кодировка</param>
        private JsonResult Json(object ob, string charset = null) => new JsonResult(ob) { ContentType = $"text/html{(charset != null ? $"; charset={charset}" : "" )}" };

        /// <summary>
        /// Поток кодировки
        /// </summary>
        /// <param name="conv">Кодировка</param>
        private Encoding GetEncoding(string conv)
        {
            if (conv.ToLower() == "utf-8")
                return Encoding.UTF8;

            return CodePagesEncodingProvider.Instance.GetEncoding(conv);
        }
        #endregion

        #region GetAsync
        /// <summary>
        /// Получить текстовый документ
        /// </summary>
        /// <param name="target">Цель в формате elFinder</param>
        /// <param name="conv">Кодировка</param>
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
        /// <summary>
        /// Сохранить текстовый документ
        /// </summary>
        /// <param name="target">Цель в формате elFinder</param>
        /// <param name="content">Текст</param>
        /// <param name="conv">Кодировка</param>
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
