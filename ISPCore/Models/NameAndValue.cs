namespace ISPCore.Models
{
    public class NameAndValue
    {
        public NameAndValue() { }
        public NameAndValue(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public int Id { get; set; }
        private string _name, _value;

        /// <summary>
        /// Имя
        /// </summary>
        public string Name
        {
            get { return _name?.Trim(); }
            set { _name = value; }
        }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value
        {
            get { return _value?.Trim(); }
            set { _value = value; }
        }
    }
}
