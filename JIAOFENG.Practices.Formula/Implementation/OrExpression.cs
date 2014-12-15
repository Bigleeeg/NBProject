using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class OrExpression : LogicExpression
    {
        public OrExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in | operation is incorrect.");

            Expression expressionFirst = this.ChildList[0];
            Expression expressionSec = this.ChildList[1];
            expressionFirst.Erpreter(vars);
            if (expressionFirst.ChangeToBoolean())
            {
                this.Value = true;
            }
            else
            {
                expressionSec.Erpreter(vars);
                this.Value = expressionSec.ChangeToBoolean();
            }
        }
    }
}

