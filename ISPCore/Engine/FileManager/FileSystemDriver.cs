using elFinder.NetCore;
using elFinder.NetCore.Models;
using elFinder.NetCore.Models.Commands;
using ISPCore.Models.FileManager;
using ISPCore.Models.FileManager.Response;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ISPCore.Engine.FileManager
{
    public class FileSystemDriver : elFinder.NetCore.Drivers.FileSystem.FileSystemDriver
    {
        #region Json/GetEncoding
        /// <summary>
        /// Получить json
        /// </summary>
        /// <param name="ob">Данные для конверта</param>
        /// <param name="charset">Кодировка</param>
        private JsonResult Json(object ob, string charset = null) => new JsonResult(ob) { ContentType = $"text/html{(charset != null ? $"; charset={charset}" : "")}" };

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
        public async Task<JsonResult> GetAsync(FullPath path, string conv)
        {
            var response = new Models.FileManager.Response.GetResponseModel();
            using (var reader = new StreamReader(await path.File.OpenReadAsync(), GetEncoding(conv)))
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
        async public Task<JsonResult> PutAsync(FullPath path, string content, string conv)
        {
            var response = new ChangedResponseModel();
            using (var fileStream = new FileStream(path.File.FullName, FileMode.Create))
            {
                using (var writer = new StreamWriter(fileStream, GetEncoding(conv)))
                {
                    writer.Write(content);
                }
            }
            response.Changed.Add((FileModel)await BaseModel.CreateAsync(this, path.File, path.RootVolume));
            return Json(response);
        }
        #endregion
    }
}
