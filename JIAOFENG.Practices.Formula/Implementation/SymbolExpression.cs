using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal abstract class SymbolExpression : Expression
    {
        public SymbolExpression(int index,int length) : base(index, length)
        {
        }
        public override void SetChildCount()
        {
            this.childCountMax = 2;
            this.childCountMin = 2;
        }
    }
}
