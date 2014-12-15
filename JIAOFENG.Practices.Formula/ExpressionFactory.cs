using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JIAOFENG.Practices.Formula
{
    internal class ExpressionFactory
    {
        internal static Dictionary<string, FunctionExtension> FunctionExtension { get; set; }
        public static List<Expression> LexicalAnalysis(string strExpression)
        {
            List<Expression> listExpression = new List<Expression>();
            for (int indexTemp = 0; indexTemp < strExpression.Length; indexTemp++)
            {
                char chrTemp = strExpression[indexTemp];
                if (chrTemp == ' ')
                {
                    continue;//如果是空格，则忽略空格指针向后移动
                }
                Expression expression = null;
                //关键字和变量,以及函数处理
                if (char.IsLetter(chrTemp))
                {
                    expression = VarExpressionFactory.LexicalAnalysis(strExpression, ref indexTemp);
                }
                else if (chrTemp.Equals('\'') || chrTemp.Equals('"'))//字符串
                {
                    expression = StringExpressionFactory.LexicalAnalysis(strExpression, ref indexTemp);
                }
                else if (char.IsDigit(chrTemp))//数值
                {
                    expression = NumberExpressionFactory.LexicalAnalysis(strExpression, ref indexTemp);
                }
                else if (SymbolExpressionFactory.DictionarySymbol.ContainsKey(chrTemp.ToString()))
                {
                    expression = SymbolExpressionFactory.LexicalAnalysis(strExpression, ref indexTemp);
                }
                else 
                {
                    throw new SyntaxException(indexTemp, 1, "包含非法字符：[" + chrTemp.ToString() + "]");
                }
                listExpression.Add(expression);
            }
            return listExpression;
        }

        /// <summary>
        /// 获取操作公式字典
        /// </summary>
        /// <param name="OperateExpressionType">操作公式类型</param>
        /// <returns>操作公式字典</returns>
        internal static Dictionary<string, object> GetOperateExpressionDictionary(OperateTypeEnum operateExpressionType)
        {
            Dictionary<string, object> myDictionary = GetOperateExpressionDictionaryFromConfig(operateExpressionType);
            if (FunctionExtension != null && operateExpressionType == OperateTypeEnum.ExpressionKeyword)
            {
                foreach (KeyValuePair<string, FunctionExtension> p in FunctionExtension)
                {
                    myDictionary.Add(p.Key, p.Value);
                }
            }
            return myDictionary;
        }
        internal static Dictionary<string, object> GetOperateExpressionDictionaryFromConfig(OperateTypeEnum operateExpressionType)
        {
            Dictionary<string, object> myDictionary = new Dictionary<string, object>();
            if (myDictionary.Count == 0)
            {
                XmlDocument myDoc = new XmlDocument();
                System.IO.Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("JIAOFENG.Practices.Formula.Expression.xml");
                myDoc.Load(s);
                XmlNodeList KeywordList = myDoc.SelectNodes(string.Format("Expression/{0}/Expression",
                    Enum.GetName(typeof(OperateTypeEnum), operateExpressionType)));
                foreach (XmlNode Node in KeywordList)
                {
                    myDictionary.Add(Node.Attributes["Word"].Value, Node.Attributes["Class"].Value);
                }
            }
            return myDictionary;
        }
    }
}
