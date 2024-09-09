using System.IO;
using System.IO.Compression;
using System.Text;

namespace GameIdea2.Compression
{
    public class StringCompression
    {
        public static byte[] CompressString(string input)
        {
            byte[] jsonBytes = Encoding.UTF8.GetBytes(input);

            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    gzipStream.Write(jsonBytes, 0, jsonBytes.Length);
                }
                return memoryStream.ToArray();
            }
        }

        public static string DecompressString(byte[] input)
        {
            using (var memoryStream = new MemoryStream(input))
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        gzipStream.CopyTo(resultStream);
                        byte[] decompressedBytes = resultStream.ToArray();
                        return Encoding.UTF8.GetString(decompressedBytes);
                    }
                }
            }
        }
    }
}