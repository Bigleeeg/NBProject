using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class AcosExpression : ArithmeticFuncExpression
    {
        public AcosExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in Acos() is incorrect");

//Acos运算的参数数量不合法//

            Expression expressionFirst = this.ChildList[0];
            expressionFirst.Erpreter(vars);
            this.Value = Math.Acos(expressionFirst.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: Parameters in Acos() are not numeric type")) * 180 / Math.PI;
//Acos的运算元素不是数值//
        }
        public override void SetChildCount()
        {
            this.childCountMax = 1;
            this.childCountMin = 1;
        }
    }
}


