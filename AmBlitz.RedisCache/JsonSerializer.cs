using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace AmBlitz.RedisCache
{
    public static class JsonSerializer
    {
        private const int BufferSize = 64 * 1024;

        public static byte[] Serialize<T>(T value)
        {
            return Compress(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }

        public static T Deserialize<T>(byte[] value)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(Decompress(value)));
        }


        private static byte[] Compress(byte[] inputData)
        {
            using (var ms = new MemoryStream())
            {
                using (var gzs = new BufferedStream(new GZipStream(ms, CompressionMode.Compress), BufferSize))
                {
                    gzs.Write(inputData, 0, inputData.Length);
                }
                return ms.ToArray();
            }
        }

        private static byte[] Decompress(byte[] inputData)
        {
            using (var cMs = new MemoryStream(inputData))
            {
                using (var dMs = new MemoryStream())
                {
                    using (var gzs = new BufferedStream(new GZipStream(cMs, CompressionMode.Decompress), BufferSize))
                    {
                        gzs.CopyTo(dMs);
                    }
                    return dMs.ToArray();
                }
            }
        }
    }
}
