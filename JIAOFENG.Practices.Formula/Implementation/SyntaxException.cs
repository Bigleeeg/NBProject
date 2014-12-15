using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Formula
{
    public class SyntaxException : Exception
    {
        public SyntaxException(int index, int length, string error):base(error)
        {
            this.Data.Add("index", index);
            this.Data.Add("Length", length);
        }
    }
}
