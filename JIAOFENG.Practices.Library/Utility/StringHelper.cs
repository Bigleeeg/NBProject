using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Library.Utility
{
    public static class StringHelper
    {
        #region Generate Passowrd

        public delegate string GenerateRandomStrDelegate(int lenth);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetRandomStr()
        {
            return GetRandomStr(8);
        }

        /// <summary>
        /// 功能：根据给定的长度取得随机字符
        /// </summary>       
        public static string GetRandomStr(int passwordLen)
        {
            GenerateRandomStrDelegate[] GetRandomStrs = new GenerateRandomStrDelegate[] { new GenerateRandomStrDelegate(GetRandomLowerLetter), new GenerateRandomStrDelegate(GetRandomNumber), new GenerateRandomStrDelegate(GetRandomSpecialCharacter), new GenerateRandomStrDelegate(GetRandomUpperLetter) };
            const int fixedLenth = 4;//Lower,Number,SpecialCharacter,Upper

            passwordLen = passwordLen - fixedLenth;
            const string randomChars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string password = string.Empty;
            int randomNum;
            Random random = new Random();
            for (int i = 0; i < passwordLen; i++)
            {
                randomNum = random.Next(randomChars.Length);
                password += randomChars[randomNum];
            }
            foreach (GenerateRandomStrDelegate generateRandomStr in GetRandomStrs)
            {
                string strPassword = generateRandomStr.Invoke(1);
                password = password.Insert(random.Next(password.Length), strPassword);
            }
            return password;
        }
        public static string GetRandomSpecialCharacter(int passwordLen)
        {
            string randomChars = "#$%&-_:{}@!";
            string password = string.Empty;
            int randomNum;
            Random random = new Random();
            for (int i = 0; i < passwordLen; i++)
            {
                randomNum = random.Next(randomChars.Length);
                password += randomChars[randomNum];
            }
            return password;
        }
        public static string GetRandomNumber(int passwordLen)
        {
            string randomChars = "0123456789";
            string password = string.Empty;
            int randomNum;
            Random random = new Random();
            for (int i = 0; i < passwordLen; i++)
            {
                randomNum = random.Next(randomChars.Length);
                password += randomChars[randomNum];
            }
            return password;
        }
        public static string GetRandomLowerLetter(int passwordLen)
        {
            string randomChars = "abcdefghijklmnopqrstuvwxyz";
            string password = string.Empty;
            int randomNum;
            Random random = new Random();
            for (int i = 0; i < passwordLen; i++)
            {
                randomNum = random.Next(randomChars.Length);
                password += randomChars[randomNum];
            }
            return password;
        }
        public static string GetRandomUpperLetter(int passwordLen)
        {
            string randomChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string password = string.Empty;
            int randomNum;
            Random random = new Random();
            for (int i = 0; i < passwordLen; i++)
            {
                randomNum = random.Next(randomChars.Length);
                password += randomChars[randomNum];
            }
            return password;
        }
        #endregion

        #region DangerousHtml
        /// <summary>
        /// 过滤html中危险标签
        /// </summary>       
        /// <returns></returns>
        public static string CutDangerousHtmlElement(string content)
        {
            //System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[^>]*>[^>]*<[^>]script[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@"<iframe[^>]*>[^>]*<[^>]iframe[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@"<frameset[^>]*>[^>]*<[^>]frameset[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<img (.+?) />", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //content = regex1.Replace(content, ""); //过滤<script></script>标记              
            //content = regex2.Replace(content, ""); //过滤iframe  
            //content = regex3.Replace(content, ""); //过滤frameset 
            //content = regex4.Replace(content, "");
            //return content;
            return FilterHandler.FilterAll(content);
        }
        #endregion

        #region Cut String
        public static string StringFormat(string str, int lengthShowed)
        {
            string temp = string.Empty;
            if (System.Text.Encoding.Default.GetByteCount(str) <= lengthShowed)//如果长度比需要的长度n小,返回原字符串
            {
                return str;
            }
            else
            {
                int t = 0;
                char[] q = str.ToCharArray();
                for (int i = 0; i < q.Length && t < lengthShowed; i++)
                {
                    if ((int)q[i] >= 0x4E00 && (int)q[i] <= 0x9FA5)//是否汉字
                    {
                        temp += q[i];
                        t += 2;
                    }
                    else
                    {
                        temp += q[i];
                        t++;
                    }
                }
                return (temp + "..");//保留两个点代替之前的三个点省略号
            }
        }
        #endregion

        /// <summary>
        ///  转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 功能：将参数转成全角
        /// 时间：2013-03-25        
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 功能：汉字转拼音缩写
        /// 日期：2013-03-25         
        /// </summary>
        /// <param name="str">要转换的汉字字符串</param>
        /// <returns>拼音缩写</returns>
        public static string GetPYString(string str)
        {
            string tempStr = "";
            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {
                    //字母和符号原样保留
                    tempStr += c.ToString();
                }
                else
                {
                    //累加拼音声母
                    tempStr += GetPYChar(c.ToString());
                }
            }
            return tempStr;
        }

        /// <summary>
        /// 功能:取单个字符的拼音声母
        /// 日期：2013-03-25         
        /// </summary>
        /// <param name="c">要转换的单个汉字</param>
        /// <returns>拼音声母</returns>
        public static string GetPYChar(string c)
        {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = ((short)(array[0] - '\0')) * 256 + ((short)(array[1] - '\0'));
            if (i < 0xB0A1) return "*";

            if (i < 0xB0C5) return "a";

            if (i < 0xB2C1) return "b";

            if (i < 0xB4EE) return "c";

            if (i < 0xB6EA) return "d";

            if (i < 0xB7A2) return "e";

            if (i < 0xB8C1) return "f";

            if (i < 0xB9FE) return "g";

            if (i < 0xBBF7) return "h";

            if (i < 0xBFA6) return "j";

            if (i < 0xC0AC) return "k";

            if (i < 0xC2E8) return "l";

            if (i < 0xC4C3) return "m";

            if (i < 0xC5B6) return "n";

            if (i < 0xC5BE) return "o";

            if (i < 0xC6DA) return "p";

            if (i < 0xC8BB) return "q";

            if (i < 0xC8F6) return "r";

            if (i < 0xCBFA) return "s";

            if (i < 0xCDDA) return "t";

            if (i < 0xCEF4) return "w";

            if (i < 0xD1B9) return "x";

            if (i < 0xD4D1) return "y";

            if (i < 0xD7FA) return "z";
            return "*";
        }

        /// <summary>
        /// 判定字符串IsNullOrEmpty
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return String.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 判定字符串IsNullOrWhiteSpace
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// 判定两个字符串是否相等
        /// </summary>
        /// <param name="str"></param>
        /// <param name="val"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool IsEqual(this string str, string val, bool ignoreCase = false)
        {
            if (str == null && val == null)
                return true;

            if (str == null || val == null)
                return false;

            return String.Compare(str, val, ignoreCase) == 0;
        }

        /// <summary>
        /// 移除尾部不需要的字符
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="trimChars"></param>
        /// <returns></returns>
        public static StringBuilder TrimEnd(this StringBuilder builder, params char[] trimChars)
        {
            if (trimChars == null)
            {
                throw new ArgumentNullException("trimChars");
            }
            while (builder.Length > 0 && trimChars.Contains(builder[builder.Length - 1]))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder;
        }

        /// <summary>
        /// 将字符串传化为Boolean值
        /// </summary>
        /// <param name="strVal">目标字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>bool值</returns>
        public static bool ToBoolean(this string strVal, bool defaultValue = false)
        {
            bool result;
            return bool.TryParse(strVal, out result) ? result : defaultValue;
        }
    }
}
