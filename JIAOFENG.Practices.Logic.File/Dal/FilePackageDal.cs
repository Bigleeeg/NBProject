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
    public static partial class FilePackageDal
    {
	    #region add 
		/// <summary>
		/// 插入记录
		/// </summary>
		public static FilePackage Add(FilePackage entity)
		{
            const string sql = "INSERT INTO `file_package` (`FilePackageName`,`CreateTime`,`CreateBy`,`UpdateTime`,`UpdateBy`, `IsDelete`) VALUES (@FilePackageName,@CreateTime,@CreateBy,@UpdateTime,@UpdateBy, @IsDelete);select last_insert_id();";

			List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySqlParameter>();
             sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@FilePackageName", entity.FilePackageName));
             sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@CreateTime", entity.CreateTime));
             sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@CreateBy", entity.CreateBy));
             sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@UpdateTime", entity.UpdateTime));
             sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@UpdateBy", entity.UpdateBy));
             sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@IsDelete", entity.IsDelete));

			int id = Convert.ToInt32(DatabaseFactory.CreateDatabase().ExecuteScalar(CommandType.Text, sql, sqlParameterList.ToArray()));
            if (id > 0)
            {
				entity.FilePackageID = id;
                return entity;
            }
            return null;
		}
        ///// <summary>
        ///// 批量插入记录
        ///// </summary>
        //private static void AddBatch(List<FilePackage> entities)
        //{
        //    if (entities == null || entities.Count == 0)
        //    {
        //        return;
        //    }
        //    const string sqlFormat = "INSERT INTO `file_package` (`FilePackageName`,`CreateTime`,`CreateBy`,`UpdateTime`,`UpdateBy`) VALUES ({0});";
        //    string sql = string.Empty;
        //    int index = 0;
        //    string currentParameter = string.Empty;
        //    List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySqlParameter>();
        //    foreach(FilePackage entity in entities)
        //    {
        //        string valuesParas = string.Empty;
        //     currentParameter  = "@FilePackageName" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.FilePackageName));valuesParas = valuesParas + "," + currentParameter;
        //     currentParameter  = "@CreateTime" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.CreateTime));valuesParas = valuesParas + "," + currentParameter;
        //     currentParameter  = "@CreateBy" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.CreateBy));valuesParas = valuesParas + "," + currentParameter;
        //     currentParameter  = "@UpdateTime" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.UpdateTime));valuesParas = valuesParas + "," + currentParameter;
        //     currentParameter  = "@UpdateBy" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.UpdateBy));valuesParas = valuesParas + "," + currentParameter;

        //        valuesParas = valuesParas.Substring(1);
        //        sql = sql + string.Format(sqlFormat, valuesParas);
        //        index++;
        //    }
            
        //    DatabaseFactory.CreateDatabase().ExecuteNonQuery(CommandType.Text, sql, sqlParameterList.ToArray());
        //}
        ///// <summary>
        ///// 批量插入记录
        ///// </summary>
        //public static void Add(List<FilePackage> entities)
        //{
        //    int i = 0;
        //    const int size = 419;
        //    do
        //    {
        //        List<FilePackage> contents = entities.Skip(i * size).Take(size).ToList();
        //        AddBatch(contents);
        //        i++;
        //    } while (i * size < entities.Count);

        //}
		#endregion 

		#region delete
		/// <summary>
		/// 根据主键ID字符串真删除多条数据
		/// </summary>
		public static bool Delete(string ids)
		{
			if(string.IsNullOrWhiteSpace(ids))
			{
				return true;
			}
			bool result = false;
			string sql = "update `file_package` set isdelete=1 WHERE FilePackageID IN (" + ids + ")";
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
             string sql = "update `file_package` set isdelete=1 where isdelete=0" + columns;
			 result =  DatabaseFactory.CreateDatabase().ExecuteNonQuery(CommandType.Text, sql, sqlParameterList.ToArray()) > 0 ? true : false;
	
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
		public static bool Delete(string columnName1, object columnValue1,string columnName2, object columnValue2,string columnName3, object columnValue3)
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
		public static bool Update(FilePackage entity)
		{
			bool result = false;
			string sql = "Update `file_package` set `FilePackageName` = @FilePackageName,`UpdateTime` = @UpdateTime,`UpdateBy` = @UpdateBy where `FilePackageID` = @FilePackageID";

			List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySqlParameter>();
sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@FilePackageID", entity.FilePackageID == null ? (object)DBNull.Value : entity.FilePackageID));
             sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@FilePackageName", entity.FilePackageName));
             sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@UpdateTime", entity.UpdateTime));
             sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter("@UpdateBy", entity.UpdateBy));

			result =  DatabaseFactory.CreateDatabase().ExecuteNonQuery(CommandType.Text, sql, sqlParameterList.ToArray()) > 0 ? true : false;
			return result;
		}
		private static void UpdateBatch(List<FilePackage> entities)
        {
		    if (entities == null || entities.Count == 0)
			{
				return;
			}
			const string sqlFormat = "Update `file_package` set {0} where {1};";
            string sql = string.Empty;
            int index = 0;
			string currentParameter = string.Empty;
            List<MySql.Data.MySqlClient.MySqlParameter> sqlParameterList = new List<MySqlParameter>();
            foreach(FilePackage entity in entities)
            {
                string valuesParas = string.Empty;
				string wherePara = string.Empty;
             currentParameter  = "@FilePackageID" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.FilePackageID));wherePara =  "FilePackageID=" + currentParameter;
             currentParameter  = "@FilePackageName" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.FilePackageName));valuesParas = valuesParas + ",FilePackageName=" + currentParameter;
             currentParameter  = "@UpdateTime" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.UpdateTime));valuesParas = valuesParas + ",UpdateTime=" + currentParameter;
             currentParameter  = "@UpdateBy" + "N" + index.ToString(); sqlParameterList.Add(new MySql.Data.MySqlClient.MySqlParameter(currentParameter, entity.UpdateBy));valuesParas = valuesParas + ",UpdateBy=" + currentParameter;

                valuesParas = valuesParas.Substring(1);
                sql = sql + string.Format(sqlFormat, valuesParas,wherePara);
				index++;
            }
            
            DatabaseFactory.CreateDatabase().ExecuteNonQuery(CommandType.Text, sql, sqlParameterList.ToArray());
        }
		/// <summary>
		/// 批量更新记录
		/// </summary>
		public static void Update(List<FilePackage> entities)
        {
            int i = 0;
            const int size = 349;
            do
            {
                List<FilePackage> contents = entities.Skip(i * size).Take(size).ToList();
                UpdateBatch(contents);
                i++;
            } while (i * size < entities.Count);

        }
		#endregion 

		#region search
		/// <summary>
		/// 转换数据行到实体对象
		/// </summary>
		private static FilePackage DataRowToEntity(DataRow dr)
		{
			FilePackage entity = new FilePackage();
			if (dr != null)
			{
               if (dr["FilePackageID"] != DBNull.Value) {entity.FilePackageID = Convert.ToInt32(dr["FilePackageID"]);} 
               if (dr["FilePackageName"] != DBNull.Value) {entity.FilePackageName = Convert.ToString(dr["FilePackageName"]);} 
               if (dr["CreateTime"] != DBNull.Value) {entity.CreateTime = Convert.ToDateTime(dr["CreateTime"]);} 
               if (dr["CreateBy"] != DBNull.Value) {entity.CreateBy = Convert.ToInt32(dr["CreateBy"]);} 
               if (dr["UpdateTime"] != DBNull.Value) {entity.UpdateTime = Convert.ToDateTime(dr["UpdateTime"]);} 
               if (dr["UpdateBy"] != DBNull.Value) {entity.UpdateBy = Convert.ToInt32(dr["UpdateBy"]);} 

			}
			return entity;
		}

		/// <summary>
		/// 转换数据行到实体对象
		/// </summary>
		private static void DataRowToEntity(FilePackage entity, DataRow dr)
		{
			if (dr != null)
			{
				entity = DataRowToEntity(dr);
			}
		}

		/// <summary>
		/// 转换数据表到实体集合
		/// </summary>
		private static List<FilePackage> DataTableToList(DataTable dt)
        {
            List<FilePackage> list = new List<FilePackage>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(DataRowToEntity(dr));
            }
            return list;
        }
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public static FilePackage GetEntityByID(int id)
		{			
			string sql = "select * from `file_package` where FilePackageID=" + id + " Limit 0,1";
			DataSet ds =  DatabaseFactory.CreateDatabase().ExecuteDataSet(sql);
			if (ds.Tables[0].Rows.Count > 0)
			{
				return DataRowToEntity(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public static FilePackage GetEntityByColumnValue(string columnName, object columnValue)
		{			
			Dictionary<string, object> where = new Dictionary<string, object>();
            where.Add(columnName, columnValue);
			
			List<FilePackage> list = GetList(where);
		    if (list.Count > 0)
			{
				return list[0];
			}
			return null;
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public static FilePackage GetEntityByColumnValue(string columnName1, object columnValue1, string columnName2, object columnValue2)
		{	
		    List<FilePackage> list = GetList(columnName1, columnValue1, columnName2, columnValue2);
		    if (list.Count > 0)
			{
				return list[0];
			}
			return null;
		}

		/// <summary>
		/// 得到多个对象实体
		/// </summary>
		public static List<FilePackage> GetListByIDs(string ids)
		{			
			List<FilePackage> list = new List<FilePackage>();
			if (!string.IsNullOrEmpty(ids))
			{
				string sql = "select * from `file_package` where FilePackageID in (" + ids + ")";
				DataSet ds =  DatabaseFactory.CreateDatabase().ExecuteDataSet(sql);
				if (ds.Tables[0].Rows.Count > 0)
				{
					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						list.Add(DataRowToEntity(dr));
					}
				}
			}
			return list;
		}
		/// <summary>
		/// 得到多个对象实体
		/// </summary>
		public static List<FilePackage> GetListByIDs(List<int> ids)
		{
			return GetListByIDs(string.Join(",", ids));
		}
		/// <summary>
		/// 得到多个对象实体
		/// </summary>
		public static List<FilePackage> GetListByIDs(int[] ids)
		{
			return GetListByIDs(string.Join(",", ids));
		}

		/// <summary>
		/// 得到多个对象实体
		/// </summary>
		public static List<FilePackage> GetList(string where)
		{			
			DataSet ds =  GetDataSet(where);
			return DataTableToList(ds.Tables[0]);
		}

		/// <summary>
		/// 得到多个对象实体
		/// </summary>
		public static List<FilePackage> GetListAll()
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
			 string sql = "select * from `file_package` where isdelete=0" + columns + " order by FilePackageID";
			 DataSet ds =  DatabaseFactory.CreateDatabase().ExecuteDataSet(CommandType.Text, sql, sqlParameterList.ToArray());
	
			 return ds;
		}

		/// <summary>
		/// 获取DataSet
		/// </summary>
		public static List<FilePackage> GetList(Dictionary<string, object> where)
		{		
		    DataSet ds = GetDataSet(where);				 
			return DataTableToList(ds.Tables[0]);
		}

		/// <summary>
		/// 得到多个对象实体
		/// </summary>
		public static List<FilePackage> GetList(string columnName1, object columnValue1,string columnName2, object columnValue2)
		{			
		    Dictionary<string, object> where = new Dictionary<string, object>();
            where.Add(columnName1, columnValue1);
			where.Add(columnName2, columnValue2);
			return GetList(where);
		}

		/// <summary>
		/// 得到多个对象实体
		/// </summary>
		public static List<FilePackage> GetList(string columnName, object columnValue)
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
			string sql = "select * from `file_package`" + where;
			DataSet ds =  DatabaseFactory.CreateDatabase().ExecuteDataSet(sql);
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
		public static PagedList<FilePackage> GetPagedList(string commandText, string orderBy, int pageIndex, int pageSize, params DbParameter[] commandParameters)
		{			
			PagedTable pt =  ExecutePagedTable(commandText, orderBy, pageIndex, pageSize, commandParameters);
			List<FilePackage> list = DataTableToList(pt);

			PagedList<FilePackage> pagedList = new PagedList<FilePackage>(list, pageIndex, pageSize, pt.TotalItemCount);
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
			string sql = "select count(1) from `file_package` " + where;
			object o =  DatabaseFactory.CreateDatabase().ExecuteScalar(sql);
	
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
			 string sql = "select count(1) from `file_package` where isdelete=0" + columns;
			 object o =  DatabaseFactory.CreateDatabase().ExecuteScalar(CommandType.Text, sql, sqlParameterList.ToArray());
	
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
			 string sql = "select isnull(MAX(FilePackageID),0) from `file_package` where isdelete=0" + columns;
			object o =  DatabaseFactory.CreateDatabase().ExecuteScalar(CommandType.Text, sql, sqlParameterList.ToArray());
	
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
			 string sql = "select FilePackageID from `file_package` where isdelete=0" + columns;
			 if (exceptedPKIDs != null && exceptedPKIDs.Count > 0)
			 {
				 sql += " and FilePackageID not in(" + string.Join(",", exceptedPKIDs) + ")";
			 }
			 string existsSql = string.Format(existsFormat, sql);
			 object o =  DatabaseFactory.CreateDatabase().ExecuteScalar(CommandType.Text, existsSql, sqlParameterList.ToArray());
	
			 return Convert.ToInt32(o) == 1;
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public static bool Exists(int id)
		{
			return Exists("FilePackageID", id);
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public static bool Exists(string columnName, object columnValue, int exceptedPKID)
		{
			Dictionary<string, object> where = new Dictionary<string, object>();
            where.Add(columnName, columnValue);
			List<int> exceptedPKIDs = new List<int>(){exceptedPKID};
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
			return DatabaseFactory.CreateDatabase().GetMaxID("FilePackageID", "file_package", where); 
		}
		#endregion 
    }
}
