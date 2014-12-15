using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace JIAOFENG.Practices.Library.Cryptography
{
    public class SymmetricCryptography
    {
        private static byte[] defaultKey = new Byte[32] 
        { 
            0xAD,   0xE0,   0xB9,   0xF4, 
            0xF5,   0xA7,   0x90,   0xB0, 
            0x6B,   0x9F,   0xAC,   0x37, 
            0xB5,   0x7C,   0x76,   0xAB, 
            0x30,   0xB7,   0xB9,   0x29, 
            0x25,   0xD1,   0xBD,   0xC2, 
            0xFE,   0xB6,   0x8F,   0xB3, 
            0xF0,   0x2D,   0x22,   0x9D 
        };
        private static byte[] defaultIV = new Byte[16] 
        { 
            0xCD,   0x21,   0x9B,   0x79, 
            0xF7,   0xCB,   0x59,   0x88, 
            0xAD,   0xE4,   0xCE,   0x7D, 
            0x17,   0xEA,   0x23,   0xCA 
        };

        private byte[] key;
        private byte[] iv;
        public SymmetricCryptography():this(defaultKey, defaultIV)
        { 
        }
        public SymmetricCryptography(byte[] key, byte[] iv)
        {
            this.key = key;
            this.iv = iv;
        }
        public string UnprotectString(string value)
        {
            byte[] bytes = Convert.FromBase64String(value);
            MemoryStream ms = new MemoryStream(bytes);
            SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();
            CryptoStream encStream = new CryptoStream(ms, rijn.CreateDecryptor(key, iv), CryptoStreamMode.Read);
            int len = encStream.ReadByte();
            byte[] buffer = new byte[len];
            encStream.Read(buffer, 0, len);
            encStream.Close();
            UTF8Encoding converterUnicode = new UTF8Encoding();
            return converterUnicode.GetString(buffer);
        }

        public string ProtectString(string value)
        {
            UTF8Encoding converterUnicode = new UTF8Encoding();
            MemoryStream ms = new MemoryStream();
            SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();
            CryptoStream encStream = new CryptoStream(ms, rijn.CreateEncryptor(key, iv), CryptoStreamMode.Write);
            byte[] buffer = converterUnicode.GetBytes(value);
            encStream.WriteByte((byte)buffer.Length);
            encStream.Write(buffer, 0, buffer.Length);
            encStream.Close();
            byte[] bytes = ms.ToArray();
            return Convert.ToBase64String(bytes);
        }
    }
}
