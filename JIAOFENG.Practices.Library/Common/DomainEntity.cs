using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace JIAOFENG.Practices.Library.Common
{
    /// <summary>
    /// 数据访问实体
    /// </summary>
    /// <typeparam name="TID">主键类型</typeparam>
    [Serializable]
    public class DomainEntity<T>
    {
        /// <summary>
        /// ID
        /// </summary>
        [XmlIgnore]
        public virtual T ID { get; set; }
    }

    /// <summary>
    /// 针对生产库的Entity
    /// </summary>
    [Serializable]
    public class DomainEntity : DomainEntity<int>, IPrimaryKey
    {
    }
}
