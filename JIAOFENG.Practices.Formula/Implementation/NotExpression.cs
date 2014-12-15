using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class NotExpression : LogicExpression
    {
        public NotExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in ! operation is incorrect.");

            Expression expressionFirst = this.ChildList[0];
            expressionFirst.Erpreter(vars);
            this.Value = !expressionFirst.ChangeToBoolean();
        }
        public override void SetPriority()
        {
            this.priority = 5;
        }
        public override void SetChildCount()
        {
            this.childCountMax = 1;
            this.childCountMin = 1;
        }
    }
}

