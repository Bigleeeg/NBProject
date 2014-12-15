using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Configuration;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;

namespace JIAOFENG.Practices.Library.FileIO
{
    /// <summary>
    /// Read excel data by OLE.net
    /// </summary>
    public class ExcelReader
    {
        //读取一个Sheet中的数据
        public static DataTable GetSheetOfExcelData(string filePath, string sheetname, string headerColumn = "YES", int mixed = 1)
        {
            OleDbConnection conn;
            System.Data.DataTable tblSchema = GetExcelSchema(filePath, out conn, headerColumn, mixed);//存放Excel的结构
            return GetOneSheetContent(conn, tblSchema, sheetname);
        }
        //读取所有的Sheet数据
        public static DataSet GetExcelData(string filePath, string headerColumn = "YES", int mixed = 1)
        {
            #region Get Data from Excel
            OleDbConnection conn;
            System.Data.DataTable tblSchema = GetExcelSchema(filePath, out conn, headerColumn, mixed);//存放Excel的结构
            return GetAllSheetContent(conn, tblSchema);
            #endregion
        }
        //读取Excel的结构 比如sheet的数量
        private static DataTable GetExcelSchema(string filepath, out OleDbConnection conn, string ifFirst = "YES", int i = 1)
        {
            // 连接字符串
            string connStrKey = ConfigurationManager.AppSettings["CurrentExcelVersion"];
            string connStr = ConfigurationManager.AppSettings[connStrKey];
            connStr = string.Format(connStr, ifFirst, i, filepath);
            // 初始化连接，并打开
            conn = new OleDbConnection(connStr);
            conn.Open();
            //获取数据源的表定义元数据
            DataTable tblSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            conn.Close();
            return tblSchema;
        }
        //读取所有的Sheet的数据 Fill到DataSet中
        private static DataSet GetAllSheetContent(OleDbConnection conn, System.Data.DataTable tblSchema)
        {
            IList<string> tblNames = new List<string>();
            foreach (DataRow row in tblSchema.Rows)
            {
                string tableName = row["TABLE_NAME"].ToString();
                if (!tableName.StartsWith("_") && (string.IsNullOrEmpty(tableName.Split('$')[1]) || tableName.Split('$')[1] == "'"))
                {//在这里进行判断是因为读取时，有时候会有系统自定义的表名
                    tblNames.Add((string)row["TABLE_NAME"]); // 读取Sheet名
                }
            }
            OleDbDataAdapter da = new OleDbDataAdapter();
            DataSet ds = new DataSet();
            string sql_F = "SELECT * FROM [{0}]";
            foreach (string tblName in tblNames)
            {
                da.SelectCommand = new OleDbCommand(String.Format(sql_F, tblName), conn);
                try
                {
                    da.Fill(ds, tblName);
                }
                catch
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    throw;
                }
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            return ds;
        }
        private static DataTable GetOneSheetContent(OleDbConnection conn, System.Data.DataTable tblSchema, string sheetname)
        {
            DataSet ds = GetAllSheetContent(conn, tblSchema);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(sheetname))
                {
                    int isnum;
                    if (int.TryParse(sheetname.Substring(0, 1), out isnum))
                    {
                        sheetname = "'" + sheetname + "$'";
                    }
                    else
                    {
                        sheetname = sheetname + "$";
                    }
                }
                dt = ds.Tables[sheetname];
            }
            return dt;
        }
        public static DataSet GetExcelDataByNPOI(Stream s)
        {
            return NPOIExcel.GetExcelData(s);
        }
        //public static DataSet Get(Stream s)
        //{
        //    Microsoft.Office.Interop.Excel.Workbook workbook = new Microsoft.Office.Interop.Excel.Workbook(s);
        //    Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Worksheets[0];
        //}
    }
}
