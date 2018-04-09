using System;
using System.IO;
using System.IO.Compression;

namespace ISPCore.Engine.Base
{
    public class GZip
    {
        public static void Compress(string inFile, string outFile)
        {
            try
            {
                // Поток для чтения исходного файла
                using (FileStream sourceStream = new FileStream(inFile, FileMode.Open))
                {
                    // Поток для записи сжатого файла
                    using (FileStream targetStream = new FileStream(outFile, FileMode.Create, FileAccess.Write))
                    {
                        // Поток архивации
                        using (GZipStream compressionStream = new GZipStream(targetStream, CompressionLevel.Optimal))
                        {
                            // Создаем архив
                            sourceStream.CopyTo(compressionStream);
                        }
                    }
                }
            }
            catch { }
        }
    }
}
