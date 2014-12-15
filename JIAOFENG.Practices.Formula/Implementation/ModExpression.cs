using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class ModExpression : ArithmeticSymbolExpression
    {
        public ModExpression(int index, int length)
            : base(index, length)
        {
        }
      
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in Mod() is incorrect.");

            Expression expressionFirst = this.ChildList[0];
            Expression expressionSecond = this.ChildList[1];
            expressionFirst.Erpreter(vars);
            expressionSecond.Erpreter(vars);
            this.Value = expressionFirst.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: The parameters in Mod() are not numeric type.") % expressionSecond.ChangeToDouble("An error occurred on business logic configuration. Please contact Controlling team. The error details: The parameters in Mod() are not numeric type.");
        }
        public override void SetPriority()
        {
            this.priority = 2;
        }
    }
}



//乘法的运算元素不是数值//
//这里是乘法吗？我改成了Mod//