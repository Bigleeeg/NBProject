using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Reflection;

namespace JIAOFENG.Practices.Formula
{
    class CallExpression : FuncExpression
    {
        public CallExpression(int index, int length)
            : base(index, length)
        {
        }
        public override void Erpreter(DataTable vars)
        {
            this.CheckChildCount("An error occurred on business logic configuration. Please contact Controlling team. The error details: Number of Parameters in Call() is incorrect.");
            Expression expressionClassName = this.ChildList[0];
            expressionClassName.Erpreter(vars);
            Expression expressionMethodName = this.ChildList[1];
            expressionMethodName.Erpreter(vars);

            Assembly assembly = Assembly.GetEntryAssembly();
            object eval = assembly.CreateInstance(expressionClassName.ChangeToString());
            if (eval == null)
            {
                throw new SyntaxException(this.Index, this.Length, "An error occurred on business logic configuration. Please contact Controlling team. The error details: Naming of method Call() is incorrect-" + expressionClassName.ChangeToString());
            }
            MethodInfo method = eval.GetType().GetMethod(expressionMethodName.ChangeToString());
            if (method == null)
            {
                throw new SyntaxException(this.Index, this.Length, "An error occurred on business logic configuration. Please contact Controlling team. The error details: Naming of method Call() is incorrect-" + expressionMethodName.ChangeToString());
            }
            object reobj = null;
            object[] args = new object[this.ChildList.Count - 2];

            for (int i = 2; i < this.ChildList.Count; i++)
            {
                args[i-2] = this.ChildList[i].Value;
            }
            try
            {
                reobj = method.Invoke(eval, args);
            }
            catch (Exception ex)
            {
                throw new SyntaxException(this.Index, this.Length, "An error occurred on business logic configuration. Please contact Controlling team. The error details: Excution of Call() failed." + expressionClassName.ChangeToString() + "-" + expressionMethodName.ChangeToString() + ":with error:" + ex.Message);
            }
            this.Value =  reobj;
        }
        public override void SetChildCount()
        {
            this.childCountMax = 1000;
            this.childCountMin = 2;
        }
    }
}

