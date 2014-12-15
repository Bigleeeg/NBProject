using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class MinExpression : ArithmeticFuncExpression
    {
        public MinExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in Min() is incorrect.");

            this.ChildList[0].Erpreter(vars);
            double temp = this.ChildList[0].ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: The parameters in Min() are not numeric type.");
            for (int i = 1; i < this.ChildList.Count;i++ )
            {
                this.ChildList[i].Erpreter(vars);
                temp = Math.Min(temp, this.ChildList[i].ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: The parameters in Min() are not numeric type."));
            }
            this.Value = temp;
        }
    }
}

