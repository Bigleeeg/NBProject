using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Library.Tasks
{
    /// <summary>
    /// 任务Usage
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TaskUsageAttribute : Attribute
    {
        private readonly HostTargets hostTargets;

        public TaskUsageAttribute(HostTargets hostTargets)
        {
            this.hostTargets = hostTargets;
        }

        /// <summary>
        /// 任务执行宿主目标
        /// </summary>
        public HostTargets Targets
        {
            get { return hostTargets; }
        }
    }
}
