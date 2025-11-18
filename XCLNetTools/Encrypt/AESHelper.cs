using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Text;

namespace XCLNetTools.Encrypt
{
    public static class AESHelper
    {
        /// <summary>
        /// 生成随机密钥和IV（Base64字符串，适合AES-128/192/256）
        /// </summary>
        /// <param name="keySize">密钥长度（字节），如16/24/32</param>
        /// <returns>(base64Key, base64IV)</returns>
        public static (string base64Key, string base64IV) GenerateKeyIV(int keySize)
        {
            // keySize只能是16（128位）、24（192位）、32（256位）
            byte[] key = new byte[keySize];
            byte[] iv = new byte[16]; // AES 块大小永远是16字节
            var random = new SecureRandom();
            random.NextBytes(key);
            random.NextBytes(iv);
            return (Convert.ToBase64String(key), Convert.ToBase64String(iv));
        }

        /// <summary>
        /// AES-CBC-PKCS7 加密字符串，返回Base64密文
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="base64Key">Base64密钥</param>
        /// <param name="base64IV">Base64 IV</param>
        /// <returns>Base64密文</returns>
        public static string Encrypt(string plainText, string base64Key, string base64IV)
        {
            try
            {
                byte[] key = Convert.FromBase64String(base64Key);
                byte[] iv = Convert.FromBase64String(base64IV);

                var cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()), new Pkcs7Padding());
                cipher.Init(true, new ParametersWithIV(new KeyParameter(key), iv));
                byte[] input = Encoding.UTF8.GetBytes(plainText);
                byte[] output = new byte[cipher.GetOutputSize(input.Length)];
                int len = cipher.ProcessBytes(input, 0, input.Length, output, 0);
                len += cipher.DoFinal(output, len);
                Array.Resize(ref output, len);
                return Convert.ToBase64String(output);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// AES-CBC-PKCS7 解密Base64密文，返回原文字符串
        /// </summary>
        /// <param name="cipherBase64">Base64密文</param>
        /// <param name="base64Key">Base64密钥</param>
        /// <param name="base64IV">Base64 IV</param>
        /// <returns>明文字符串</returns>
        public static string Decrypt(string cipherBase64, string base64Key, string base64IV)
        {
            try
            {
                byte[] key = Convert.FromBase64String(base64Key);
                byte[] iv = Convert.FromBase64String(base64IV);

                var cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()), new Pkcs7Padding());
                cipher.Init(false, new ParametersWithIV(new KeyParameter(key), iv));
                byte[] input = Convert.FromBase64String(cipherBase64);
                byte[] output = new byte[cipher.GetOutputSize(input.Length)];
                int len = cipher.ProcessBytes(input, 0, input.Length, output, 0);
                len += cipher.DoFinal(output, len);
                Array.Resize(ref output, len);
                return Encoding.UTF8.GetString(output);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}