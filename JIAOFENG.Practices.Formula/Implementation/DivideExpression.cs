using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal class DivideExpression : ArithmeticSymbolExpression
    {
        public DivideExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred due to business logic configuration. Please contact Controlling team. The error details:  Number of Parameters in division operation is incorrect.");
//除法的运算元素数量不合法//

            Expression expressionFirst = this.ChildList[0];
            Expression expressionSecond = this.ChildList[1];
            expressionFirst.Erpreter(vars);
            expressionSecond.Erpreter(vars);
            double second = expressionSecond.ChangeToDouble("An error occurred due to business logic configuration. Please contact Controlling team. The error details:  Parameters in division operation are not numeric type.");
//除法的运算元素不是数值//
            if (second == 0)
            {
                throw new SyntaxException(this.Index, this.Length, "An error occurred due to business logic configuration. Please contact Controlling team. The error details:  Division by zero");
//除法的分母为零//
            }
            this.Value = expressionFirst.ChangeToDouble("An error occurred due to business logic configuration. Please contact Controlling team. The error details:  Parameters in division operation are not numeric type.") / second;
//除法的运算元素不是数值//
        }
        public override void SetPriority()
        {
            this.priority = 2;
        }
    }
}
