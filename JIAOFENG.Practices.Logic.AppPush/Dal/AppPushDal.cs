using System; 
using System.Collections.Generic;
using System.Data;

namespace JIAOFENG.Practices.Logic.AppPush
{
    internal abstract class AppPushDal : IAppPushDal
    {
        protected JIAOFENG.Practices.Database.Database database = null;
        public AppPushDal(JIAOFENG.Practices.Database.Database db)
        {
            this.database = db;
        }

        #region add
        public virtual ApppushEntity Add(ApppushEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region search
        public virtual List<ApppushEntity> GetWaitingSendMails(int failInterval, int failMostNum)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region delete
        public void Delete(string ids)
        {
            string sql = "DELETE FROM AppPush WHERE WechatPushID IN (" + ids + ")";
            database.ExecuteNonQuery(sql);
        }

		public void Delete(int id)
		{
            string sql = "DELETE FROM AppPush WHERE WechatPushID = " + id;
            database.ExecuteNonQuery(sql);
		}

        public void Delete(List<int> ids)
		{
			Delete(string.Join(",", ids));
		}

        public void Delete(int[] ids)
		{
			Delete(string.Join(",", ids));
		}
        #endregion

        #region update
        public virtual void UpdataMailStatus(int mailID, PushStatus status)
        {
            throw new NotImplementedException();
        }
        public virtual void UpdateSendCount(int mailID)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region private method
        private static ApppushEntity DataRowToEntity(DataRow dr)
        {
            ApppushEntity entity = new ApppushEntity();
            if (dr != null)
            {
                if (dr["AppPushID"] != DBNull.Value) { entity.AppPushID = Convert.ToInt32(dr["AppPushID"]); }
                if (dr["ClientID"] != DBNull.Value) { entity.ClientID = Convert.ToString(dr["ClientID"]); }
                if (dr["Client"] != DBNull.Value) { entity.Client = Convert.ToString(dr["Client"]); }
                if (dr["ClientVersion"] != DBNull.Value) { entity.ClientVersion = Convert.ToString(dr["ClientVersion"]); }
                if (dr["Subject"] != DBNull.Value) { entity.Subject = Convert.ToString(dr["Subject"]); }
                if (dr["Summary"] != DBNull.Value) { entity.Summary = Convert.ToString(dr["Summary"]); }
                if (dr["Body"] != DBNull.Value) { entity.Body = Convert.ToString(dr["Body"]); }
                if (dr["Logo"] != DBNull.Value) { entity.Logo = Convert.ToString(dr["Logo"]); }
                if (dr["Url"] != DBNull.Value) { entity.Url = Convert.ToString(dr["Url"]); }
                if (dr["CreateUserName"] != DBNull.Value) { entity.CreateUserName = Convert.ToString(dr["CreateUserName"]); }
                if (dr["Status"] != DBNull.Value) { entity.Status = Convert.ToInt32(dr["Status"]); }
                if (dr["SendCount"] != DBNull.Value) { entity.SendCount = Convert.ToInt32(dr["SendCount"]); }
                if (dr["IsBodyHtml"] != DBNull.Value) { entity.IsBodyHtml = Convert.ToBoolean(dr["IsBodyHtml"]); }
                if (dr["Priority"] != DBNull.Value) { entity.Priority = Convert.ToInt32(dr["Priority"]); }
                if (dr["LastTrySendTime"] != DBNull.Value) { entity.LastTrySendTime = Convert.ToDateTime(dr["LastTrySendTime"]); }
                if (dr["BodyEncoding"] != DBNull.Value) { entity.BodyEncoding = Convert.ToString(dr["BodyEncoding"]); }
                if (dr["CreateTime"] != DBNull.Value) { entity.CreateTime = Convert.ToDateTime(dr["CreateTime"]); }
                if (dr["CreateBy"] != DBNull.Value) { entity.CreateBy = Convert.ToInt32(dr["CreateBy"]); }
                if (dr["UpdateTime"] != DBNull.Value) { entity.UpdateTime = Convert.ToDateTime(dr["UpdateTime"]); }
                if (dr["UpdateBy"] != DBNull.Value) { entity.UpdateBy = Convert.ToInt32(dr["UpdateBy"]); }

            }
            return entity;
        }
        /// <summary>
        /// 转换数据行到实体对象
        /// </summary>
        protected static void DataRowToEntity(ApppushEntity entity, DataRow dr)
        {
            if (dr != null)
            {
                entity = DataRowToEntity(dr);
            }
        }
        protected static List<ApppushEntity> DataTableToList(DataTable dt)
        {
            List<ApppushEntity> list = new List<ApppushEntity>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(DataRowToEntity(dr));
            }
            return list;
        }
        #endregion
    }
}
