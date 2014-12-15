using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Library.Tasks
{
    /// <summary>
    /// 后台任务
    /// </summary>
    public interface IBackgroundTask
    {
        /// <summary>
        /// 执行后台任务
        /// </summary>
        /// <param name="args">后参任务参数</param>
        void Execute(BackgroundTaskArgs args);
    }
}
