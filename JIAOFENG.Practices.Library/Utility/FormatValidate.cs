﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace JIAOFENG.Practices.Library.Utility
{
    /// <summary>
    /// 数据格式校验类，也可用于用户输入校验
    /// </summary>
    public static class FormatValidate
    {
        private static Regex RegPhone = new Regex("^[0-9]+[-]?[0-9]+[-]?[0-9]$");
        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
        private static Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");

        #region 数字字符串检查
        /// <summary>
        /// 是否数字字符串
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string inputData)
        {
            Match m = RegNumber.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumberSign(string inputData)
        {
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(string inputData)
        {
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimalSign(string inputData)
        {
            Match m = RegDecimalSign.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 中文检测

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsHasCHZN(string inputData)
        {
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 邮件地址
        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 日期格式判断
        /// <summary>
        /// 日期格式字符串判断
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(string str)
        {
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    DateTime.Parse(str);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 其他
        /// <summary>
        /// 字符串编码
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static string HtmlEncode(string inputData)
        {
            return HttpUtility.HtmlEncode(inputData);
        }
        ////字符串清理
        //public static string InputText(string inputString, int maxLength)
        //{
        //    StringBuilder retVal = new StringBuilder();

        //    // 检查是否为空
        //    if ((inputString != null) && (inputString != String.Empty))
        //    {
        //        inputString = inputString.Trim();

        //        //检查长度
        //        if (inputString.Length > maxLength)
        //            inputString = inputString.Substring(0, maxLength);

        //        //替换危险字符
        //        for (int i = 0; i < inputString.Length; i++)
        //        {
        //            switch (inputString[i])
        //            {
        //                case '"':
        //                    retVal.Append("&quot;");
        //                    break;
        //                case '<':
        //                    retVal.Append("&lt;");
        //                    break;
        //                case '>':
        //                    retVal.Append("&gt;");
        //                    break;
        //                default:
        //                    retVal.Append(inputString[i]);
        //                    break;
        //            }
        //        }
        //        retVal.Replace("'", " ");// 替换单引号
        //    }
        //    return retVal.ToString();

        //}
        /// <summary>
        /// 记普通文本-转换成-HTML code
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string Encode(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }
        /// <summary>
        ///解析html成 普通文本
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string Decode(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }

        //public static string SqlTextClear(string sqlText)
        //{
        //    if (sqlText == null)
        //    {
        //        return null;
        //    }
        //    if (sqlText == "")
        //    {
        //        return "";
        //    }
        //    sqlText = sqlText.Replace(",", "");//去除,
        //    sqlText = sqlText.Replace("<", "");//去除<
        //    sqlText = sqlText.Replace(">", "");//去除>
        //    sqlText = sqlText.Replace("--", "");//去除--
        //    sqlText = sqlText.Replace("'", "");//去除'
        //    sqlText = sqlText.Replace("\"", "");//去除"
        //    sqlText = sqlText.Replace("=", "");//去除=
        //    sqlText = sqlText.Replace("%", "");//去除%
        //    sqlText = sqlText.Replace(" ", "");//去除空格
        //    return sqlText;
        //}
        #endregion

        //#region 是否由特定字符组成
        //public static bool isContainSameChar(string strInput)
        //{
        //    string charInput = string.Empty;
        //    if (!string.IsNullOrEmpty(strInput))
        //    {
        //        charInput = strInput.Substring(0, 1);
        //    }
        //    return isContainSameChar(strInput, charInput, strInput.Length);
        //}

        //public static bool isContainSameChar(string strInput, string charInput, int lenInput)
        //{
        //    if (string.IsNullOrEmpty(charInput))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        Regex RegNumber = new Regex(string.Format("^([{0}])+$", charInput));
        //        //Regex RegNumber = new Regex(string.Format("^([{0}]{{1}})+$", charInput,lenInput));
        //        Match m = RegNumber.Match(strInput);
        //        return m.Success;
        //    }
        //}
        //#endregion

        //#region 检查输入的参数是不是某些定义好的特殊字符：这个方法目前用于密码输入的安全检查
        ///// <summary>
        ///// 检查输入的参数是不是某些定义好的特殊字符：这个方法目前用于密码输入的安全检查
        ///// </summary>
        //public static bool isContainSpecChar(string strInput)
        //{
        //    string[] list = new string[] { "123456", "654321" };
        //    bool result = new bool();
        //    for (int i = 0; i < list.Length; i++)
        //    {
        //        if (strInput == list[i])
        //        {
        //            result = true;
        //            break;
        //        }
        //    }
        //    return result;
        //}
        //#endregion
    }
}