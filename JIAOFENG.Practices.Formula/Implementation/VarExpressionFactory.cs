using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    /// <summary>
    /// 变量/关键字工厂
    /// </summary>
    internal class VarExpressionFactory
    {
        private static Dictionary<string, object> dictionaryKeyword;
        static VarExpressionFactory()
        {
            dictionaryKeyword = ExpressionFactory.GetOperateExpressionDictionary(OperateTypeEnum.ExpressionKeyword);
        }
        /// <summary>
        /// 词法分析
        /// </summary>
        /// <param name="expressionList">公式对象列表</param>
        /// <param name="strExpression">源表达式</param>
        /// <param name="index">分析序号</param>      
        public static Expression LexicalAnalysis(string strExpression, ref int index)
        {
            int intIndexCurrent = index;//当前序号
            bool blnContinue = true;
            char chrTemp;
            string strTempWord = "";//获取临时字符串

            while (blnContinue && intIndexCurrent < strExpression.Length)
            {
                chrTemp = strExpression[intIndexCurrent];
                //关键字支持大小写字母及数字，但不允许以数字开头,hft[2014/04/22]增加下划线
                if (char.IsLetter(chrTemp) || char.IsDigit(chrTemp) || chrTemp == '_')
                {
                    intIndexCurrent += 1;
                }
                else
                {
                    blnContinue = false;
                }
            }            
            strTempWord = strExpression.Substring(index, intIndexCurrent - index);//获取临时词
            Expression expression = ProduceExpression(strTempWord, index);
            index = intIndexCurrent - 1;//设置指针到读取结束位置
            return expression;           
        }

        /// <summary>
        /// 产生公式对象
        /// </summary>
        /// <param name="expressionWord">分析得到的单词</param>
        /// <param name="index">当前序号</param>
        /// <returns>公式对象</returns>
        
        protected static Expression ProduceExpression(string expressionWord, int index)
        {
            Expression expression;
            expressionWord = expressionWord.Trim();
            if (dictionaryKeyword.ContainsKey(expressionWord.ToLower()))
            {
                if (dictionaryKeyword[expressionWord.ToLower()] is Delegate)
                {
                    string strFullClassName = "JIAOFENG.Practices.Formula.DelegateExpression";
                    Type expressionType = Type.GetType(strFullClassName);
                    expression = (Expression)Activator.CreateInstance(expressionType, new object[] { index, expressionWord.Length, (FunctionExtension)dictionaryKeyword[expressionWord.ToLower()] });
                }
                else
                {
                    string strFullClassName = "JIAOFENG.Practices.Formula." + dictionaryKeyword[expressionWord.ToLower()];
                    Type expressionType = Type.GetType(strFullClassName);
                    expression = (Expression)Activator.CreateInstance(expressionType, new object[] { index, expressionWord.Length });
                    //对常数的特殊处理
                    switch (expressionWord.ToLower())
                    {
                        case "pi"://以下为常量公式对象
                            expression.ValueType = typeof(double);
                            expression.Value = Math.PI;
                            break;
                        case "true":
                            expression.ValueType = typeof(bool);
                            expression.Value = true;
                            break;
                        case "false":
                            expression.ValueType = typeof(bool);
                            expression.Value = false;
                            break;
                        default:
                            break;
                    }
                }  
            }
            else
            {
                expression = new VarExpression(index, expressionWord.Length, expressionWord);
            }
            expression.ExpressionName = expressionWord;
            return expression;
        }
    }
}
