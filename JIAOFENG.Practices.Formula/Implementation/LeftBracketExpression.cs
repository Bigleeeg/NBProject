using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal class LeftBracketExpression : SeparatorExpression
    {
        public LeftBracketExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in operation is incorrect.");

            Expression expressionFirst = this.ChildList[0];
            expressionFirst.Erpreter(vars);
            this.Value = expressionFirst.Value;
            this.ValueType = expressionFirst.ValueType;
        }
    }
}

