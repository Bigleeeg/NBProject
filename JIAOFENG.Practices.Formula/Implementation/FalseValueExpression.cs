using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class FalseValueExpression : ValueExpression
    {
        public FalseValueExpression(int index, int length)
            : base(index, length)
        {
            this.ValueType = typeof(bool);
            this.Value = false;
        }
    }
}
