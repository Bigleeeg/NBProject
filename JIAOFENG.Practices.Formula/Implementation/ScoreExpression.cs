using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal class ScoreExpression : FuncExpression
    {
        public ScoreExpression(int index, int length)
            : base(index, length)
        {
            this.ValueType = typeof(string);
        }
        public override void Erpreter(DataTable vars)
        {
            char[] start = new char[] {'(','['};
            char[] end = new char[] { ')', ']' };
            int indexCurrent = 0;
            int indexStart = 0;
            int indexEnd = 0;
            List<string> sections = new List<string>();

            Expression expressionFirst = this.ChildList[0];
            expressionFirst.Erpreter(vars);
            double value = expressionFirst.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: Lack of number of Parameters in Score().");

            Expression expressionSec = this.ChildList[1];
            expressionSec.Erpreter(vars);
            string expression = expressionSec.ChangeToString();
            int expressionLength = expression.Length;
            do
            {
                indexStart = expression.IndexOfAny(start, indexCurrent);
                indexCurrent = indexStart + 1;
                indexEnd = expression.IndexOfAny(end, indexCurrent);
                indexCurrent = indexEnd + 1;
                sections.Add(expression.Substring(indexStart, indexEnd - indexStart + 1));
            } while (expressionLength > indexCurrent);

            foreach (string section in sections)
            {
                string[] items = section.Split(',');
                if (items.Length != 3)
                {
                    throw new SyntaxException(this.Index, this.Length, "An error occurred on business logic configuration. Please contact Controlling team. The error details: Lack of number of Parameters in Score().");
                }

                string firstTag = items[0].Substring(0, 1);
                double firstValue;
                if (!double.TryParse(new Calculator().Run(items[0].Substring(1), vars), out firstValue))
                {
                    throw new SyntaxException(this.Index, this.Length, "An error occurred on business logic configuration. Please contact Controlling team. The error details: The parameters in Score() are not numeric type.");
                }

                string lastTag = items[2].Substring(items[2].Length - 1, 1);
                double lasttValue;
                if (!double.TryParse(new Calculator().Run(items[2].Substring(0, items[2].Length - 1), vars), out lasttValue))
                {
                    throw new SyntaxException(this.Index, this.Length, "An error occurred on business logic configuration. Please contact Controlling team. The error details: The parameters in Score() are not numeric type.");
                }

                bool result = false;
                if (firstTag == "(")
                {
                    result = value > firstValue;
                }
                else if (firstTag == "[")
                {
                    result = value >= firstValue;
                }
                //是否满足最小值验证
                if (!result)
                {
                    continue;//否则跳出当前验证
                }

                if (lastTag == ")")
                {
                    result = value < lasttValue;
                }
                else if (lastTag == "]")
                {
                    result = value <= lasttValue;
                }

                //能满足最大和最小值验证，则返回当前分数
                if (result)
                {
                    this.Value = new Calculator().Run(items[1], vars);
                    break;
                }
            }
        }
        public override void SetChildCount()
        {
            this.childCountMax = 2;
            this.childCountMin = 2;
        }
    }
}


