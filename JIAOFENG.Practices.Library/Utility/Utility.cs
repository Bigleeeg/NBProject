using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Xml;
using System.Web;

namespace JIAOFENG.Practices.Library.Utility
{
    public class Utility
    {

        /// <summary>
        /// 功能：判断DataSet中是否有数据
        /// 时间：1-11
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool CheckDs(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 功能：判断DataRow[]中是否有数据
        /// 时间：1-11
        /// 
        /// </summary>       
        public static bool CheckRows(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 功能：判断DataTable中是否有数据
        /// 时间：1-11
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool CheckDt(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 功能：取得当前的时间
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentTime()
        {
            DateTime dt = DateTime.Now;
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 功能：取得当前的时间
        /// </summary>
        /// <returns></returns>
        public static string GetFileName()
        {
            DateTime dt = DateTime.Now;
            return dt.ToString("yyyyMMddHHmmss");
        }
        /// <summary>
        /// 功能：设置格式
        /// </summary>
        /// <returns></returns>
        public static string SetDateFormat(object date)
        {
            return date == DBNull.Value || date == null ? "" : Convert.ToDateTime(date).ToString("yyyy-MM-dd HH:mm");
        }
        /// <summary>
        /// Function:将Object转换成Oracle日期格式
        /// </summary>       
        public static string SetOracleDateFormat(object date)
        {
            return date == DBNull.Value || date == null ? "" : "to_date('" + date + "','yyyy-mm-dd hh24:mi:ss')";
        }
        public static string SetDateFormat(object date, string format)
        {
            return date == DBNull.Value || date == null ? "" : Convert.ToDateTime(date).ToString(format);
        }

        /// <summary>
        /// 比较两个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="originalDict"></param>
        /// <param name="currentDict"></param>
        /// <param name="addList"></param>
        /// <param name="updateList"></param>
        /// <param name="deleteList"></param>
        public static void CompareDiff<T>(IDictionary<string, T> originalDict, IDictionary<string, T> currentDict, out IList<T> addList, out IList<T> updateList, out IList<T> deleteList)
        {
            addList = null;
            updateList = null;
            deleteList = null;

            if ((originalDict == null || originalDict.Count == 0) && (currentDict == null || currentDict.Count == 0))
            {
                return;
            }

            if (originalDict == null || originalDict.Count == 0)
            {
                addList = currentDict.Values.ToList();
            }

            if (currentDict == null || currentDict.Count == 0)
            {
                deleteList = originalDict.Values.ToList();
            }

            addList = new List<T>();
            updateList = new List<T>();
            deleteList = new List<T>();

            foreach (var item in originalDict)
            {
                if (currentDict.ContainsKey(item.Key))
                {
                    updateList.Add(currentDict[item.Key]);
                }
                else
                {
                    deleteList.Add(item.Value);
                }
            }

            foreach (var item in currentDict)
            {
                if (!originalDict.ContainsKey(item.Key))
                {
                    addList.Add(item.Value);
                }
            }

        }
    }
}
