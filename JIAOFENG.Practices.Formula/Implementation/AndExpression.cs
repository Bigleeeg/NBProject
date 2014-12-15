using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class AndExpression : LogicExpression
    {
        public AndExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in & operation is incorrect.");
//&的运算元素数量不合法//

            Expression expressionFirst = this.ChildList[0];
            Expression expressionSec = this.ChildList[1];
            expressionFirst.Erpreter(vars);
            if (!expressionFirst.ChangeToBoolean())
            {
                this.Value = false;
            }
            else
            {
                expressionSec.Erpreter(vars);
                this.Value = expressionSec.ChangeToBoolean();
            }
        }
    }
}

