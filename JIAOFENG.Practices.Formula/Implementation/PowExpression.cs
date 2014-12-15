using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class PowExpression : ArithmeticFuncExpression
    {
        public PowExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in Pow() is incorrect.");

            Expression expressionFirst = this.ChildList[0];
            Expression expressionSecond = this.ChildList[1];
            expressionFirst.Erpreter(vars);
            expressionSecond.Erpreter(vars);
            this.Value = Math.Pow(expressionFirst.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: The parameters in Pow() are not numeric type."), expressionSecond.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: The parameters in Pow() are not numeric type."));
        }
        public override void SetChildCount()
        {
            this.childCountMax = 2;
            this.childCountMin = 2;
        }
    }
}

