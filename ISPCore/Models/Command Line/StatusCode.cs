namespace ISPCore.Models.Command_Line
{
    public class StatusCode
    {
        /// <summary>
        /// Не возвращать html в ответе
        /// </summary>
        public bool IPtables { get; set; }

        /// <summary>
        /// Не возвращать html в ответе
        /// </summary>
        public bool Checklink { get; set; }
    }
}
