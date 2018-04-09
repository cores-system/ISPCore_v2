namespace ISPCore.Models.Base.Notification
{
    public class MoreBase
    {
        public MoreBase() { }
        public MoreBase(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public int Id { get; set; }
        public int NotationId { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }
    }
}
