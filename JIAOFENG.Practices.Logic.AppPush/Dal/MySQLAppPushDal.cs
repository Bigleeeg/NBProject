using JIAOFENG.Practices.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace JIAOFENG.Practices.Logic.AppPush
{
    internal class MySQLAppPushDal : AppPushDal
    {
        public MySQLAppPushDal(JIAOFENG.Practices.Database.Database db)
            : base(db)
        {

        }
        #region add
        public override ApppushEntity Add(ApppushEntity entity)
        {
            const string sql = "INSERT INTO `apppush` (`ClientID`,`Client`,`ClientVersion`,`Subject`,`Summary`,`Body`,`Logo`,`Url`,`CreateUserName`,`Status`,`SendCount`,`IsBodyHtml`,`Priority`,`LastTrySendTime`,`BodyEncoding`,`CreateTime`,`CreateBy`,`UpdateTime`,`UpdateBy`) VALUES (@ClientID,@Client,@ClientVersion,@Subject,@Summary,@Body,@Logo,@Url,@CreateUserName,@Status,@SendCount,@IsBodyHtml,@Priority,@LastTrySendTime,@BodyEncoding,@CreateTime,@CreateBy,@UpdateTime,@UpdateBy);select last_insert_id();";

            List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySqlParameter>();
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@ClientID", entity.ClientID));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@Client", entity.Client == null ? (object)DBNull.Value : entity.Client));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@ClientVersion", entity.ClientVersion == null ? (object)DBNull.Value : entity.ClientVersion));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@Subject", entity.Subject));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@Summary", entity.Summary));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@Body", entity.Body));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@Logo", entity.Logo == null ? (object)DBNull.Value : entity.Logo));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@Url", entity.Url == null ? (object)DBNull.Value : entity.Url));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@CreateUserName", entity.CreateUserName));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@Status", entity.Status));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@SendCount", entity.SendCount));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@IsBodyHtml", entity.IsBodyHtml));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@Priority", entity.Priority));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@LastTrySendTime", entity.LastTrySendTime == null ? (object)DBNull.Value : entity.LastTrySendTime));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@BodyEncoding", entity.BodyEncoding));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@CreateTime", entity.CreateTime));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@CreateBy", entity.CreateBy));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@UpdateTime", entity.UpdateTime));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@UpdateBy", entity.UpdateBy));


            int id = Convert.ToInt32(DatabaseFactory.CreateDatabase().ExecuteScalar(CommandType.Text, sql, sqlParameterList.ToArray()));
            if (id > 0)
            {
                entity.AppPushID = id;
                return entity;
            }
            return null;
        }
        #endregion 

        #region search
        public override List<ApppushEntity> GetWaitingSendMails(int failInterval, int failMostNum)
        {
            const string dateTimeBarFormat = "or (SendCount={0} and DATE_SUB(now(),INTERVAL {1} MINUTE) >= LastTrySendTime)";

            string dateTimeBar = string.Empty;
            for (int i = 1; i < failMostNum; i++)
            {
                dateTimeBar += string.Format(dateTimeBarFormat, i, Math.Pow(failInterval, i));
            }
            string strSql = @"SELECT * FROM AppPush WHERE Status =" + (int)PushStatus.UnSend + " and (SendCount = 0 {0})";
            strSql = string.Format(strSql, dateTimeBar);
            DataSet ds = database.ExecuteDataSet(strSql);
            List<ApppushEntity> list = DataTableToList(ds.Tables[0]);
            return list;
        }
        #endregion

        #region update
        public override void UpdataMailStatus(int appPushID, PushStatus status)
        {
            string strSql = "Update AppPush SET LastTrySendTime=now(), Status=" + (int)status + " WHERE AppPushID=" + appPushID;
            database.ExecuteNonQuery(strSql);
        }
        public override void UpdateSendCount(int appPushID)
        {
            string strSql = "Update AppPush SET LastTrySendTime=now(), SendCount=SendCount+1 WHERE AppPushID=" + appPushID;
            database.ExecuteNonQuery(strSql);
        }
        #endregion
    }
}
