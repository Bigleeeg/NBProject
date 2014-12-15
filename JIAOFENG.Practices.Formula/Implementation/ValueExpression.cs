using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    class ValueExpression : Expression
    {
        public ValueExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
        }
        public override void SetPriority()
        {
            this.priority = 10;
        }
        public override void SetChildCount()
        {
            this.childCountMax = 0;
            this.childCountMin = 0;
        }
    }
}
