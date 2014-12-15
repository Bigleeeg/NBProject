using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal abstract class ArithmeticSymbolExpression : SymbolExpression
    {
        public ArithmeticSymbolExpression(int index, int length)
            : base(index, length)
        {
            this.ValueType = typeof(double);
        }
        public override void SetChildCount()
        {
            this.childCountMax = 2;
            this.childCountMin = 2;
        }
    }
}
