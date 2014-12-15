using System; 
using System.Collections.Generic;
using System.Data;

namespace JIAOFENG.Practices.Logic.Wechat
{
    internal abstract class WechatPushDal : IWechatPushDal
    {
        protected JIAOFENG.Practices.Database.Database database = null;
        public WechatPushDal(JIAOFENG.Practices.Database.Database db)
        {
            this.database = db;
        }

        #region add
        public virtual WechatpushEntity Add(WechatpushEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region search
        public virtual List<WechatpushEntity> GetWaitingSendMails(int failInterval, int failMostNum)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region delete
        public void Delete(string ids)
        {
            string sql = "DELETE FROM WechatPush WHERE WechatPushID IN (" + ids + ")";
            database.ExecuteNonQuery(sql);
        }

		public void Delete(int id)
		{
            string sql = "DELETE FROM WechatPush WHERE WechatPushID = " + id;
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
        private static WechatpushEntity DataRowToEntity(DataRow dr)
        {
            WechatpushEntity entity = new WechatpushEntity();
            if (dr != null)
            {
                if (dr["WechatPushID"] != DBNull.Value) { entity.WechatPushID = Convert.ToInt32(dr["WechatPushID"]); }
                if (dr["ToUser"] != DBNull.Value) { entity.ToUser = Convert.ToString(dr["ToUser"]); }
                if (dr["TemplateId"] != DBNull.Value) { entity.TemplateId = Convert.ToString(dr["TemplateId"]); }
                if (dr["Url"] != DBNull.Value) { entity.Url = Convert.ToString(dr["Url"]); }
                if (dr["TopColor"] != DBNull.Value) { entity.TopColor = Convert.ToString(dr["TopColor"]); }
                if (dr["BodyXml"] != DBNull.Value) { entity.BodyXml = Convert.ToString(dr["BodyXml"]); }
                if (dr["Status"] != DBNull.Value) { entity.Status = Convert.ToInt32(dr["Status"]); }
                if (dr["SendCount"] != DBNull.Value) { entity.SendCount = Convert.ToInt32(dr["SendCount"]); }
                if (dr["Priority"] != DBNull.Value) { entity.Priority = Convert.ToInt32(dr["Priority"]); }
                if (dr["LastTrySendTime"] != DBNull.Value) { entity.LastTrySendTime = Convert.ToDateTime(dr["LastTrySendTime"]); }
                if (dr["BodyEncoding"] != DBNull.Value) { entity.BodyEncoding = Convert.ToString(dr["BodyEncoding"]); }
                if (dr["CreateUserName"] != DBNull.Value) { entity.CreateUserName = Convert.ToString(dr["CreateUserName"]); }
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
        protected static void DataRowToEntity(WechatpushEntity entity, DataRow dr)
        {
            if (dr != null)
            {
                entity = DataRowToEntity(dr);
            }
        }
        protected static List<WechatpushEntity> DataTableToList(DataTable dt)
        {
            List<WechatpushEntity> list = new List<WechatpushEntity>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(DataRowToEntity(dr));
            }
            return list;
        }
        #endregion
    }
}
