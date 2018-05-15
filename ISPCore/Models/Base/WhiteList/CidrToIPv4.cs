namespace ISPCore.Models.Base.WhiteList
{
    public class CidrToIPv4
    {
        /// <summary>
        /// Числовая модель IPv4
        /// </summary>
        /// <param name="firstUsable">Начальное число</param>
        /// <param name="lastUsable">Конечное число</param>
        public CidrToIPv4(ulong firstUsable, ulong lastUsable)
        {
            this.FirstUsable = firstUsable;
            this.LastUsable = lastUsable;
        }

        /// <summary>
        /// Начальное число
        /// </summary>
        public ulong FirstUsable { get; }

        /// <summary>
        /// Конечное число
        /// </summary>
        public ulong LastUsable { get; }
    }
}
