using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Library.Tasks
{
    /// <summary>
    /// 任务执行宿主目标
    /// </summary>
    public enum HostTargets
    {
        /// <summary>
        /// Web站点
        /// </summary>
        Web = 1,

        /// <summary>
        /// 后台服务
        /// </summary>
        NTService = 2,

        /// <summary>
        /// Winform应用程序
        /// </summary>
        Application = 4,
    }
}
