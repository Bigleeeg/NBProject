using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    /// <summary>
    /// 语法树分析
    /// </summary>
    internal class SyntaxTreeAnalyse
    {
        /// <summary>
        /// 公式段提取，提取顶级操作公式
        /// </summary>
        /// <param name="indexStart">起始序号</param>
        /// <param name="indexEnd">结束序号</param>
        /// <returns>传入公式段的顶级操作公式记录对象</returns>        
        internal static Expression SyntaxTreeGetTopExpressionAnalyse(List<Expression> expressionList, int indexStart, int indexEnd)
        {
            int intIndexCurrent = indexStart;//当前处理序号
            int intIndexSubStart = indexStart;//子公式段的起始序号
            Expression expression;//获取当前Expression
            Stack<Expression> stackCompart = new Stack<Expression>();//括号堆栈
            Stack<Expression> stackOperate = new Stack<Expression>();//操作公式堆栈

            for (int intIndex = indexStart; intIndex <= indexEnd; intIndex++)
            {
                expression = expressionList[intIndex];
                intIndexCurrent = intIndex;
                if (expression is LeftBracketExpression)
                {
                    stackCompart.Push(expressionList[intIndexCurrent]);//把左括号压栈
                    //获取该括号对中包含的公式段
                    intIndexSubStart = intIndexCurrent + 1;//设置子公式段的起始序号
                    //提取公式段
                    //如果语法错误，比如在公式段的末尾没有配对的右括号公式，则会出错，这里假设为语法正确
                    //左右括号匹配检测放到预处理器那来做
                    while (stackCompart.Count > 0)
                    {
                        intIndexCurrent += 1;
                        if (intIndexCurrent >= expressionList.Count)
                        {
                            //Error or auto add lossed bracket
                            throw new SyntaxException(expression.Index, expression.Length, "缺少配对的括号");
                        }

                        if (expressionList[intIndexCurrent] is LeftBracketExpression)
                        {
                            stackCompart.Push(expressionList[intIndexCurrent]);
                        }
                        else if (expressionList[intIndexCurrent] is RightBracketExpression)
                        {
                            stackCompart.Pop();
                        }
                    }

                    Expression expressionInnerTop = SyntaxTreeGetTopExpressionAnalyse(expressionList, intIndexSubStart, intIndexCurrent - 1);
                    if (expressionInnerTop != null)//只有在取得括号中的顶级节点才添加
                    {
                        expression.ChildList.Add(expressionInnerTop);
                    }
                    else
                    {
                        //无法获取括号内的顶级节点
                        throw new SyntaxException(expression.Index, expression.Length, "括号内不包含计算表达式");
                    }

                    intIndex = intIndexCurrent;//移动序号到当前序号
                    SyntaxTreeStackAnalyse(stackOperate, expression);
                }//if expression is LeftBracketExpression
                else if (expression is FuncExpression)//方法处理
                {
                    //检查方法后是否接着左括号
                    if (intIndexCurrent >= indexEnd || !(expressionList[intIndexCurrent + 1] is LeftBracketExpression))
                    {
                        throw new SyntaxException(expression.Index, expression.Length, expression.ExpressionName + "：方法后缺少括号");
                    }

                    intIndexSubStart = intIndexCurrent;//设置子公式段的起始序号
                    intIndexCurrent += 1;//指针后移
                    stackCompart.Push(expressionList[intIndexCurrent]);//把左括号压栈
                    //提取公式段
                    //如果语法错误，比如在公式段的末尾没有配对的右括号公式，则会出错，这里假设为语法正确
                    while (stackCompart.Count > 0)
                    {
                        intIndexCurrent += 1;
                        if (intIndexCurrent >= expressionList.Count)
                        {
                            //Error or auto add lossed bracket
                            throw new SyntaxException(expression.Index, expression.Length, "缺少配对的括号");
                        }
                        if (expressionList[intIndexCurrent] is LeftBracketExpression)
                        {
                            stackCompart.Push(expressionList[intIndexCurrent]);
                        }
                        else if (expressionList[intIndexCurrent] is RightBracketExpression)
                        {
                            stackCompart.Pop();
                        }
                    }

                    if ((intIndexCurrent - intIndexSubStart) == 2)//如果右括号的序号和方法的序号之差是2，说明中间只有一个左括号
                    {
                        throw new SyntaxException(expressionList[intIndexCurrent - 1].Index, 1, "括号内不包含计算表达式");
                    }

                    //分析方法公式段，因为方法括号段中可能有多个运算结果，比如if，max等，不能用获取顶级节点的方法SyntaxTreeGetTopExpressionAnalyse
                    SyntaxTreeMethodAnalyse(expressionList, intIndexSubStart, intIndexCurrent);

                    intIndex = intIndexCurrent;//移动序号到当前序号
                    SyntaxTreeStackAnalyse(stackOperate, expression);
                }//if expression is ExpressionKeyword
                else if (expression is SymbolExpression || expression is ValueExpression || expression is VarExpression)
                {
                    SyntaxTreeStackAnalyse(stackOperate, expression);
                }
                else
                {
                    //不做处理
                    throw new SyntaxException(expression.Index, expression.Length, "运算符位置错误");
                }
            }
            return SyntaxTreeStackGetTopExpression(stackOperate);//返回顶级公式
        }


        /// <summary>
        /// 方法公式段分析（处于括号中间的代码段）
        /// </summary>
        /// <param name="ListExpression">公式列表</param>
        /// <param name="IndexStart">括号开始的序号</param>
        /// <param name="IndexEnd">括号结束的序号</param>
        /// <remarks>只处理完整的方法段，比如if(), round()</remarks>
        
        private static void SyntaxTreeMethodAnalyse(List<Expression> ListExpression, int IndexStart, int IndexEnd)
        {
            int intIndexSubStart = IndexStart;//子公式段的起始序号
            intIndexSubStart = IndexStart + 2;//移动子公式段的开始序号到括号后面
            int intIndexSubEnd = IndexEnd;//子公式段的结束序号
            Expression FuncExpression = ListExpression[IndexStart];//方法公式对象
            Expression ExpressionCurrent;//获取当前Expression
            Stack<Expression> StackCompart = new Stack<Expression>();//分隔符堆栈

            for (int intIndex = IndexStart; intIndex <= IndexEnd; intIndex++)
            {
                ExpressionCurrent = ListExpression[intIndex];
                if (ExpressionCurrent is LeftBracketExpression)
                {
                    StackCompart.Push(ExpressionCurrent);
                }
                else if (ExpressionCurrent is RightBracketExpression)
                {
                    StackCompart.Pop();
                    if (StackCompart.Count == 0)//如果是最后一个右括号
                    {
                        intIndexSubEnd = intIndex - 1;//设置段结束序号
                        FuncExpression.ChildList.Add(SyntaxTreeGetTopExpressionAnalyse(ListExpression, intIndexSubStart, intIndexSubEnd));//递归
                    }
                }
                else if (ExpressionCurrent is CommaExpression)
                {
                    if (StackCompart.Count == 1)//如果是方法的子段
                    {
                        intIndexSubEnd = intIndex - 1;//设置段结束序号
                        FuncExpression.ChildList.Add(SyntaxTreeGetTopExpressionAnalyse(ListExpression, intIndexSubStart, intIndexSubEnd));//递归
                        intIndexSubStart = intIndex + 1;//把子公式段序号后移
                    }
                }
                else
                {
                    //不作处理
                }
            }
        }

        /// <summary>
        /// 语法树堆栈分析，基于公式的优先级
        /// </summary>
        /// <param name="syntaxTreeStack">语法树堆栈</param>
        /// <param name="newExpression">新公式</param>       
        private static void SyntaxTreeStackAnalyse(Stack<Expression> syntaxTreeStack, Expression newExpression)
        {
            if (syntaxTreeStack.Count == 0)//如果语法树堆栈中不存在公式，则直接压栈
            {
                syntaxTreeStack.Push(newExpression);
            }
            else//否则，比较优先级进行栈操作
            {
                //比较优先级，如果新Expression优先级高，则压栈；
                //如果新Expression优先级低，则弹栈，把弹出的Expression设置为新Expression的下级，并把新Expression压栈；
                //相同优先级也弹栈，并将新Expression压栈
                //if (this.m_DicExpressionTypePriority[SyntaxTreeStack.Peek().ExpressionType] < this.m_DicExpressionTypePriority[newExpression.ExpressionType])//低进
                if (syntaxTreeStack.Peek().Priority < newExpression.Priority)//低进
                {
                    syntaxTreeStack.Push(newExpression);//低进
                }
                else
                {
                    Expression TempExpression = null;
                    //如果堆栈中有公式，并且栈顶的公式优先级大于等于新公式的优先级，则继续弹栈
                    while (syntaxTreeStack.Count > 0 && (syntaxTreeStack.Peek().Priority >= newExpression.Priority))
                    {
                        TempExpression = syntaxTreeStack.Pop();
                        if (syntaxTreeStack.Count > 0)//检测栈顶是否可能为空，如果为空则退出
                        {
                            if (syntaxTreeStack.Peek().Priority >= newExpression.Priority)
                            {
                                syntaxTreeStack.Peek().ChildList.Add(TempExpression);
                            }
                            else
                            {
                                newExpression.ChildList.Add(TempExpression);
                            }
                        }
                        else
                        {
                            newExpression.ChildList.Add(TempExpression);
                        }
                    }
                    syntaxTreeStack.Push(newExpression);//压栈
                }
            }
        }

        /// <summary>
        /// 获取语法树堆栈的顶级公式
        /// </summary>
        /// <param name="syntaxTreeStack">语法树堆栈</param>
        /// <returns>顶级公式</returns>       
        private static Expression SyntaxTreeStackGetTopExpression(Stack<Expression> syntaxTreeStack)
        {
            Expression tempExpression = null;
            if (syntaxTreeStack.Count > 0)
            {
                tempExpression = syntaxTreeStack.Pop();
                while (syntaxTreeStack.Count > 0)
                {
                    syntaxTreeStack.Peek().ChildList.Add(tempExpression);
                    tempExpression = syntaxTreeStack.Pop();
                }
            }
            return tempExpression;
        }
    }
}
