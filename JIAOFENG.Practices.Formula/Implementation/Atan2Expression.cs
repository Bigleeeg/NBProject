using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class Atan2Expression : ArithmeticFuncExpression
    {
        public Atan2Expression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in Atan2() is incorrect.");
//Atan2运算的参数数量不合法//

            Expression expressionFirst = this.ChildList[0];
            expressionFirst.Erpreter(vars);
            Expression expressionSec = this.ChildList[1];
            expressionSec.Erpreter(vars);
            this.Value = Math.Atan2(expressionFirst.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: Parameters in Atan2() are not numeric type."), expressionSec.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: Parameters in Atan2() are not numeric type.")) * 180 / Math.PI;
//Atan2的运算元素不是数值//
        }
        public override void SetChildCount()
        {
            this.childCountMax = 2;
            this.childCountMin = 2;
        }
    }
}

