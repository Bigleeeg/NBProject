using JIAOFENG.Practices.Library.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using JIAOFENG.Practices.Library.Utility;
using System.Web;
using System.Configuration;
using Newtonsoft.Json;

namespace JIAOFENG.Practices.Library.Mvc.EasyUI
{
    public static class MvcExtension
    {

        /// <summary>
        /// 将PagedTable的数据转化为JsonResult.
        /// </summary>
        /// <param name="data">按DataTable的分页数据</param>
        /// <returns>JsonResult对象</returns>
        public static JsonResult ToJsonResult(this DataTable data)
        {
            JsonResult result = new TextJsonResult();

            StringBuilder sbRows = new StringBuilder();
            sbRows.Append("{");
            sbRows.Append("\"rows\":[");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (data != null && data.Rows != null)
            {
                foreach (DataRow row in data.Rows)
                {
                    sbRows.Append("{");
                    foreach (DataColumn column in data.Columns)
                    {
                        sbRows.AppendFormat("\"{0}\":{1},", column.ColumnName, serializer.Serialize(row[column]));
                    }
                    sbRows.TrimEnd(',');
                    sbRows.Append("},");
                }
                sbRows.TrimEnd(',');
                sbRows.Append("],");

                sbRows.AppendFormat("\"total\":{0}", (data is PagedTable) ? ((PagedTable)data).TotalItemCount : data.Rows.Count);
            }

            sbRows.Append("}");
            result.Data = sbRows;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return result;
        }
        /// <summary>
        /// 将PagedTable的数据转化为JsonResult.
        /// </summary>
        /// <param name="data">按DataTable的分页数据</param>
        /// <returns>JsonResult对象</returns>
        public static JsonResult ToJsonResultWithDateTime(this DataTable data, bool onlyDay)
        {
            JsonResult result = new TextJsonResult();

            StringBuilder sbRows = new StringBuilder();
            sbRows.Append("{");
            sbRows.Append("\"rows\":[");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (data != null && data.Rows != null)
            {
                foreach (DataRow row in data.Rows)
                {
                    sbRows.Append("{");
                    foreach (DataColumn column in data.Columns)
                    {
                        if (row[column] != DBNull.Value && column.DataType == typeof(DateTime))
                        {
                            DateTime time = DateTime.Parse(row[column] + string.Empty);
                            string dateFormat = ConfigurationManager.AppSettings["DateFormat"];
                            if (onlyDay)
                            {
                                sbRows.AppendFormat("\"{0}\":{1},", column.ColumnName, serializer.Serialize(time.ToString(dateFormat)));                               
                            }
                            else
                            {
                                sbRows.AppendFormat("\"{0}\":{1},", column.ColumnName, serializer.Serialize(time.ToString(dateFormat + " hh:mm:ss")));
                            }
                        }
                        else
                        {
                            sbRows.AppendFormat("\"{0}\":{1},", column.ColumnName, serializer.Serialize(row[column]));
                        }
                    }
                    sbRows.TrimEnd(',');
                    sbRows.Append("},");
                }
                sbRows.TrimEnd(',');
                sbRows.Append("],");

                sbRows.AppendFormat("\"total\":{0}", (data is PagedTable) ? ((PagedTable)data).TotalItemCount : data.Rows.Count);
            }

            sbRows.Append("}");
            result.Data = sbRows;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return result;
        }
        public static string ToJson<T>(this List<T> list)
        {
            var result = JsonConvert.SerializeObject(
                new { rows = list, total = list.Count }, new JsonSerializerSettings { DateFormatString = ConfigurationManager.AppSettings["DateFormat"] });
            return result;
        }
    }      
}
