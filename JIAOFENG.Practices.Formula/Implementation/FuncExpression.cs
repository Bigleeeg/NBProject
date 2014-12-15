using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal abstract class FuncExpression : Expression
    {
        public FuncExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void SetPriority()
        {
            this.priority = 7;
        }
        public override void SetChildCount()
        {
            this.childCountMax = 1000;
            this.childCountMin = 1;
        }
    }
}
