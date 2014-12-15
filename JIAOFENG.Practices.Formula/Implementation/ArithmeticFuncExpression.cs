using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal abstract class ArithmeticFuncExpression : FuncExpression
    {
        public ArithmeticFuncExpression(int index, int length)
            : base(index, length)
        {
            this.ValueType = typeof(double);
        }
    }
}
