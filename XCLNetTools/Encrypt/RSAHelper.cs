using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Text;

namespace XCLNetTools.Encrypt
{
    public static class RSAHelper
    {
        /// <summary>
        /// 载入公钥PEM（支持“BEGIN PUBLIC KEY”）
        /// </summary>
        private static AsymmetricKeyParameter LoadPublicKey(string pem)
        {
            try
            {
                using (var sr = new StringReader(pem))
                {
                    var pr = new PemReader(sr);
                    var obj = pr.ReadObject();
                    return (AsymmetricKeyParameter)obj;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 载入私钥PEM（支持PKCS8格式“BEGIN PRIVATE KEY”或PKCS1格式“BEGIN RSA PRIVATE KEY”）
        /// </summary>
        private static AsymmetricKeyParameter LoadPrivateKey(string pem)
        {
            try
            {
                using (var sr = new StringReader(pem))
                {
                    var pr = new PemReader(sr);
                    var obj = pr.ReadObject();
                    var pair = obj as AsymmetricCipherKeyPair;
                    if (pair != null)
                        return pair.Private;
                    return (AsymmetricKeyParameter)obj;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 创建RSA密钥对并返回(公钥PEM, 私钥PEM)元组
        /// </summary>
        public static (string publicPEM, string privatePEM) GeneratePemPair(int keySize)
        {
            try
            {
                // 1. 生成密钥对
                var keyGen = new RsaKeyPairGenerator();
                keyGen.Init(new KeyGenerationParameters(new SecureRandom(), keySize));
                AsymmetricCipherKeyPair keyPair = keyGen.GenerateKeyPair();

                // 2. 导出私钥（PKCS8格式）
                string privateKeyPem;
                using (var sw = new StringWriter())
                {
                    var pemWriter = new PemWriter(sw);
                    pemWriter.WriteObject(keyPair.Private);
                    pemWriter.Writer.Flush();
                    privateKeyPem = sw.ToString();
                }

                // 3. 导出公钥（X.509格式）
                string publicKeyPem;
                using (var sw = new StringWriter())
                {
                    var pemWriter = new PemWriter(sw);
                    pemWriter.WriteObject(keyPair.Public);
                    pemWriter.Writer.Flush();
                    publicKeyPem = sw.ToString();
                }

                return (publicKeyPem, privateKeyPem);
            }
            catch
            {
                return (string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// 用公钥PEM字符串加密文本，返回Base64密文
        /// </summary>
        public static string Encrypt(string plainText, string publicKeyPem)
        {
            try
            {
                var pubKey = LoadPublicKey(publicKeyPem);
                var engine = new Pkcs1Encoding(new RsaEngine());
                engine.Init(true, pubKey);
                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                var cipherBytes = engine.ProcessBlock(plainBytes, 0, plainBytes.Length);
                return Convert.ToBase64String(cipherBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 用私钥PEM字符串解密Base64密文，返回原文
        /// </summary>
        public static string Decrypt(string cipherBase64, string privateKeyPem)
        {
            try
            {
                var priKey = LoadPrivateKey(privateKeyPem);
                var engine = new Pkcs1Encoding(new RsaEngine());
                engine.Init(false, priKey);
                var cipherBytes = Convert.FromBase64String(cipherBase64);
                var plainBytes = engine.ProcessBlock(cipherBytes, 0, cipherBytes.Length);
                return Encoding.UTF8.GetString(plainBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 用私钥PEM对明文签名（SHA256withRSA），返回Base64签名
        /// </summary>
        public static string Sign(string plainText, string privateKeyPem)
        {
            try
            {
                var priKey = LoadPrivateKey(privateKeyPem);
                var signer = new RsaDigestSigner(new Org.BouncyCastle.Crypto.Digests.Sha256Digest());
                signer.Init(true, priKey);
                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                signer.BlockUpdate(plainBytes, 0, plainBytes.Length);
                var signature = signer.GenerateSignature();
                return Convert.ToBase64String(signature);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 用公钥PEM验证明文和Base64签名（SHA256withRSA），返回true/false
        /// </summary>
        public static bool Verify(string plainText, string base64Signature, string publicKeyPem)
        {
            try
            {
                var pubKey = LoadPublicKey(publicKeyPem);
                var signer = new RsaDigestSigner(new Org.BouncyCastle.Crypto.Digests.Sha256Digest());
                signer.Init(false, pubKey);
                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                signer.BlockUpdate(plainBytes, 0, plainBytes.Length);
                var signature = Convert.FromBase64String(base64Signature);
                return signer.VerifySignature(signature);
            }
            catch
            {
                return false;
            }
        }
    }
}