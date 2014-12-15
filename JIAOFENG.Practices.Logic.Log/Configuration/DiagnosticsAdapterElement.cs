using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Log
{
    /// <summary>
    /// 所有的实现都在AdapterElement，此子类没用上
    /// </summary>
    public class DiagnosticsAdapterElement : AdapterElement
    {
        public override ILogTarget CreateLogTarget()
        {
            return new DiagnosticsAdapter();
        }
    }
}
