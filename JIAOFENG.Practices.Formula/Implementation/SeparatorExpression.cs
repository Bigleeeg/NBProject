using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal class SeparatorExpression : SymbolExpression
    {
        public SeparatorExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
        }
        public override void SetPriority()
        {
            this.priority = 6;
        }
        public override void SetChildCount()
        {
            //只计算顶级公式
            this.childCountMax = 1;
            this.childCountMin = 1;
        }
    }
}
