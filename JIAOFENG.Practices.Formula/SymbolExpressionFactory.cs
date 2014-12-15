using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    /// <summary>
    /// 运算符工厂
    /// </summary>
    
    class SymbolExpressionFactory
    {
        private static Dictionary<string, object> dictionarySymbol = new Dictionary<string, object>();
        public static Dictionary<string, object> DictionarySymbol
        {
            get
            {
                return dictionarySymbol;
            }
        }
        static SymbolExpressionFactory()
        {
            dictionarySymbol = ExpressionFactory.GetOperateExpressionDictionary(OperateTypeEnum.ExpressionSymbol);
        }

        /// <summary>
        /// 词法分析
        /// </summary>
        /// <param name="expressionList">公式对象列表</param>
        /// <param name="strExpression">源表达式</param>
        /// <param name="index">分析序号</param>       
        public static Expression LexicalAnalysis(string strExpression, ref int index)
        {
            string strTempWord;
            if ((index < strExpression.Length - 1) && dictionarySymbol.ContainsKey(strExpression.Substring(index, 2)))//如果是双字节操作符
            {
                strTempWord = strExpression.Substring(index, 2);
                index += 1;//指针后移一位
            }
            else
            {
                strTempWord = strExpression.Substring(index, 1);
            }

            Expression expression = ProduceExpression(strTempWord, index);
            return expression;
        }

        /// <summary>
        /// 产生公式对象
        /// </summary>
        /// <param name="expressionWord">分析得到的单词</param>
        /// <param name="index">当前序号</param>
        /// <returns>公式对象</returns>
        
        protected static Expression ProduceExpression(string expressionWord, int Index)
        {
            Expression expression;

            if (dictionarySymbol.ContainsKey(expressionWord.ToLower()))
            {
                string strFullClassName = "JIAOFENG.Practices.Formula." + dictionarySymbol[expressionWord.ToLower()];
                Type ExpressionType = Type.GetType(strFullClassName);
                expression = (Expression)Activator.CreateInstance(ExpressionType, new object[] { Index, expressionWord.Length });
            }
            else
            {
                //错误字符串，抛出错误，语法错误
                throw new SyntaxException(Index, expressionWord.Length, "未知表达式：" + expressionWord);
            }
            expression.ExpressionName = expressionWord;
            return expression;
        }
    }
}
