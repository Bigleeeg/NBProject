using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class AtanExpression : ArithmeticFuncExpression
    {
        public AtanExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in Atan() is incorrect");

            Expression expressionFirst = this.ChildList[0];
            expressionFirst.Erpreter(vars);
            this.Value = Math.Atan(expressionFirst.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: Parameters in Atan2() are not numeric type.")) * 180 / Math.PI;
        }
        public override void SetChildCount()
        {
            this.childCountMax = 1;
            this.childCountMin = 1;
        }
    }
}

