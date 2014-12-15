using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Data; 
using System.Data.Common;
using System.Data.SqlClient; 
using Dashinginfo.Practices.Database; 
using Dashinginfo.Practices.Library.Common;
using Dashinginfo.Practices.Logic.Mail.Entity;

namespace Dashinginfo.Practices.Logic.Mail
{
    internal static class MailAttachmentDal
    {
	    #region add 
        public static MailAttachmentEntity Add(MailAttachmentEntity entity)
        {
            string sql = string.Empty;
            switch (MailManager.DatabaseProviderType)
            {
                case DatabaseProviderType.SqlServer:
                    sql = "INSERT INTO MailAttachment (AttachmentType,FileName,NameEncoding,Attachment,MailID,CreateTime,CreateBy) VALUES (@AttachmentType,@FileName,@NameEncoding,@Attachment,@MailID,@CreateTime,@CreateBy);SELECT SCOPE_IDENTITY();";
                    break;
                case DatabaseProviderType.Oracle:
                    sql = "INSERT INTO MailAttachment (MailAttachmentID, AttachmentType,FileName,NameEncoding,Attachment,MailID,CreateTime,CreateBy) VALUES (seq_MailAttachment.Nextval, @AttachmentType,@FileName,@NameEncoding,@Attachment,@MailID,@CreateTime,@CreateBy);select seq_MailAttachment.CURRVAL from dual;";
                    break;
                case DatabaseProviderType.MySQL:
                    sql = "INSERT INTO MailAttachment (AttachmentType,FileName,NameEncoding,Attachment,MailID,CreateTime,CreateBy) VALUES (@AttachmentType,@FileName,@NameEncoding,@Attachment,@MailID,@CreateTime,@CreateBy);select last_insert_id();";
                    break;
                default:
                    sql = "INSERT INTO MailAttachment (AttachmentType,FileName,NameEncoding,Attachment,MailID,CreateTime,CreateBy) VALUES (@AttachmentType,@FileName,@NameEncoding,@Attachment,@MailID,@CreateTime,@CreateBy);SELECT SCOPE_IDENTITY();";
                    break;
            }

            List<DbParameter> sqlParameterList = new List<DbParameter>();
            sqlParameterList.Add(MailManager.CreateDbParameter("@AttachmentType", entity.AttachmentType));
            sqlParameterList.Add(MailManager.CreateDbParameter("@FileName", entity.FileName));
            sqlParameterList.Add(MailManager.CreateDbParameter("@NameEncoding", entity.NameEncoding));
            sqlParameterList.Add(MailManager.CreateDbParameter("@Attachment", entity.Attachment == null ? (object)DBNull.Value : entity.Attachment));
            sqlParameterList.Add(MailManager.CreateDbParameter("@MailID", entity.MailID));
            sqlParameterList.Add(MailManager.CreateDbParameter("@CreateTime", entity.CreateTime));
            sqlParameterList.Add(MailManager.CreateDbParameter("@CreateBy", entity.CreateBy));

            int id = Convert.ToInt32(DatabaseFactory.CreateDatabase().ExecuteScalar(CommandType.Text, sql, sqlParameterList.ToArray()));
            if (id > 0)
            {
                entity.MailAttachmentID = id;
                return entity;
            }
            return null;
        }
		#endregion 

		#region delete
        public static void Delete(string ids)
        {
            string sql = "DELETE FROM MailAttachment WHERE MailAttachmentID IN (" + ids + ")";
            DatabaseFactory.CreateDatabase().ExecuteNonQuery(sql);
        }

		public static void Delete(int id)
		{
            string sql = "DELETE FROM MailAttachment WHERE MailAttachmentID = " + id;
            DatabaseFactory.CreateDatabase().ExecuteNonQuery(sql);
		}

        public static void Delete(List<int> ids)
		{
			Delete(string.Join(",", ids));
		}

        public static void Delete(int[] ids)
		{
			Delete(string.Join(",", ids));
		}
		#endregion 

		#region search
        /// <summary>
        /// 得到多个对象实体
        /// </summary>
        public static List<MailAttachmentEntity> GetList(string where, DatabaseProviderType providerType, string connectingString)
        {
            DataSet ds = GetDataSet(where, providerType, connectingString);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 得到DataSet
        /// </summary>
        public static DataSet GetDataSet(string where, DatabaseProviderType providerType, string connectingString)
        {
            if (where == null) where = string.Empty;
            if (where.Trim() != string.Empty)
            {
                where = " where " + where;
            }
            string sql = "select * from MailAttachment" + where;
            DataSet ds = DatabaseFactory.CreateDatabase(providerType, connectingString).ExecuteDataSet(sql);
            return ds;
        }
		#endregion 

        #region private method
        /// <summary>
        /// 转换数据行到实体对象
        /// </summary>
        private static MailAttachmentEntity DataRowToEntity(DataRow dr)
        {
            MailAttachmentEntity entity = new MailAttachmentEntity();
            if (dr != null)
            {
                if (dr["MailAttachmentID"] != DBNull.Value) { entity.MailAttachmentID = Convert.ToInt32(dr["MailAttachmentID"]); }
                if (dr["AttachmentType"] != DBNull.Value) { entity.AttachmentType = Convert.ToInt32(dr["AttachmentType"]); }
                if (dr["FileName"] != DBNull.Value) { entity.FileName = Convert.ToString(dr["FileName"]); }
                if (dr["NameEncoding"] != DBNull.Value) { entity.NameEncoding = Convert.ToString(dr["NameEncoding"]); }
                if (dr["Attachment"] != DBNull.Value) { entity.Attachment = (Byte[])(dr["Attachment"]); }
                if (dr["MailID"] != DBNull.Value) { entity.MailID = Convert.ToInt32(dr["MailID"]); }
                if (dr["CreateTime"] != DBNull.Value) { entity.CreateTime = Convert.ToDateTime(dr["CreateTime"]); }
                if (dr["CreateBy"] != DBNull.Value) { entity.CreateBy = Convert.ToInt32(dr["CreateBy"]); }
            }
            return entity;
        }
        /// <summary>
        /// 转换数据行到实体对象
        /// </summary>
        private static void DataRowToEntity(MailAttachmentEntity entity, DataRow dr)
        {
            if (dr != null)
            {
                entity = DataRowToEntity(dr);
            }
        }
        /// <summary>
        /// 转换数据表到实体集合
        /// </summary>
        private static List<MailAttachmentEntity> DataTableToList(DataTable dt)
        {
            List<MailAttachmentEntity> list = new List<MailAttachmentEntity>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(DataRowToEntity(dr));
            }
            return list;
        }
        #endregion
    }
}
