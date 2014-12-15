using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JIAOFENG.Practices.Library.Cryptography
{
    public class DESCryptography
    {
        private const string m_defaultEncryptKey = "@dashing!";
        private static byte[] IV = new byte[] { 10, 20, 30, 40, 50, 60, 70, 80 };
        public static string Decrypt(string stringToDecrypt, string sEncryptionKey = m_defaultEncryptKey)
        {
            byte[] key = new byte[0];
            byte[] inputByteArray = new byte[stringToDecrypt.Length];
            try
            {
                key = Encoding.Unicode.GetBytes(sEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.Unicode.GetString(ms.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string Encrypt(string stringToEncrypt, string sEncryptionKey = m_defaultEncryptKey)
        {
            byte[] key = new byte[0];
            try
            {
                key = Encoding.Unicode.GetBytes(sEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.Unicode.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}

