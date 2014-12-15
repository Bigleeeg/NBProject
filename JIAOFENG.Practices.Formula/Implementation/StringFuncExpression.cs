using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal abstract class StringFuncExpression : FuncExpression
    {
        public StringFuncExpression(int index, int length)
            : base(index, length)
        {
            this.ValueType = typeof(string);
        }
    }
}
