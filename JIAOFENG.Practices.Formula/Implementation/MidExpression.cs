using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class MidExpression : StringFuncExpression
    {
        public MidExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in Mid() is incorrect.");

            Expression expressionFirst = this.ChildList[0];
            Expression expressionSecond = this.ChildList[1];
            Expression expressionThree = this.ChildList[2];
            expressionFirst.Erpreter(vars);
            expressionSecond.Erpreter(vars);
            expressionThree.Erpreter(vars);
            this.Value = expressionFirst.ChangeToString().Substring(Convert.ToInt32(expressionSecond.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: The second parameter in Mid() is not integer.")), Convert.ToInt32(expressionThree.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: The third parameter in Mid() is not integer.")));
        }
        public override void SetChildCount()
        {
            this.childCountMax = 3;
            this.childCountMin = 3;
        }
    }
}
