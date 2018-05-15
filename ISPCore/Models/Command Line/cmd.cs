namespace ISPCore.Models.Command_Line
{
    public class cmd
    {
        /// <summary>
        /// 
        /// </summary>
        public Timeout Timeout { get; set; } = new Timeout();

        /// <summary>
        /// Где можно не возвращать ответ в html
        /// </summary>
        public StatusCode StatusCode { get; set; } = new StatusCode();
    }
}
