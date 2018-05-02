namespace ISPCore.Models.Base.WhiteList
{
    public class CidrToIPv4
    {
        public CidrToIPv4(ulong FirstUsable, ulong LastUsable)
        {
            this.FirstUsable = FirstUsable;
            this.LastUsable = LastUsable;
        }

        /// <summary>
        /// 
        /// </summary>
        public ulong FirstUsable { get; }

        /// <summary>
        /// 
        /// </summary>
        public ulong LastUsable { get; }
    }
}
