using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal class ExpressionPreprocessor
    {
        public bool Process(ref string strExpression)
        {
            //check empty string
            if (string.IsNullOrWhiteSpace(strExpression))
            {
                return false;
            }

            //remove space
            //strExpression = strExpression.Replace(@" ", string.Empty);如果是包含空格的字符串呢？
            strExpression = strExpression.Trim();
            //verify "()"
            //int length = 0;
            //char[] charArray = strExpression.ToCharArray();
            //foreach (char c in charArray)
            //{
            //    if (c == '(')
            //    {
            //        length++;
            //    }
            //    else if (c == ')')
            //    {
            //        length--;
            //    }
            //    if (length < 0)
            //    {
            //        return false;
            //    }
            //}
            //if (length != 0)
            //{
            //    return false;
            //}

            //公式的起始关键字=
            if (strExpression.StartsWith("="))
            {
                strExpression = strExpression.Substring(1);
            }

            //other process
            //...

            return true;
        }
    }
}
