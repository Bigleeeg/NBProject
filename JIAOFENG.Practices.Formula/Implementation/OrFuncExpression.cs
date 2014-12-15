using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal class OrFuncExpression : LogicFuncExpression
    {
        public OrFuncExpression(int index, int length)
            : base(index, length)
        {
            this.ValueType = typeof(bool);
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in Or operation is incorrect.");
            foreach (Expression expression in this.ChildList)
            {
                expression.Erpreter(vars);
                if (expression.ChangeToBoolean())
                {
                    this.Value = true;
                    return;
                }
            }
            this.Value = false;
        }
        public override void SetChildCount()
        {
            this.childCountMax = 1000;
            this.childCountMin = 2;
        }
    }
}

