namespace ISPCore.Models.Command_Line
{
    public class cmd
    {
        /// <summary>
        /// 
        /// </summary>
        public Timeout Timeout { get; set; } = new Timeout();

        /// <summary>
        /// 
        /// </summary>
        public StatusCode StatusCode { get; set; } = new StatusCode();

        /// <summary>
        /// 
        /// </summary>
        public Cache Cache { get; set; } = new Cache();
    }
}
