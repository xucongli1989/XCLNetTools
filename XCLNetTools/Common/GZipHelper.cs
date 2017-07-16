using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace XCLNetTools.Common
{
    /// <summary>
    /// gzip相关
    /// </summary>
    public static class GZipHelper
    {
        /// <summary>
        /// gzip压缩字符串
        /// </summary>
        /// <param name="text">待压缩的字符串</param>
        /// <returns>压缩后的值</returns>
        public static string GZipCompressString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            byte[] buffer = Encoding.UTF8.GetBytes(text);

            using (var memoryStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gZipStream.Write(buffer, 0, buffer.Length);
                }
                memoryStream.Position = 0;
                var compressedData = new byte[memoryStream.Length];
                memoryStream.Read(compressedData, 0, compressedData.Length);

                var gZipBuffer = new byte[compressedData.Length + 4];
                Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
                return Convert.ToBase64String(gZipBuffer);
            }
        }

        /// <summary>
        /// 解压缩字符串
        /// </summary>
        /// <param name="compressedText">待解压的字符串</param>
        /// <returns>解压后的值</returns>
        public static string GZipDecompressString(string compressedText)
        {
            if (string.IsNullOrEmpty(compressedText))
            {
                return string.Empty;
            }
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);
                var buffer = new byte[dataLength];
                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }
                return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}