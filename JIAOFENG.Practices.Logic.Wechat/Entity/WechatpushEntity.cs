using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using JIAOFENG.Practices.Library.Common;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public partial class WechatpushEntity : EntityExtInfo
    {
	    #region constructor
        public WechatpushEntity()
        {

        }
        public WechatpushEntity(WechatpushEntity entity)
        {
			CopyFrom(entity);
        }
		#endregion

		#region property

       public int WechatPushID { get; set; } 

       public string ToUser { get; set; } 

       public string TemplateId { get; set; } 

       public string Url { get; set; } 

       public string TopColor { get; set; } 

       public string BodyXml { get; set; } 

       public int Status { get; set; } 

       public int SendCount { get; set; } 

       public int Priority { get; set; } 

       public DateTime? LastTrySendTime { get; set; } 

       public string BodyEncoding { get; set; } 

       public string CreateUserName { get; set; } 

		#endregion

		public void CopyFrom(WechatpushEntity entity)
		{
          this.WechatPushID = entity.WechatPushID; 
          this.ToUser = entity.ToUser; 
          this.TemplateId = entity.TemplateId; 
          this.Url = entity.Url; 
          this.TopColor = entity.TopColor; 
          this.BodyXml = entity.BodyXml; 
          this.Status = entity.Status; 
          this.SendCount = entity.SendCount; 
          this.Priority = entity.Priority; 
          this.LastTrySendTime = entity.LastTrySendTime; 
          this.BodyEncoding = entity.BodyEncoding; 
          this.CreateUserName = entity.CreateUserName; 
          this.CreateTime = entity.CreateTime; 
          this.CreateBy = entity.CreateBy; 
          this.UpdateTime = entity.UpdateTime; 
          this.UpdateBy = entity.UpdateBy; 

		}
    }
}
