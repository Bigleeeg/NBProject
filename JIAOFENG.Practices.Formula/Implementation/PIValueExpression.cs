using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class PIValueExpression : ValueExpression
    {
        public PIValueExpression(int index, int length)
            : base(index, length)
        {
            this.ValueType = typeof(double);
            this.Value = Math.PI;
        }
    }
}
