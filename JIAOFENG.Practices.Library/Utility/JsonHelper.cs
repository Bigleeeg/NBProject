using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
namespace JIAOFENG.Practices.Library.Utility
{
    public class JsonHelper
    {
        /// <summary>
        /// 生成Json格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSON(object o)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(o.GetType());

            using (MemoryStream stream = new MemoryStream())
            {
                //JSON序列化
                serializer.WriteObject(stream, o);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
        /// <summary>
        /// 获取Json的Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObjectFromJSON<T>(string json)
        {
            object o;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                o = serializer.ReadObject(stream);
            }
            return (T)o;
        }
        /// <summary>
        /// 反回JSON数据到前台
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns>JSON字符串</returns>
        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder JsonString = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonString.Append("{ ");
                JsonString.Append("\"TableInfo\":[ ");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{ ");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("} ");
                    }
                    else
                    {
                        JsonString.Append("}, ");
                    }
                }
                JsonString.Append("]}");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
