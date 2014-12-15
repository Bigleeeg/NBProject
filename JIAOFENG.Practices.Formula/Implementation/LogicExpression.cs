using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal abstract class LogicExpression : SymbolExpression
    {
        public LogicExpression(int index, int length)
            : base(index, length)
        {
            this.ValueType = typeof(bool);
        }
        public override void SetPriority()
        {
            this.priority = 3;
        }
    }
}
