using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using JIAOFENG.Practices.Library.Common;

namespace JIAOFENG.Practices.Formula
{
    /// <summary>
    /// 计算器入口程序
    /// </summary>
    public class Calculator
    {
        private ExpressionPreprocessor preprocessor = new ExpressionPreprocessor();
        public Calculator()
        {
        }
        public Calculator(Dictionary<string, FunctionExtension> functionExtension)
        {
            ExpressionFactory.FunctionExtension = functionExtension;
        }

        static Calculator()
        {
            Cache = new Dictionary<string, Expression>();
        }

        #region Verity
        public bool Verify(string strExpression)
        {
            string errorMessage;
            string stackTrace;
            return Verify(strExpression, out errorMessage, out stackTrace);
        }
        public bool Verify(string strExpression, out string errorMessage, out string stackTrace)
        {
            List<Expression> expressionList;
            return Verify(strExpression, out expressionList, out errorMessage, out stackTrace);
        }
        public bool Verify(string strExpression, out List<Expression> expressionList, out string errorMessage, out string stackTrace)
        {
            errorMessage = string.Empty;
            stackTrace = string.Empty;
            expressionList = null;
            //表达式预处理
            if (!preprocessor.Process(ref strExpression))
            {
                errorMessage = "An error occurred on business logic configuration. Please contact Controlling team. The error details:  Expression is incorrect.";
                stackTrace = "";
                return false;
            }

            try
            {
                expressionList = ExpressionFactory.LexicalAnalysis(strExpression);
                if (expressionList.FindAll(e => e.ExpressionName == "(").Count != expressionList.FindAll(e => e.ExpressionName == ")").Count)
                {
                    errorMessage = "An error occurred on business logic configuration. Please contact Controlling team. The error details:  Mismatched brackets.";
                    stackTrace = "";
                    return false;
                }
                return true;
            }
            catch(SyntaxException ex)
            {
                errorMessage = ex.Message;
                stackTrace = ex.StackTrace;
                return false;
            }           
        }
        #endregion

        #region Run
        public string Run(string strExpression)
        {
            DataTable dt = null;
            return Run(strExpression, dt);
        }
        public string Run(string strExpression, Dictionary<string, string> vars)
        {
            string errorMessage;
            string stackTrace;
            return Run(strExpression, vars, null, out errorMessage, out stackTrace);
        }
        public string Run(string strExpression, Dictionary<string, string> vars, Dictionary<string, VarExtension> varDelegates, out string errorMessage, out string stackTrace)
        {
            DataTable dt = new DataTable();
            #region column
            if (vars != null)
            {
                foreach (string key in vars.Keys)
                {
                    dt.Columns.Add(new DataColumn(key, typeof(string)));                 
                }
            }
            if (varDelegates != null)
            {
                foreach (string key in varDelegates.Keys)
                {
                    if (!vars.ContainsKey(key))
                    {
                        dt.Columns.Add(new DataColumn(key, typeof(VarExtension)));
                    }   
                }
            }
            #endregion

            #region row
            if (vars != null || varDelegates != null)
            {
                DataRow dr = dt.NewRow();
                if (vars != null)
                {
                    foreach (string key in vars.Keys)
                    {
                        dr[key] = vars[key];
                    }
                }
                if (varDelegates != null)
                {
                    foreach (string key in varDelegates.Keys)
                    {
                        if (!vars.ContainsKey(key))
                        {
                            dr[key] = varDelegates[key];
                        }
                    }
                }
                dt.Rows.Add(dr);
            }    
            #endregion
            return Run(strExpression, dt, out errorMessage, out stackTrace);
        }
        public string Run(string strExpression, DataTable vars)
        {
            string errorMessage;
            string stackTrace;
            return Run(strExpression, vars, out errorMessage, out stackTrace);
        }
        private static Dictionary<string, Expression> Cache;
        public static Expression GetExpression(string key)
        {
            lock (Cache)
            {
                if (Cache.ContainsKey(key))
                {
                    return Cache[key];
                }
                else
                {
                    return null;
                }
            }           
        }
        public string Run(string strExpression, DataTable vars, out string errorMessage, out string stackTrace)
        {
            List<Expression> expressionList;
            errorMessage = string.Empty;
            stackTrace = string.Empty;
            Expression expression = GetExpression(strExpression);
            if (expression == null)
            {
                if (Verify(strExpression,  out expressionList,out errorMessage, out stackTrace))
                {
                    expression = Analyse(expressionList);
                    lock (Cache)
                    {
                        Cache.Add(strExpression, expression);
                        if (Cache.Count > 24)
                        {
                            Dictionary<string, Expression>.KeyCollection.Enumerator e = Cache.Keys.GetEnumerator();
                            e.MoveNext();
                            Cache.Remove(e.Current);
                        }
                    }                   
                }
                else
                {
                    return "!Error:" + errorMessage;
                }
            }

            try
            {
                expression.Erpreter(vars);             
            }
            catch (DataLostException ex)
            {
                errorMessage = string.Format("无法找到对象Code:{0}", ex.DataCode);
                stackTrace = ex.StackTrace;
                return "!Error:" + errorMessage;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                stackTrace = ex.StackTrace;
                return "!Error:" + errorMessage;
            }
            return expression.ChangeToString();
        }
        #endregion

        #region private method
        /// <summary>
        /// 分析语句并返回公式记录对象
        /// </summary>
        /// <param name="strExpression">运算表达式</param>
        /// <returns>顶级Expression对象</returns>
        internal Expression Analyse(List<Expression> listExpression)
        {       
            //语法树分析，将Expression列表按优先级转换为树
            Expression expressionTop = SyntaxTreeAnalyse.SyntaxTreeGetTopExpressionAnalyse(listExpression, 0, listExpression.Count - 1);
            return expressionTop;
        }
        #endregion
    }
}
