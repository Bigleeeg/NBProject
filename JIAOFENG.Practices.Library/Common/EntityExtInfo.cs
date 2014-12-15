using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Library.Common
{
    /// <summary>
    /// 实体的扩展信息
    /// </summary>
    [Serializable]
    public class EntityExtInfo : IDBEntity
    {
        public EntityExtInfo()
        {
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public virtual int CreateBy { get; set; }

        private DateTime updateTime;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public virtual DateTime UpdateTime
        {
            get
            {
                if (this.updateTime < this.CreateTime)
                {
                    this.updateTime = this.CreateTime;
                }
                return this.updateTime;
            }
            set
            {
                this.updateTime = value;
            }
        }

        private int updateby;

        /// <summary>
        /// 最后操作者
        /// </summary>
        public virtual int UpdateBy
        {
            get
            {
                if (this.updateby == 0)
                {
                    this.updateby = this.CreateBy;
                }
                return this.updateby;
            }
            set
            {
                this.updateby = value;
            }
        }

        public virtual void CopyFrom(EntityExtInfo entity)
        {
            this.CreateTime = entity.CreateTime;
            this.CreateBy = entity.CreateBy;
            this.UpdateTime = entity.UpdateTime;
            this.UpdateBy = entity.UpdateBy;
        }
    }
}
