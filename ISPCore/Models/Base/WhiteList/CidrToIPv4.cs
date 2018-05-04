using System;

namespace ISPCore.Models.Base.WhiteList
{
    public class CidrToIPv4
    {
        public CidrToIPv4(ulong firstUsable, ulong lastUsable)
        {
            this.FirstUsable = firstUsable;
            this.LastUsable = lastUsable;
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
