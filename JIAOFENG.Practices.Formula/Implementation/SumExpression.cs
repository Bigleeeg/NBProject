using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal class SumExpression : FuncExpression
    {
        public string ColumnName { get; set; }

        public SumExpression(int index, int length)
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
                throw new SyntaxException(this.Index, this.Length, "An error occurred on business logic configuration. Please contact Controlling team. The error details: cannot find column：[" + ColumnName + "]！");
            }

            try
            {
                double sum = 0;
                foreach (DataRow dr in vars.Rows)
                {
                    if (dr[this.ColumnName] != DBNull.Value && dr[this.ColumnName].ToString().Trim() != string.Empty)
                    {
                        sum += double.Parse(dr[this.ColumnName].ToString().Trim());
                    }
                }
                this.Value = sum;
            }
            catch
            {
                throw new SyntaxException(this.Index, this.Length, "An error occurred on business logic configuration. Please contact Controlling team. The error details: Parameters are incorrect：[" + ColumnName + "]！");
            }
        }
        public override void SetChildCount()
        {
            this.childCountMax = 1;
            this.childCountMin = 1;
        }
    }
}


//An error occurred on business logic configuration. Please contact Controlling team. The error details: The number of Parameters in Sin() is incorrect.//
//An error occurred on business logic configuration. Please contact Controlling team. The error details: The parameters in Sin() are not numeric type.//