using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal class IfExpression : FuncExpression
    {
        public IfExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in if() operation is incorrect.");

            Expression expressionFirst = this.ChildList[0];
            Expression expressionSecond = this.ChildList[1];
            Expression expressionThree = this.ChildList[2];
            expressionFirst.Erpreter(vars);
            if (expressionFirst.ChangeToBoolean())
            {
                expressionSecond.Erpreter(vars);
                this.Value = expressionSecond.Value;
            }
            else
            {
                expressionThree.Erpreter(vars);
                this.Value = expressionThree.Value;
            }
        }
        public override void SetChildCount()
        {
            this.childCountMax = 3;
            this.childCountMin = 3;
        }
    }
}

