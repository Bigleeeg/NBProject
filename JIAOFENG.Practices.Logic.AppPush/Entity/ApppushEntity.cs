using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using JIAOFENG.Practices.Library.Common;

namespace JIAOFENG.Practices.Logic.AppPush
{
    public partial class ApppushEntity : EntityExtInfo
    {
	    #region constructor
        public ApppushEntity()
        {

        }
        public ApppushEntity(ApppushEntity entity)
        {
			CopyFrom(entity);
        }
		#endregion

		#region property

       public int AppPushID { get; set; } 

       public string ClientID { get; set; } 

       public string Client { get; set; } 

       public string ClientVersion { get; set; } 

       public string Subject { get; set; } 

       public string Summary { get; set; } 

       public string Body { get; set; } 

       public string Logo { get; set; } 

       public string Url { get; set; } 

       public string CreateUserName { get; set; } 

       public int Status { get; set; } 

       public int SendCount { get; set; } 

       public bool IsBodyHtml { get; set; } 

       public int Priority { get; set; } 

       public DateTime? LastTrySendTime { get; set; } 

       public string BodyEncoding { get; set; } 

		#endregion

		public void CopyFrom(ApppushEntity entity)
		{
          this.AppPushID = entity.AppPushID; 
          this.ClientID = entity.ClientID; 
          this.Client = entity.Client; 
          this.ClientVersion = entity.ClientVersion; 
          this.Subject = entity.Subject; 
          this.Summary = entity.Summary; 
          this.Body = entity.Body; 
          this.Logo = entity.Logo; 
          this.Url = entity.Url; 
          this.CreateUserName = entity.CreateUserName; 
          this.Status = entity.Status; 
          this.SendCount = entity.SendCount; 
          this.IsBodyHtml = entity.IsBodyHtml; 
          this.Priority = entity.Priority; 
          this.LastTrySendTime = entity.LastTrySendTime; 
          this.BodyEncoding = entity.BodyEncoding; 
          this.CreateTime = entity.CreateTime; 
          this.CreateBy = entity.CreateBy; 
          this.UpdateTime = entity.UpdateTime; 
          this.UpdateBy = entity.UpdateBy; 

		}
    }
}
