using System;
using System.Collections.Generic;
using System.Text;

namespace JIAOFENG.Practices.Library.Utility
{
    [Serializable]
    public class CallMessage
    {       
        public CallMessage(string className, string methodName)
            : this(className, methodName, null)
        {
        }
        public CallMessage(string className, string methodName, object[] args)
        {
            this.ClassName = className;
            this.MethodName = methodName;
            this.Args = args;
        }
        public int UserID { get;set;}
        public string ClassName { get;set;}
        public string MethodName { get;set;}
        public object[] Args { get;set;}
    }
}
