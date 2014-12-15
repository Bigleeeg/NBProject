using JIAOFENG.Practices.Library.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Formula
{
    public delegate string FunctionExtension(params object[] paras);
    class DelegateExpression : FuncExpression
    {
        FunctionExtension func = null;
        public DelegateExpression(int index, int length, FunctionExtension func)
            : base(index, length)
        {
            this.func = func;
        }
        public override void Erpreter(DataTable vars)
        {
            object[] paras = new object[this.ChildList.Count];
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in Delegate() is incorrect. ");
					// Delegate 运算的参数数量不合法//
            for(int i=0; i< this.ChildList.Count ;i++)
            {
                this.ChildList[i].Erpreter(vars);
                paras[i] = this.ChildList[i].Value;
            }
            this.Value = func.Invoke(paras);            
        }
    }
}
