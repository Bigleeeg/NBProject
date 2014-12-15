using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class CosExpression : ArithmeticFuncExpression
    {
        public CosExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in cos() is incorrect.");

            Expression expressionFirst = this.ChildList[0];
            expressionFirst.Erpreter(vars);
            this.Value = Math.Cos(expressionFirst.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: Parameters in cos() are not numeric type.") * Math.PI / 180);
        }
        public override void SetChildCount()
        {
            this.childCountMax = 1;
            this.childCountMin = 1;
        }
    }
}

