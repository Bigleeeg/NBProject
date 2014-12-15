using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    /// <summary>
    /// 字符串工厂
    /// </summary>  
    internal class StringExpressionFactory
    {
        /// <summary>
        /// 词法分析
        /// </summary>
        /// <param name="expressionList">公式对象列表</param>
        /// <param name="strExpression">源表达式</param>
        /// <param name="index">分析序号</param>       
        public static Expression LexicalAnalysis(string strExpression, ref int index)
        {
            string strQuotationMark = strExpression.Substring(index, 1);//引号，可以是单引号，也可以是双引号
            int intIndexCurrent = index + 1;//指向后一个字符
            string strTempChar = "";//临时字符
            StringBuilder strTempWord = new StringBuilder();//临时字符串
            bool blnContinue = true;

            while (blnContinue && intIndexCurrent < strExpression.Length)//循环直到标志位置False或超出长度
            {
                strTempChar = strExpression.Substring(intIndexCurrent, 1);

                if (strTempChar.Equals(strQuotationMark))//如果是字符串分隔符
                {
                    if ((intIndexCurrent < strExpression.Length - 1)
                        && strExpression.Substring(intIndexCurrent + 1, 1).Equals(strQuotationMark))//如果后面还是引号，则进行转义，将两个引号转义为一个引号字符
                    {
                        strTempWord.Append(strQuotationMark);//添加一个引号字符到临时字符串中
                        intIndexCurrent += 2;//向后移两位
                    }
                    else
                    {
                        blnContinue = false;//遇到字符串结束引号，退出
                    }
                }
                else
                {
                    strTempWord.Append(strTempChar);//添加当前字符到临时字符串中
                    intIndexCurrent += 1;//向后移一位
                }
            }           
            Expression expression = ProduceExpression(strTempWord.ToString(), index);
            index = intIndexCurrent;//指针移到当前指针，即字符串结束引号
            return expression;          
        }

        /// <summary>
        /// 产生公式对象
        /// </summary>
        /// <param name="expressionWord">分析得到的单词</param>
        /// <param name="index">当前序号</param>
        /// <returns>公式对象</returns>       
        public static Expression ProduceExpression(string expressionWord, int index)
        {
            Expression expression = new ValueExpression(index, expressionWord.Length);
            expression.ValueType = typeof(string);
            expression.Value = expressionWord;
            expression.ExpressionName = expressionWord;
            return expression;
        }
    }
}
