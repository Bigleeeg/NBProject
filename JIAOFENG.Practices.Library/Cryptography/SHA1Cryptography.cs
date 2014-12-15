using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Library.Cryptography
{
    public class SHA1Cryptography
    {
        public static string ProtectString(string str)
        {
            System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] s = sha1.ComputeHash(UnicodeEncoding.UTF8.GetBytes(str));
            return BitConverter.ToString(s).Replace("-", "");
        }
    }
}
