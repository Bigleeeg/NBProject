using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using JIAOFENG.Practices.Database;
using JIAOFENG.Practices.Library.Common;

namespace JIAOFENG.Practices.Logic.File
{
    public static partial class FileStorageDal
    {
        #region add
        /// <summary>
        /// 插入记录
        /// </summary>
        public static FileStorage Add(FileStorage entity)
        {
            const string sql = "INSERT INTO `file_storage` (`FileCode`, `FileName`,`ExtensionName`,`FileType`,`SaveLocationType`,`SavePath`,`Context`,`CreateTime`,`CreateBy`,`UpdateTime`,`UpdateBy`,`FilePackageID`) VALUES (@FileCode, @FileName,@ExtensionName,@FileType,@SaveLocationType,@SavePath,@Context,@CreateTime,@CreateBy,@UpdateTime,@UpdateBy,@FilePackageID);select last_insert_id();";

            List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySqlParameter>();
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@FileCode", entity.FileCode));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@FileName", entity.FileName));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@ExtensionName", entity.ExtensionName));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@FileType", entity.FileType));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@SaveLocationType", entity.SaveLocationType));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@SavePath", entity.SavePath == null ? (object)DBNull.Value : entity.SavePath));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@Context", entity.Context == null ? (object)DBNull.Value : entity.Context));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@CreateTime", entity.CreateTime));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@CreateBy", entity.CreateBy));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@UpdateTime", entity.UpdateTime));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@UpdateBy", entity.UpdateBy));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@FilePackageID", entity.FilePackageID));

            int id = Convert.ToInt32(DatabaseFactory.CreateDatabase().ExecuteScalar(CommandType.Text, sql, sqlParameterList.ToArray()));
            if (id > 0)
            {
                entity.FileStorageID = id;
                return entity;
            }
            return null;
        }
        /// <summary>
        /// 批量插入记录
        /// </summary>
        private static void AddBatch(List<FileStorage> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                return;
            }
            const string sqlFormat = "INSERT INTO `file_storage` (FileCode,`FileName`,`ExtensionName`,`FileType`,`SaveLocationType`,`SavePath`,`Context`,`CreateTime`,`CreateBy`,`UpdateTime`,`UpdateBy`,`FilePackageID`) VALUES ({0});";
            string sql = string.Empty;
            int index = 0;
            string currentParameter = string.Empty;
            List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySqlParameter>();
            foreach (FileStorage entity in entities)
            {
                string valuesParas = string.Empty;
                currentParameter = "@FileCode" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.FileCode)); valuesParas = valuesParas + "," + currentParameter;
                currentParameter = "@FileName" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.FileName)); valuesParas = valuesParas + "," + currentParameter;
                currentParameter = "@ExtensionName" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.ExtensionName)); valuesParas = valuesParas + "," + currentParameter;
                currentParameter = "@FileType" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.FileType)); valuesParas = valuesParas + "," + currentParameter;
                currentParameter = "@SaveLocationType" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.SaveLocationType)); valuesParas = valuesParas + "," + currentParameter;
                currentParameter = "@SavePath" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.SavePath == null ? (object)DBNull.Value : entity.SavePath)); valuesParas = valuesParas + "," + currentParameter;
                currentParameter = "@Context" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.Context == null ? (object)DBNull.Value : entity.Context)); valuesParas = valuesParas + "," + currentParameter;
                currentParameter = "@CreateTime" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.CreateTime)); valuesParas = valuesParas + "," + currentParameter;
                currentParameter = "@CreateBy" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.CreateBy)); valuesParas = valuesParas + "," + currentParameter;
                currentParameter = "@UpdateTime" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.UpdateTime)); valuesParas = valuesParas + "," + currentParameter;
                currentParameter = "@UpdateBy" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.UpdateBy)); valuesParas = valuesParas + "," + currentParameter;
                currentParameter = "@FilePackageID" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.FilePackageID)); valuesParas = valuesParas + "," + currentParameter;

                valuesParas = valuesParas.Substring(1);
                sql = sql + string.Format(sqlFormat, valuesParas);
                index++;
            }

            DatabaseFactory.CreateDatabase().ExecuteNonQuery(CommandType.Text, sql, sqlParameterList.ToArray());
        }
        /// <summary>
        /// 批量插入记录
        /// </summary>
        public static void Add(List<FileStorage> entities)
        {
            int i = 0;
            const int size = 190;
            do
            {
                List<FileStorage> contents = entities.Skip(i * size).Take(size).ToList();
                AddBatch(contents);
                i++;
            } while (i * size < entities.Count);

        }
        #endregion

        #region delete
        /// <summary>
        /// 根据主键ID字符串真删除多条数据
        /// </summary>
        public static bool Delete(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                return true;
            }
            bool result = false;
            string sql = "DELETE FROM `file_storage` WHERE FileStorageID IN (" + ids + ")";
            result = DatabaseFactory.CreateDatabase().ExecuteNonQuery(sql) > 0 ? true : false;
            return result;
        }
        /// <summary>
        /// 根据筛选条件真删除多条数据
        /// </summary>
        public static bool Delete(Dictionary<string, object> where)
        {
            bool result = false;
            string columns = string.Empty;
            List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySql.Data.MySqlClient.MySqlParameter>();
            foreach (string key in where.Keys)
            {
                columns += " and `" + key + "`=" + "@" + key;
                sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@" + key, where[key]));
            }
            string sql = "DELETE from `file_storage` where 1=1" + columns;
            result = DatabaseFactory.CreateDatabase().ExecuteNonQuery(CommandType.Text, sql, sqlParameterList.ToArray()) > 0 ? true : false;

            return result;
        }
        /// <summary>
        /// 根据筛选条件真删除多条数据
        /// </summary>
        public static bool Delete(string columnName, object columnValue)
        {
            Dictionary<string, object> where = new Dictionary<string, object>();
            where.Add(columnName, columnValue);
            return Delete(where);
        }
        /// <summary>
        /// 根据筛选条件真删除多条数据
        /// </summary>
        public static bool Delete(string columnName1, object columnValue1, string columnName2, object columnValue2, string columnName3, object columnValue3)
        {
            Dictionary<string, object> where = new Dictionary<string, object>();
            where.Add(columnName1, columnValue1);
            where.Add(columnName2, columnValue2);
            where.Add(columnName3, columnValue3);
            return Delete(where);
        }
        /// <summary>
        /// 根据主键ID真删除一条数据
        /// </summary>
        public static bool Delete(int id)
        {
            return Delete(id.ToString());
        }
        /// <summary>
        /// 根据主键ID集合真删除多条数据
        /// </summary>
        public static bool Delete(List<int> ids)
        {
            return Delete(string.Join(",", ids));
        }
        /// <summary>
        /// 根据主键ID集合真删除多条数据
        /// </summary>
        public static bool Delete(int[] ids)
        {
            return Delete(string.Join(",", ids));
        }
        #endregion

        #region update
        /// <summary>
        /// 更新记录
        /// </summary>
        public static bool Update(FileStorage entity)
        {
            bool result = false;
            string sql = "Update `file_storage` set `FileName` = @FileName,`ExtensionName` = @ExtensionName,`FileType` = @FileType,`SaveLocationType` = @SaveLocationType,`SavePath` = @SavePath,`Context` = @Context,`UpdateTime` = @UpdateTime,`UpdateBy` = @UpdateBy,`FilePackageID` = @FilePackageID where `FileStorageID` = @FileStorageID";

            List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySqlParameter>();
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@FileStorageID", entity.FileStorageID));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@FileName", entity.FileName));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@ExtensionName", entity.ExtensionName));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@FileType", entity.FileType));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@SaveLocationType", entity.SaveLocationType));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@SavePath", entity.SavePath == null ? (object)DBNull.Value : entity.SavePath));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@Context", entity.Context == null ? (object)DBNull.Value : entity.Context));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@UpdateTime", entity.UpdateTime));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@UpdateBy", entity.UpdateBy));
            sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@FilePackageID", entity.FilePackageID));

            result = DatabaseFactory.CreateDatabase().ExecuteNonQuery(CommandType.Text, sql, sqlParameterList.ToArray()) > 0 ? true : false;
            return result;
        }
        private static void UpdateBatch(List<FileStorage> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                return;
            }
            const string sqlFormat = "Update `file_storage` set {0} where {1};";
            string sql = string.Empty;
            int index = 0;
            string currentParameter = string.Empty;
            List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySqlParameter>();
            foreach (FileStorage entity in entities)
            {
                string valuesParas = string.Empty;
                string wherePara = string.Empty;
                currentParameter = "@FileStorageID" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.FileStorageID)); wherePara = "FileStorageID=" + currentParameter;
                currentParameter = "@FileName" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.FileName)); valuesParas = valuesParas + ",FileName=" + currentParameter;
                currentParameter = "@ExtensionName" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.ExtensionName)); valuesParas = valuesParas + ",ExtensionName=" + currentParameter;
                currentParameter = "@FileType" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.FileType)); valuesParas = valuesParas + ",FileType=" + currentParameter;
                currentParameter = "@SaveLocationType" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.SaveLocationType)); valuesParas = valuesParas + ",SaveLocationType=" + currentParameter;
                currentParameter = "@SavePath" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.SavePath == null ? (object)DBNull.Value : entity.SavePath)); valuesParas = valuesParas + ",SavePath=" + currentParameter;
                currentParameter = "@Context" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.Context == null ? (object)DBNull.Value : entity.Context)); valuesParas = valuesParas + ",Context=" + currentParameter;
                currentParameter = "@UpdateTime" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.UpdateTime)); valuesParas = valuesParas + ",UpdateTime=" + currentParameter;
                currentParameter = "@UpdateBy" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.UpdateBy)); valuesParas = valuesParas + ",UpdateBy=" + currentParameter;
                currentParameter = "@FilePackageID" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.FilePackageID)); valuesParas = valuesParas + ",FilePackageID=" + currentParameter;

                valuesParas = valuesParas.Substring(1);
                sql = sql + string.Format(sqlFormat, valuesParas, wherePara);
                index++;
            }

            DatabaseFactory.CreateDatabase().ExecuteNonQuery(CommandType.Text, sql, sqlParameterList.ToArray());
        }
        /// <summary>
        /// 批量更新记录
        /// </summary>
        public static void Update(List<FileStorage> entities)
        {
            int i = 0;
            const int size = 174;
            do
            {
                List<FileStorage> contents = entities.Skip(i * size).Take(size).ToList();
                UpdateBatch(contents);
                i++;
            } while (i * size < entities.Count);

        }
        #endregion

        #region search
        /// <summary>
        /// 转换数据行到实体对象
        /// </summary>
        private static FileStorage DataRowToEntity(DataRow dr)
        {
            FileStorage entity;
            ESaveLocationType saveLocationType = (ESaveLocationType)Convert.ToInt32(dr["SaveLocationType"]);
            switch (saveLocationType)
            {
                case ESaveLocationType.WebFolder:
                    entity = new LocalFolderFile();

                    break;
                case ESaveLocationType.FileServer:
                    entity = new FileServerFile();
                    break;
                case ESaveLocationType.LocalDB:
                    entity = new LocalDBFile();
                    break;
                case ESaveLocationType.MongoDB:
                    entity = new MongoDBFile();
                    break;
                default:
                    entity = new LocalDBFile();
                    break;
            }
            if (dr != null)
            {
                if (dr["FileStorageID"] != DBNull.Value) { entity.FileStorageID = Convert.ToInt32(dr["FileStorageID"]); }
                if (dr["FileCode"] != DBNull.Value) { entity.FileName = Convert.ToString(dr["FileCode"]); }
                if (dr["FileName"] != DBNull.Value) { entity.FileName = Convert.ToString(dr["FileName"]); }
                if (dr["ExtensionName"] != DBNull.Value) { entity.ExtensionName = Convert.ToString(dr["ExtensionName"]); }
                if (dr["FileType"] != DBNull.Value) { entity.FileType = Convert.ToString(dr["FileType"]); }
                if (dr["SaveLocationType"] != DBNull.Value) { entity.SaveLocationType = (ESaveLocationType)Convert.ToInt32(dr["SaveLocationType"]); }
                if (dr["SavePath"] != DBNull.Value) { entity.SavePath = Convert.ToString(dr["SavePath"]); }
                //if (dr["Context"] != DBNull.Value) {entity.Context = byte[](dr["Context"]);} 
                if (dr["CreateTime"] != DBNull.Value) { entity.CreateTime = Convert.ToDateTime(dr["CreateTime"]); }
                if (dr["CreateBy"] != DBNull.Value) { entity.CreateBy = Convert.ToInt32(dr["CreateBy"]); }
                if (dr["UpdateTime"] != DBNull.Value) { entity.UpdateTime = Convert.ToDateTime(dr["UpdateTime"]); }
                if (dr["UpdateBy"] != DBNull.Value) { entity.UpdateBy = Convert.ToInt32(dr["UpdateBy"]); }
                if (dr["FilePackageID"] != DBNull.Value) { entity.FilePackageID = Convert.ToInt32(dr["FilePackageID"]); }

            }
            return entity;
        }
        private static List<FileStorage> DataReaderToList(System.Data.Common.DbDataReader reader)
        {
            List<FileStorage> list = new List<FileStorage>();
            while (reader.Read())
            {
                FileStorage entity;
                ESaveLocationType saveLocationType = (ESaveLocationType)Convert.ToInt32(reader["SaveLocationType"]);
                switch (saveLocationType)
                {
                    case ESaveLocationType.WebFolder:
                        entity = new LocalFolderFile();

                        break;
                    case ESaveLocationType.FileServer:
                        entity = new FileServerFile();
                        break;
                    case ESaveLocationType.LocalDB:
                        entity = new LocalDBFile();
                        break;
                    case ESaveLocationType.MongoDB:
                        entity = new MongoDBFile();
                        break;
                    default:
                        entity = new LocalDBFile();
                        break;
                }
                if (reader["FileStorageID"] != DBNull.Value) { entity.FileStorageID = Convert.ToInt32(reader["FileStorageID"]); }
                if (reader["FileCode"] != DBNull.Value) { entity.FileCode = Convert.ToString(reader["FileCode"]); }
                if (reader["FileName"] != DBNull.Value) { entity.FileName = Convert.ToString(reader["FileName"]); }
                if (reader["ExtensionName"] != DBNull.Value) { entity.ExtensionName = Convert.ToString(reader["ExtensionName"]); }
                if (reader["FileType"] != DBNull.Value) { entity.FileType = Convert.ToString(reader["FileType"]); }
                if (reader["SaveLocationType"] != DBNull.Value) { entity.SaveLocationType = (ESaveLocationType)Convert.ToInt32(reader["SaveLocationType"]); }
                if (reader["SavePath"] != DBNull.Value) { entity.SavePath = Convert.ToString(reader["SavePath"]); }
                if (reader["Context"] != DBNull.Value)
                {
                    long len = reader.GetBytes(7, 0, null, 0, 0);
                    byte[] buffer = new byte[len];
                    if (len > 0)
                    {
                        len = reader.GetBytes(7, 0, buffer, 0, (int)len);
                        entity.Context = buffer;
                    }
                }
                //if (reader["Context"] != DBNull.Value) {entity.Context = byte[](reader["Context"]);} 
                if (reader["CreateTime"] != DBNull.Value) { entity.CreateTime = Convert.ToDateTime(reader["CreateTime"]); }
                if (reader["CreateBy"] != DBNull.Value) { entity.CreateBy = Convert.ToInt32(reader["CreateBy"]); }
                if (reader["UpdateTime"] != DBNull.Value) { entity.UpdateTime = Convert.ToDateTime(reader["UpdateTime"]); }
                if (reader["UpdateBy"] != DBNull.Value) { entity.UpdateBy = Convert.ToInt32(reader["UpdateBy"]); }
                if (reader["FilePackageID"] != DBNull.Value) { entity.FilePackageID = Convert.ToInt32(reader["FilePackageID"]); }

                list.Add(entity);
            }
            return list;
        }

        /// <summary>
        /// 转换数据行到实体对象
        /// </summary>
        private static void DataRowToEntity(FileStorage entity, DataRow dr)
        {
            if (dr != null)
            {
                entity = DataRowToEntity(dr);
            }
        }

        /// <summary>
        /// 转换数据表到实体集合
        /// </summary>
        private static List<FileStorage> DataTableToList(DataTable dt)
        {
            List<FileStorage> list = new List<FileStorage>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(DataRowToEntity(dr));
            }
            return list;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public static FileStorage GetEntityByID(int id)
        {
            string sql = "select * from `file_storage` where FileStorageID=" + id + " Limit 0,1";
            //DataSet ds =  DatabaseFactory.CreateDatabase().ExecuteDataSet(sql);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    return DataRowToEntity(ds.Tables[0].Rows[0]);
            //}
            //else
            //{
            //    return null;
            //}

            using (DbDataReader reader = DatabaseFactory.CreateDatabase().ExecuteReader(sql))
            {
                return DataReaderToList(reader)[0];
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public static FileStorage GetEntityByColumnValue(string columnName, object columnValue)
        {
            Dictionary<string, object> where = new Dictionary<string, object>();
            where.Add(columnName, columnValue);

            List<FileStorage> list = GetList(where);
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public static FileStorage GetEntityByColumnValue(string columnName1, object columnValue1, string columnName2, object columnValue2)
        {
            List<FileStorage> list = GetList(columnName1, columnValue1, columnName2, columnValue2);
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        /// <summary>
        /// 得到多个对象实体
        /// </summary>
        public static List<FileStorage> GetListByIDs(string ids)
        {
            List<FileStorage> list = new List<FileStorage>();
            if (!string.IsNullOrEmpty(ids))
            {
                string sql = "select * from `file_storage` where FileStorageID in (" + ids + ")";
                using (System.Data.Common.DbDataReader reader = DatabaseFactory.CreateDatabase().ExecuteReader(sql))
                {
                    list = DataReaderToList(reader);
                }
            }
            return list;
        }
        /// <summary>
        /// 得到多个对象实体
        /// </summary>
        public static List<FileStorage> GetListByIDs(List<int> ids)
        {
            return GetListByIDs(string.Join(",", ids));
        }
        /// <summary>
        /// 得到多个对象实体
        /// </summary>
        public static List<FileStorage> GetListByIDs(int[] ids)
        {
            return GetListByIDs(string.Join(",", ids));
        }

        /// <summary>
        /// 得到多个对象实体
        /// </summary>
        public static List<FileStorage> GetList(string where)
        {
            DataSet ds = GetDataSet(where);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 得到多个对象实体
        /// </summary>
        public static List<FileStorage> GetListAll()
        {
            Dictionary<string, object> where = new Dictionary<string, object>();

            return GetList(where);
        }

        /// <summary>
        /// 获取DataSet
        /// </summary>
        public static DataSet GetDataSet(Dictionary<string, object> where)
        {
            string columns = string.Empty;
            List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySql.Data.MySqlClient.MySqlParameter>();
            foreach (string key in where.Keys)
            {
                columns += " and `" + key + "`=" + "@" + key;
                sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@" + key, where[key]));
            }
            string sql = "select * from `file_storage` where 1=1" + columns + " order by FileStorageID";
            DataSet ds = DatabaseFactory.CreateDatabase().ExecuteDataSet(CommandType.Text, sql, sqlParameterList.ToArray());

            return ds;
        }
        public static List<FileStorage> GetList(Dictionary<string, object> where)
        {
            string columns = string.Empty;
            List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySql.Data.MySqlClient.MySqlParameter>();
            foreach (string key in where.Keys)
            {
                columns += " and `" + key + "`=" + "@" + key;
                sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@" + key, where[key]));
            }
            string sql = "select * from `file_storage` where 1=1" + columns + " order by FileStorageID";
            using (DbDataReader reader = DatabaseFactory.CreateDatabase().ExecuteReader(CommandType.Text, sql, sqlParameterList.ToArray()))
            {
                return DataReaderToList(reader);
            }
        }

        /// <summary>
        /// 得到多个对象实体
        /// </summary>
        public static List<FileStorage> GetList(string columnName1, object columnValue1, string columnName2, object columnValue2)
        {
            Dictionary<string, object> where = new Dictionary<string, object>();
            where.Add(columnName1, columnValue1);
            where.Add(columnName2, columnValue2);
            return GetList(where);
        }

        /// <summary>
        /// 得到多个对象实体
        /// </summary>
        public static List<FileStorage> GetList(string columnName, object columnValue)
        {
            Dictionary<string, object> where = new Dictionary<string, object>();
            where.Add(columnName, columnValue);

            return GetList(where);
        }

        /// <summary>
        /// 得到多个对象实体
        /// </summary>
        public static DataSet GetDataSet(string where)
        {
            if (where == null) where = "";
            if (where.Trim() != "")
            {
                where = " where " + where;
            }
            string sql = "select * from `file_storage`" + where;
            DataSet ds = DatabaseFactory.CreateDatabase().ExecuteDataSet(sql);
            return ds;
        }

        /// <summary>
        /// 得到多个对象实体
        /// </summary>
        public static DataSet GetDataSetAll()
        {
            string where = "";
            return GetDataSet(where);
        }

        /// <summary>
        /// 执行分页数据查询
        /// </summary>
        /// <param name="commandText">sql查询语句</param>
        /// <param name="orderBy">sql语句的排序列</param>
        /// <param name="pageIndex">显示第几页数据</param>
        /// <param name="pageSize">每页记录树</param>
        /// <param name="commandParameters">sql语句的参数</param>
        /// <returns>分页后的table</returns>
        public static PagedTable ExecutePagedTable(string commandText, string orderBy, int pageIndex, int pageSize, params DbParameter[] commandParameters)
        {
            return DatabaseFactory.CreateDatabase().ExecutePagedTable(commandText, orderBy, pageIndex, pageSize, commandParameters);
        }

        /// <summary>
        /// 得到多个对象实体
        /// </summary>
        public static PagedList<FileStorage> GetPagedList(string commandText, string orderBy, int pageIndex, int pageSize, params DbParameter[] commandParameters)
        {
            PagedTable pt = ExecutePagedTable(commandText, orderBy, pageIndex, pageSize, commandParameters);
            List<FileStorage> list = DataTableToList(pt);

            PagedList<FileStorage> pagedList = new PagedList<FileStorage>(list, pageIndex, pageSize, pt.TotalItemCount);
            return pagedList;
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public static int GetRecordCount(string where)
        {
            if (where == null) where = "";
            if (where.Trim() != "")
            {
                where = " where " + where;
            }
            string sql = "select count(1) from `file_storage` " + where;
            object o = DatabaseFactory.CreateDatabase().ExecuteScalar(sql);

            return Convert.ToInt32(o);
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public static int GetRecordCount(Dictionary<string, object> where)
        {
            string columns = string.Empty;
            List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySql.Data.MySqlClient.MySqlParameter>();
            foreach (string key in where.Keys)
            {
                columns += " and `" + key + "`=" + "@" + key;
                sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@" + key, where[key]));
            }
            string sql = "select count(1) from `file_storage` where 1=1" + columns;
            object o = DatabaseFactory.CreateDatabase().ExecuteScalar(CommandType.Text, sql, sqlParameterList.ToArray());

            return Convert.ToInt32(o);
        }

        public static int GetPKId(Dictionary<string, object> where)
        {
            string columns = string.Empty;
            List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySql.Data.MySqlClient.MySqlParameter>();
            foreach (string key in where.Keys)
            {
                columns += " and `" + key + "`=" + "@" + key;
                sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@" + key, where[key]));
            }
            string sql = "select isnull(MAX(FileStorageID),0) from `file_storage` where 1=1" + columns;
            object o = DatabaseFactory.CreateDatabase().ExecuteScalar(CommandType.Text, sql, sqlParameterList.ToArray());

            return Convert.ToInt32(o);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public static bool Exists(Dictionary<string, object> where, List<int> exceptedPKIDs = null)
        {
            string columns = string.Empty;
            List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySql.Data.MySqlClient.MySqlParameter>();
            foreach (string key in where.Keys)
            {
                columns += " and `" + key + "`=" + "@" + key;
                sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@" + key, where[key]));
            }
            const string existsFormat = "select exists({0}) as existed";
            string sql = "select FileStorageID from `file_storage` where 1=1" + columns;
            if (exceptedPKIDs != null && exceptedPKIDs.Count > 0)
            {
                sql += " and FileStorageID not in(" + string.Join(",", exceptedPKIDs) + ")";
            }
            string existsSql = string.Format(existsFormat, sql);
            object o = DatabaseFactory.CreateDatabase().ExecuteScalar(CommandType.Text, existsSql, sqlParameterList.ToArray());

            return Convert.ToInt32(o) == 1;
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public static bool Exists(int id)
        {
            return Exists("FileStorageID", id);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public static bool Exists(string columnName, object columnValue, int exceptedPKID)
        {
            Dictionary<string, object> where = new Dictionary<string, object>();
            where.Add(columnName, columnValue);
            List<int> exceptedPKIDs = new List<int>() { exceptedPKID };
            return Exists(where, exceptedPKIDs);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public static bool Exists(string columnName, object columnValue, List<int> exceptedPKIDs = null)
        {
            Dictionary<string, object> where = new Dictionary<string, object>();
            where.Add(columnName, columnValue);
            return Exists(where, exceptedPKIDs);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public static bool Exists(string columnName1, object columnValue1, string columnName2, object columnValue2, List<int> exceptedPKIDs = null)
        {
            Dictionary<string, object> where = new Dictionary<string, object>();
            where.Add(columnName1, columnValue1);
            where.Add(columnName2, columnValue2);
            return Exists(where, exceptedPKIDs);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public static bool Exists(string columnName1, object columnValue1, string columnName2, object columnValue2, string columnName3, object columnValue3, List<int> exceptedPKIDs = null)
        {
            Dictionary<string, object> where = new Dictionary<string, object>();
            where.Add(columnName1, columnValue1);
            where.Add(columnName2, columnValue2);
            where.Add(columnName3, columnValue3);
            return Exists(where, exceptedPKIDs);
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public static int GetMaxId(string where)
        {
            return DatabaseFactory.CreateDatabase().GetMaxID("FileStorageID", "file_storage", where);
        }
        #endregion
    }
}
