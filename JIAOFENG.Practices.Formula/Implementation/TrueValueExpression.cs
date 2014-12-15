using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class TrueValueExpression : ValueExpression
    {
        public TrueValueExpression(int index, int length)
            : base(index, length)
        {
            this.ValueType = typeof(bool);
            this.Value = true;
        }
    }
}
