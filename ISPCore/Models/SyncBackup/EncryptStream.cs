using System.IO;
using System.Security.Cryptography;

namespace ISPCore.Models.SyncBackup
{
    public class EncryptStream : CryptoStream
    {
        public EncryptStream(Stream stream, ICryptoTransform transform, CryptoStreamMode mode) : base(stream, transform, mode)
        {
        }

        private long _length = 0;
        public override void SetLength(long value)
        {
            _length = value;
        }

        public override long Length => _length;


        private long _position = 0;
        public override long Position { get => _position; set => _position = value; }
    }
}
