using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Library.Tasks
{
    /// <summary>
    /// 加载后台任务
    /// </summary>
    [TaskUsageAttribute(HostTargets.NTService)]
    public class LoadBackgroundTask : IBootStrapperTask
    {
        private readonly IEnumerable<IBackgroundTask> tasks;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// 后台任务
        /// </summary>
        /// <param name="tasks">所有的后台任务枚举</param>
        public LoadBackgroundTask(IEnumerable<IBackgroundTask> tasks)
        {
            this.tasks = tasks;
        }

        /// <summary>
        /// 执行所有的后台任务
        /// </summary>
        /// <param name="args">后台任务参数</param>
        public void Execute(BootStrapperTaskArgs args)
        {
            foreach (var task in tasks)
            {
                Task taskRunner=Task.Factory.StartNew(() =>
                {
                    BackgroundTaskArgs taskArgs = new BackgroundTaskArgs();
                    taskArgs.Token = cancellationTokenSource.Token;
                    task.Execute(taskArgs);

                }, cancellationTokenSource.Token);
            }
        }

        /// <summary>
        /// 清空所有后台任务
        /// </summary>
        /// <param name="args">后台任务参数</param>
        public void Cleanup(BootStrapperTaskArgs args)
        {
            cancellationTokenSource.Cancel();
        }
    }
}
