namespace ISPCore.Models.Command_Line
{
    public class Cache
    {
        /// <summary>
        /// Время в милисекундах
        /// </summary>
        public int Checklink { get; set; } = 1000; // 1s

        /// <summary>
        /// Время в милисекундах
        /// </summary>
        public int AntiBot { get; set; } = 300;
    }
}
