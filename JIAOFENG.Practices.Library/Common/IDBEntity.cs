using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Library.Common
{
    /// <summary>
    /// 每一个DB表必须满足的字段
    /// </summary>
    public interface IDBEntity
    {
        /// <summary>
        /// 创建者
        /// </summary>
        int CreateBy { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后操作者
        /// </summary>
        int UpdateBy { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        DateTime UpdateTime { get; set; }
    }
}
