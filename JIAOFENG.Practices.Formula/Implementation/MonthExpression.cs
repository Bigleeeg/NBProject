using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class MonthExpression : FuncExpression
    {
        public MonthExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in Month() is incorrect.");

            Expression expressionFirst = this.ChildList[0];
            expressionFirst.Erpreter(vars);
            DateTime dt;
            if (DateTime.TryParse(expressionFirst.ChangeToString(), out dt))
            {
                this.Value = (double)(dt.Month);
                this.ValueType = typeof(double);
            }
            else
            {
                throw new SyntaxException(this.Index, this.Length, "An error occurred on business logic configuration. Please contact Controlling team. The error details: The parameters in Month() are not date type.");
            }
        }
        public override void SetChildCount()
        {
            this.childCountMax = 1;
            this.childCountMin = 1;
        }
    }
}

 