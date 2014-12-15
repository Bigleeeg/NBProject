using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal class XorExpression : LogicFuncExpression
    {
        public XorExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: The number of Parameters in Xor() is incorrect.");
            Expression expressionFirst = this.ChildList[0];
            expressionFirst.Erpreter(vars);
            Expression expressionSec = this.ChildList[1];
            expressionSec.Erpreter(vars);
            this.Value = !(expressionFirst.ChangeToBoolean() == expressionSec.ChangeToBoolean());
        }
        public override void SetChildCount()
        {
            this.childCountMax = 2;
            this.childCountMin = 2;
        }
    }
}


