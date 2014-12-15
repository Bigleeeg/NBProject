using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Library.Tasks
{
    /// <summary>
    /// 启动任务
    /// </summary>
    public interface IBootStrapperTask
    {
        /// <summary>
        /// 执行引导程序
        /// </summary>
        /// <param name="args">启动任务参数</param>
        void Execute(BootStrapperTaskArgs args);

        /// <summary>
        /// 引导程序清理
        /// </summary>
        /// <param name="args">程序启动参数</param>
        void Cleanup(BootStrapperTaskArgs args);
    }
}
