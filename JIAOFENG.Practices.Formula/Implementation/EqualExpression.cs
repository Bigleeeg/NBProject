using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class EqualExpression : CompareExpression
    {
        public EqualExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in Equal comparison operation is incorrect.");

            Expression expressionFirst = this.ChildList[0];
            Expression expressionSecond = this.ChildList[1];
            expressionFirst.Erpreter(vars);
            expressionSecond.Erpreter(vars);
            if (expressionFirst.ValueType == typeof(double) || expressionSecond.ValueType == typeof(double))
            {
                this.Value = expressionFirst.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: Parameters in Equal comparison operation are not numeric type.") == expressionSecond.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: Parameters in Equal comparison operation are not numeric type.");
            }
            else
            {
                this.Value = expressionFirst.ChangeToString() == expressionSecond.ChangeToString();
            }
        }
    }
}

