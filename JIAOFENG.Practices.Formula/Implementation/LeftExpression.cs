using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class LeftExpression : StringFuncExpression
    {
        public LeftExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in Left() is incorrect.");

            Expression expressionFirst = this.ChildList[0];
            Expression expressionSecond = this.ChildList[1];
            expressionFirst.Erpreter(vars);
            expressionSecond.Erpreter(vars);
            this.Value = expressionFirst.ChangeToString().Substring(0, Convert.ToInt32(expressionSecond.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: The second parameter in Left() is not integer.")));
        }
        public override void SetChildCount()
        {
            this.childCountMax = 2;
            this.childCountMin = 2;
        }
    }
}


