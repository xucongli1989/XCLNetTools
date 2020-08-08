using System;
using System.IO;
using System.Text;

namespace XCLNetTools.FileHandler
{
    /// <summary>
    /// 自动判断文件编码帮助类（需要引入 System.Text.Encoding.CodePages）
    /// </summary>
    public static class TextCodeGuessHelper
    {
        private static Encoding UTF8 => Encoding.UTF8;
        private static Encoding GBK => Encoding.GetEncoding("GBK");

        static TextCodeGuessHelper()
        {
            RegisterMoreEncoding();
        }

        /// <summary>
        /// 注册更多的编码类型类型
        /// </summary>
        /// <remarks>
        /// <para>需要引入 NuGet包:System.Text.Encoding.CodePages</para>
        /// <para>调用下面的编码类型需要先调用此方法</para>
        /// <para>详情链接：https://docs.microsoft.com/zh-cn/dotnet/api/system.text.encoding.registerprovider?redirectedfrom=MSDN&view=netframework-4.8#System_Text_Encoding_RegisterProvider_System_Text_EncodingProvider_</para>
        /// </remarks>
        private static void RegisterMoreEncoding()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// 获取编码
        /// </summary>
        /// <param name="name">编码名称，如(UTF-8)</param>
        public static Encoding GetEncoding(string name) => Encoding.GetEncoding(name);

        /// <summary>
        /// 根据文件路径，判断文件编码
        /// </summary>
        public static Encoding GuessFileEncoding(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new Exception("文件 " + filename + " 不存在!");
            }
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                return GuessStreamEncoding(fs);
            }
        }

        /// <summary>
        /// 根据文件流，判断文件编码
        /// </summary>
        public static Encoding GuessStreamEncoding(Stream stream)
        {
            if (!stream.CanRead)
            {
                return null;
            }
            using (var br = new BinaryReader(stream))
            {
                var buffer = br.ReadBytes(3);
                if (buffer[0] == 0xFE && buffer[1] == 0xFF)//FE FF 254 255  UTF-16 BE (big-endian)
                {
                    return Encoding.BigEndianUnicode;
                }

                if (buffer[0] == 0xFF && buffer[1] == 0xFE)//FF FE 255 254  UTF-16 LE (little-endian)
                {
                    return Encoding.Unicode;
                }
                if (buffer[0] == 0xEF && buffer[1] == 0xBB & buffer[2] == 0xBF)//EF BB BF 239 187 191 UTF-8
                {
                    return Encoding.UTF8;//with BOM
                }

                if (IsUtf8WithoutBom(stream))
                {
                    return Encoding.UTF8;//without BOM
                }
                if (IsPlainASCII(stream))
                {
                    return Encoding.ASCII; //默认返回ascii编码
                }
                return GBK;
            }
        }

        /// <summary>
        /// 根据文件路径，判断文件是否为纯ASCII
        /// </summary>
        public static bool IsPlainASCII(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return IsPlainASCII(fs);
            }
        }

        /// <summary>
        /// 根据文件流，判断文件是否为纯ASCII
        /// </summary>
        public static bool IsPlainASCII(Stream stream)
        {
            bool isAllASCII = true;
            long totalLength = stream.Length;
            stream.Seek(0, SeekOrigin.Begin);//重置 position 位置
            using (var br = new BinaryReader(stream, Encoding.Default, true))
            {
                for (long i = 0; i < totalLength; i++)
                {
                    byte b = br.ReadByte();
                    /*
                     * 原理是
                     * 0x80     1000 0000
                     * &
                     * 0x75 (p) 0111 0101
                     * ASCII字符都比128小，与运算自然都是0
                     */
                    if ((b & 0x80) != 0)// (1000 0000): 值小于0x80的为ASCII字符
                    {
                        isAllASCII = false;
                        break;
                    }
                }
            }
            return isAllASCII;
        }

        private static bool IsUtf8WithoutBom(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);//重置 position 位置
            bool isAllASCII = true;
            long totalLength = stream.Length;
            long nBytes = 0;
            using (var br = new BinaryReader(stream, Encoding.Default, true))
            {
                for (long i = 0; i < totalLength; i++)
                {
                    byte b = br.ReadByte();
                    // (1000 0000): 值小于0x80的为ASCII字符
                    // 等同于 if(b < 0x80 )
                    if ((b & 0x80) != 0) //0x80 128
                    {
                        isAllASCII = false;
                    }
                    if (nBytes == 0)
                    {
                        if (b >= 0x80)
                        {
                            if (b >= 0xFC && b <= 0xFD) { nBytes = 6; }//此范围内为6字节UTF-8字符
                            else if (b >= 0xF8) { nBytes = 5; }// 此范围内为5字节UTF-8字符
                            else if (b >= 0xF0) { nBytes = 4; }// 此范围内为4字节UTF-8字符
                            else if (b >= 0xE0) { nBytes = 3; }// 此范围内为3字节UTF-8字符
                            else if (b >= 0xC0) { nBytes = 2; }// 此范围内为2字节UTF-8字符
                            else { return false; }
                            nBytes--;
                        }
                    }
                    else
                    {
                        if ((b & 0xC0) != 0x80) { return false; }//0xc0 192  (11000000): 值介于0x80与0xC0之间的为无效UTF-8字符
                        nBytes--;
                    }
                }
            }
            if (nBytes > 0)
            {
                return false;
            }
            if (isAllASCII)
            {
                return false;
            }
            return true;
        }

        private static string ReadFile(string path, Encoding encoding)
        {
            using (StreamReader sr = new StreamReader(path, encoding, true))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// 根据文件路径，判断文件是GBK还是UTF8编码
        /// </summary>
        public static Encoding IsGBKOrUTF8(string fileName)
        {
            var utf8Str = ReadFile(fileName, UTF8);
            var gbkStr = ReadFile(fileName, GBK);
            return utf8Str.Length <= gbkStr.Length ? UTF8 : GBK;
        }
    }
}