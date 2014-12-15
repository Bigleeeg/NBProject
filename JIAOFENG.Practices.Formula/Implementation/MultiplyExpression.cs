using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal class MultiplyExpression : ArithmeticSymbolExpression
    {
        public MultiplyExpression(int index, int length)
            : base(index, length)
        {
        }
      
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in Multiply operation is incorrect.");

            Expression expressionFirst = this.ChildList[0];
            Expression expressionSecond = this.ChildList[1];
            expressionFirst.Erpreter(vars);
            expressionSecond.Erpreter(vars);
            this.Value = expressionFirst.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: The parameters in Multiply operation  are not numeric type") * expressionSecond.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: The parameters in Multiply operation are not numeric type");
        }
        public override void SetPriority()
        {
            this.priority = 2;
        }
    }
}

