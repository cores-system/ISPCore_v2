using System.Linq;
using System.Collections.Generic;
using ISPCore.Models.Databases.Interface;

namespace ISPCore.Models.Response
{
    public class UpdateToIds
    {
        /// <summary>
        /// Отдает список новых полей и Id в базе SQL
        /// </summary>
        /// <param name="msg">Текст ответа</param>
        /// <param name="RewriteToId">Новый Id для сохранения результатов</param>
        /// <param name="massIds">Список полей которым нужно присвоить новый Id</param>
        public UpdateToIds(string msg, int RewriteToId, params IDictionary<string, IId>[] massIds)
        {
            foreach (var mass in massIds)
            {
                foreach (var item in mass) {
                    updateToIds.Add(new { @key=item.Key, @Id=item.Value.Id });
                }
            }

            this.msg = msg;
            this.Id = RewriteToId;
        }

        /// <summary>
        /// Аналог Models.Response.RewriteToId
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Аналог Models.Response.Text
        /// </summary>
        public string msg { get; private set; }

        /// <summary>
        /// Список полей которым нужно присвоить новый Id
        /// </summary>
        public List<dynamic> updateToIds { get; private set; } = new List<dynamic>();
    }
}
