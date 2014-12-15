using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    /// <summary>
    /// 数值工厂
    /// </summary>   
    internal class NumberExpressionFactory
    {
        /// <summary>
        /// 词法分析
        /// </summary>
        /// <param name="expressionList">公式对象列表</param>
        /// <param name="strExpression">源表达式</param>
        /// <param name="index">分析序号</param>     
        public static Expression LexicalAnalysis(string strExpression, ref int index)
        {
            int intIndexCurrent = index;//指向后一个字符
            bool blnContinue = true;
            char chrTemp;
            string strTempWord;

            while (blnContinue && intIndexCurrent < strExpression.Length)
            {
                chrTemp = strExpression[intIndexCurrent];
                if (char.IsDigit(chrTemp) || chrTemp.Equals('.') )
                {
                    intIndexCurrent += 1;
                }
                else
                {
                    blnContinue = false;
                }
            }         
            strTempWord = strExpression.Substring(index, intIndexCurrent - index);//取得临时字符串
            Expression expression = ProduceExpression(strTempWord, index);
            index = intIndexCurrent - 1;//指针移到当前指针的前一位，然后赋值给循环指针
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
            double dblValue;
            Expression expression;
            if (double.TryParse(expressionWord, out dblValue))
            {
                expression = new ValueExpression(index + 1, expressionWord.Length); 
                expression.ValueType = typeof(double);
                expression.Value = dblValue;
            }
            else
            {
                throw new SyntaxException(index, expressionWord.Length, "表达式 " + expressionWord + " 无法转换成数值。");
            }
            expression.ExpressionName = expressionWord;
            return expression;
        }
    }
}
