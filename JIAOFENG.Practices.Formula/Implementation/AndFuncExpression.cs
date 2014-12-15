using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal class AndFuncExpression : LogicFuncExpression
    {
        public AndFuncExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in And() is incorrect");
//And运算的参数数量不合法//
            foreach (Expression expression in this.ChildList)
            {
                expression.Erpreter(vars);
                if (!expression.ChangeToBoolean())
                {
                    this.Value = false;
                    return;
                }
            }
            this.Value = true;
        }
    }
}

