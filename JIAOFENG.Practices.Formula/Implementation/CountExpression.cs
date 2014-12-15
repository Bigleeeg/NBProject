using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal class CountExpression : FuncExpression
    {
        public string ColumnName { get; set; }

        public CountExpression(int index, int length)
            : base(index, length)
        {
            this.ValueType = typeof(double);
        }
        public override void Erpreter(DataTable vars)
        {
            VarExpression expressionFirst = (VarExpression)this.ChildList[0];
            this.ColumnName = expressionFirst.VariableName;
            if (vars.Columns[this.ColumnName] == null)
            {
                throw new SyntaxException(this.Index, this.Length, "An error occurred on business logic configuration. Please contact Controlling team. The error details: cannot find the column：[" + ColumnName + "]！");
            }

            HashSet<string> hashSet = new HashSet<string>();
            foreach (DataRow dr in vars.Rows)
            {
                if (dr[this.ColumnName] != DBNull.Value && dr[this.ColumnName].ToString().Trim() != string.Empty)
                {
                    hashSet.Add(dr[this.ColumnName].ToString().Trim());
                }
            }
            this.Value = hashSet.Count;
        }
        public override void SetChildCount()
        {
            this.childCountMax = 1;
            this.childCountMin = 1;
        }
    }
}


