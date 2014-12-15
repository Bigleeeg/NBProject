using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal abstract class LogicFuncExpression : FuncExpression
    {
        public LogicFuncExpression(int index, int length)
            : base(index, length)
        {
            this.ValueType = typeof(bool);
        }
        public override void SetChildCount()
        {
            this.childCountMax = 1000;
            this.childCountMin = 2;
        }
    }
}
