using ISPCore.Models.Databases.Interface;

namespace ISPCore.Models.Base.WhiteList
{
    public class WhiteListModel : IId
    {
        public WhiteListModel() { }
        public WhiteListModel(string Description, string Value, WhiteListType Type)
        {
            this.Description = Description;
            this.Value = Value;
            this.Type = Type;
        }

        /// <summary>
        /// Id задания
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Тип записи
        /// </summary>
        public WhiteListType Type { get; set; }
    }
}
