using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public static class SecurityTest
    {
        public static string UTF8ByteToStr(byte[] input)
        {
            if (input == null) return null;
            List<byte> tmp = new List<byte>();
            foreach(byte it in input)
            {
                if (it != 0)
                {
                    tmp.Add(it);
                }
                else
                {
                    break;
                }
            }
            return Encoding.UTF8.GetString(tmp.ToArray());
        }
        public static byte[] UTF8StrToByte(string Key,int Length)
        {
            if (Key == null) return null;
            byte[] bArr = new byte[Length];
            byte[] tmp;
            tmp = Encoding.UTF8.GetBytes(Key);
            for(int i = 0; i < Length; i++)
            {
                if (i < tmp.Length) bArr[i] = tmp[i];
            }
            return bArr;
        }

        //******
        public static byte[] Sha1(byte[] bData)
        {
            try
            {
                if (bData == null) return null;
                using (System.Security.Cryptography.SHA1Managed sha=new System.Security.Cryptography.SHA1Managed())
                {
                    return sha.ComputeHash(bData);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static byte[] Sha256(byte[] bData)
        {
            try
            {
                if (bData == null) return null;
                using (System.Security.Cryptography.SHA256Managed sha = new System.Security.Cryptography.SHA256Managed())
                {
                    return sha.ComputeHash(bData);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static byte[] Md5(byte[] bData)
        {
            try
            {
                if (bData == null) return null;
                using (System.Security.Cryptography.MD5 Md = System.Security.Cryptography.MD5.Create())
                {
                    return Md.ComputeHash(bData);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static byte[] AesE(string text, ref byte[] Key, ref byte[] IV)
        {
            try
            {
                if (text == null) return null;

                using (System.Security.Cryptography.AesManaged Aes = new System.Security.Cryptography.AesManaged())
                {
                    byte[] encrypted;

                    if (Key == null)
                    {
                        Aes.GenerateKey();
                        Key = Aes.Key;
                    }
                    else
                    {
                        Aes.Key = Key;
                    }

                    if (IV == null)
                    {
                        Aes.GenerateIV();
                        IV = Aes.IV;
                    }
                    else
                    {
                        Aes.IV = IV;
                    }
                    
                    System.Security.Cryptography.ICryptoTransform encryptor = Aes.CreateEncryptor();
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                //Write all data to the stream.
                                swEncrypt.Write(text);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }

                    return encrypted;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string AesD(byte[] text, ref byte[] Key, ref byte[] IV)
        {
            try
            {
                if (text == null || Key == null || IV == null) return null;
                if (text.Length % 16 != 0 || Key.Length != 32 || IV.Length != 16) return null;
                
                using (System.Security.Cryptography.AesManaged Aes = new System.Security.Cryptography.AesManaged())
                {
                    string plaintext;
                    Aes.Key = Key;
                    Aes.IV = IV;
                    
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = Aes.CreateDecryptor(Aes.Key, Aes.IV);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(text))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return plaintext;
                }

            }
            catch (CryptographicException)
            {
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string RsaGetK()
        {
            try
            {
                using (System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    return rsa.ToXmlString(true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string RsaToPub(string xmlK)
        {
            try
            {
                using (System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(xmlK);
                    return rsa.ToXmlString(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 公鑰加密 私鑰解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="RSAParams"></param>
        /// <param name="DoOAEPPadding"></param>
        /// <returns></returns>
        public static byte[] RsaE(byte[] data, string RSAParams, bool DoOAEPPadding)
        {
            try
            {
                using (System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(RSAParams);
                    return rsa.Encrypt(data, DoOAEPPadding);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static byte[] RsaD(byte[] data, string RSAParams, bool DoOAEPPadding)
        {
            try
            {
                using (System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(RSAParams);
                    return rsa.Decrypt(data, DoOAEPPadding);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 公鑰解密 私鑰加密 
        /// </summary>
        /// <param name="hashData"></param>
        /// <param name="AlgName"></param>
        /// <param name="xmlKey"></param>
        /// <returns></returns>
        public static byte[] Signature(byte[] hashData, string AlgName, string xmlKey)
        {
            try
            {
                using (System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(xmlKey);
                    System.Security.Cryptography.RSAPKCS1SignatureFormatter SF = new RSAPKCS1SignatureFormatter(rsa);

                    SF.SetHashAlgorithm(AlgName);
                    return SF.CreateSignature(hashData);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool SignatureC(byte[] hashD, byte[] signD, string AlgName, string xmlKey)
        {
            try
            {
                using (System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(xmlKey);

                    System.Security.Cryptography.RSAPKCS1SignatureDeformatter SD = new RSAPKCS1SignatureDeformatter(rsa);

                    SD.SetHashAlgorithm(AlgName);

                    return SD.VerifySignature(hashD, signD);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
