using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal class LookUpExpression : FuncExpression
    {
        public LookUpExpression(int index, int length)
            : base(index, length)
        {
            this.ValueType = typeof(string);
        }
        public override void Erpreter(DataTable vars)
        {
        }
        public override void SetChildCount()
        {
            this.childCountMax = 4;
            this.childCountMin = 4;
        }
    }
}
