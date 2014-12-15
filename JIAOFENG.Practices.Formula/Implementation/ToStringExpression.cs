using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class ToStringExpression : StringFuncExpression
    {
        public ToStringExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: The number of Parameters in String() is incorrect.");

            Expression expressionFirst = this.ChildList[0];
            expressionFirst.Erpreter(vars);
            this.Value = expressionFirst.ChangeToString();
        }
        public override void SetChildCount()
        {
            this.childCountMax = 1;
            this.childCountMin = 1;
        }
    }
}



