using ISPCore.Models.Base;

namespace ISPCore.Models.Home
{
    /// <summary>
    /// Главная страница
    /// Журнал авторизации
    /// </summary>
    public class Jurnal : JurnalBase
    {
        /// <summary>
        /// Действие
        /// </summary>
        public string Msg { get; set; }
    }
}
