using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace JIAOFENG.Practices.Library.Tasks
{
    /// <summary>
    /// 任务启动器
    /// </summary>
    public class BootStrapper
    {
        public static List<IBootStrapperTask> Tasks = new List<IBootStrapperTask>();
        /// <summary>
        /// 启动初始化任务
        /// </summary>
        public static void Start(HostTargets targets)
        {
            //var tasks = ObjectContainer.ResolveServices<IBootStrapperTask>();
            //foreach (var task in tasks)
            //{
            //    var usage = task.GetType().GetCustomAttribute<TaskUsageAttribute>();
            //    if (usage != null && (targets & usage.Targets) == targets)
            //    {
            //        task.Execute(BootStrapperTaskArgs.Empty);
            //    }
            //}

            foreach (IBootStrapperTask task in Tasks)
            {
                task.Execute(BootStrapperTaskArgs.Empty);
            }
        }
    }
}
