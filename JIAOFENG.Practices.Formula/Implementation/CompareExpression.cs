using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal abstract class CompareExpression : SymbolExpression
    {
        public CompareExpression(int index, int length)
            : base(index, length)
        {
            this.ValueType = typeof(bool);
        }
        public override void SetPriority()
        {
            this.priority = 4;
        }
        public override void SetChildCount()
        {
            this.childCountMax = 2;
            this.childCountMin = 2;
        }
    }
}
