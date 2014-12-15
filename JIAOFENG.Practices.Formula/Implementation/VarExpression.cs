using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    internal class VarExpression : Expression
    {
        public string VariableName { get; set; }
        public VarExpression(int index, int length, string variable)
            : base(index, length)
        {
            this.VariableName = variable;
        }
        public override void Erpreter(DataTable vars)
        {
            if (vars.Rows[0][this.VariableName] != DBNull.Value)
            {
                if (vars.Rows[0][this.VariableName] is Delegate)
                {
                    try
                    {
                        VarExtension v = (VarExtension)vars.Rows[0][this.VariableName];
                        this.Value = v.Invoke();
                        this.ValueType = typeof(string);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("operating error [{0} with {1}]", ex.Message, this.VariableName));
                    }
                }
                else
                {
                    this.Value = vars.Rows[0][this.VariableName].ToString();
                    this.ValueType = typeof(string);
                }               
            }
            else
            {
                throw new SyntaxException(this.Index, this.Length, "operation error：[" + this.VariableName + "]！");
            }
        }
        public override void SetPriority()
        {
            this.priority = 8;
        }
        public override void SetChildCount()
        {
            this.childCountMax = 0;
            this.childCountMin = 0;
        }
    }
}

