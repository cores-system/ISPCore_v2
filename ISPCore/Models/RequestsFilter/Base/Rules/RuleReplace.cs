using ISPCore.Models.Databases.Interface;
using ISPCore.Models.RequestsFilter.Domains.Types;

namespace ISPCore.Models.RequestsFilter.Base.Rules
{
    public class RuleReplace : IId
    {
        public int Id { get; set; }

        /// <summary>
        /// Статус правила
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// URL для замены ответа
        /// </summary>
        public string uri { get; set; }

        /// <summary>
        /// GET аргументы
        /// </summary>
        public string GetArgs { get; set; }

        /// <summary>
        /// POST аргументы
        /// </summary>
        public string PostArgs { get; set; }

        /// <summary>
        /// Разрешеные символы
        /// </summary>
        public string RegexWhite { get; set; }

        /// <summary>
        /// Тип ответа
        /// </summary>
        public TypeResponseRule TypeResponse { get; set; }

        #region Тип ответа - "302"
        /// <summary>
        /// Куда отправить пользователя "/{arg}"
        /// </summary>
        public string ResponceUri { get; set; }
        #endregion

        #region Тип ответа - "kode"
        /// <summary>
        /// Content-Type ответа
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Пользовательский под
        /// </summary>
        public string kode { get; set; }
        #endregion
    }
}
