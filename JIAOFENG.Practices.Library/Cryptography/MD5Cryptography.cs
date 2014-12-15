using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace JIAOFENG.Practices.Library.Cryptography
{
    public class MD5Cryptography
    {
        public static string ProtectString(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] s = md5.ComputeHash(UnicodeEncoding.UTF8.GetBytes(str));
            return BitConverter.ToString(s).Replace("-","");
        }
    }
}
