using JIAOFENG.Practices.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace JIAOFENG.Practices.Logic.Wechat
{
    internal class MySQLWechatPushDal : WechatPushDal
    {
        public MySQLWechatPushDal(JIAOFENG.Practices.Database.Database db)
            : base(db)
        {

        }
        #region add
        public override WechatpushEntity Add(WechatpushEntity entity)
        {
            const string sql = "INSERT INTO `wechatpush` (`ToUser`,`TemplateId`,`Url`,`TopColor`,`BodyXml`,`Status`,`SendCount`,`Priority`,`LastTrySendTime`,`BodyEncoding`,`CreateUserName`,`CreateTime`,`CreateBy`,`UpdateTime`,`UpdateBy`) VALUES (@ToUser,@TemplateId,@Url,@TopColor,@BodyXml,@Status,@SendCount,@Priority,@LastTrySendTime,@BodyEncoding,@CreateUserName,@CreateTime,@CreateBy,@UpdateTime,@UpdateBy);select last_insert_id();";

            List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySqlParameter>();
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@ToUser", entity.ToUser));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@TemplateId", entity.TemplateId));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@Url", entity.Url));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@TopColor", entity.TopColor));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@BodyXml", entity.BodyXml));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@Status", entity.Status));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@SendCount", entity.SendCount));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@Priority", entity.Priority));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@LastTrySendTime", entity.LastTrySendTime == null ? (object)DBNull.Value : entity.LastTrySendTime));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@BodyEncoding", entity.BodyEncoding));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@CreateUserName", entity.CreateUserName));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@CreateTime", entity.CreateTime));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@CreateBy", entity.CreateBy));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@UpdateTime", entity.UpdateTime));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@UpdateBy", entity.UpdateBy));

            int id = Convert.ToInt32(DatabaseFactory.CreateDatabase().ExecuteScalar(CommandType.Text, sql, sqlParameterList.ToArray()));
            if (id > 0)
            {
                entity.WechatPushID = id;
                return entity;
            }
            return null;
        }
        #endregion 

        #region search
        public override List<WechatpushEntity> GetWaitingSendMails(int failInterval, int failMostNum)
        {
            const string dateTimeBarFormat = "or (SendCount={0} and DATE_SUB(now(),INTERVAL {1} MINUTE) >= LastTrySendTime)";

            string dateTimeBar = string.Empty;
            for (int i = 1; i < failMostNum; i++)
            {
                dateTimeBar += string.Format(dateTimeBarFormat, i, Math.Pow(failInterval, i));
            }
            string strSql = @"SELECT * FROM WechatPush WHERE Status =" + (int)PushStatus.UnSend + " and (SendCount = 0 {0})";
            strSql = string.Format(strSql, dateTimeBar);
            DataSet ds = database.ExecuteDataSet(strSql);
            List<WechatpushEntity> list = DataTableToList(ds.Tables[0]);
            return list;
        }
        #endregion

        #region update
        public override void UpdataMailStatus(int wechatPushID, PushStatus status)
        {
            string strSql = "Update WechatPush SET LastTrySendTime=now(), Status=" + (int)status + " WHERE WechatPushID=" + wechatPushID;
            database.ExecuteNonQuery(strSql);
        }
        public override void UpdateSendCount(int wechatPushID)
        {
            string strSql = "Update WechatPush SET LastTrySendTime=now(), SendCount=SendCount+1 WHERE WechatPushID=" + wechatPushID;
            database.ExecuteNonQuery(strSql);
        }
        #endregion
    }
}
