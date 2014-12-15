using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Library.Tasks
{
    /// <summary>
    /// 后台任务的参数
    /// </summary>
    public class BackgroundTaskArgs
    {
        /// <summary>
        /// 传播有关应取消操作的通知。
        /// </summary>
        public CancellationToken Token { get; set; }
    }
}
