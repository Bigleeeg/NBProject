using JIAOFENG.Practices.Library.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace JIAOFENG.Practices.Library.Common
{
    public static class CommonExtension
    {
        public static void CopyEntityExtInfoTo
            (
                this EntityExtInfo entityOriginal,
                EntityExtInfo entityTarget
            )
        {
            entityTarget.CreateBy = entityOriginal.CreateBy;
            entityTarget.CreateTime = entityOriginal.CreateTime;
            entityTarget.UpdateBy = entityOriginal.UpdateBy;
            entityTarget.UpdateTime = entityOriginal.UpdateTime;
        }

        public static void CopyEntityExtInfoFrom
            (
                this EntityExtInfo entityTarget,
                EntityExtInfo entityOriginal
            )
        {
            entityTarget.CreateBy = entityOriginal.CreateBy;
            entityTarget.CreateTime = entityOriginal.CreateTime;
            entityTarget.UpdateBy = entityOriginal.UpdateBy;
            entityTarget.UpdateTime = entityOriginal.UpdateTime;
        }

        /// <summary>
        /// 获取类型的CustomAttribute
        /// </summary>
        /// <typeparam name="T">要获取的Attribute类型</typeparam>
        /// <param name="type">目标类型</param>
        /// <param name="inherit">是否采用继承方式查找</param>
        /// <returns>当前类型上的T类型的Attribute实例</returns>
        public static T GetCustomAttribute<T>(this Type type, bool inherit = false) where T : Attribute
        {
            if (type.IsDefined(typeof(T), true))
            {
                return (T)type.GetCustomAttributes(typeof(T), inherit)[0];
            }
            return default(T);
        }

        public static PagedTable ToPagedTable(this DataTable dataTable, int pageIndex, int pageSize)
        {
            PagedTable pagedTable = new PagedTable(dataTable, pageIndex, pageSize);
            return pagedTable;
        }
        public static string GetQuery(this Uri url, string keyName)
        {
            string baseUrl;
            NameValueCollection nvc;
            UrlOper.ParseUrl(url.AbsoluteUri, out baseUrl, out nvc);
            return nvc[keyName];
        }
        public static void CompareDiff<T>(this IList<T> currentList, IList<T> originalList, out IList<T> addList, out IList<T> updateList, out IList<T> deleteList) where T : IPrimaryKey
        {
            addList = new List<T>();
            updateList = new List<T>();
            deleteList = new List<T>();

            if ((currentList == null || currentList.Count == 0) && (originalList == null || originalList.Count == 0))
            {
                return;
            }

            if (originalList == null || originalList.Count == 0)
            {
                addList = currentList;
            }

            if (currentList == null || currentList.Count == 0)
            {
                deleteList = originalList;
            }

            //var oldDic = originalList.ToDictionary(item => item.ID);
            //var currentDic = currentList.ToDictionary(item => item.ID);

            //foreach (var item in oldDic)
            //{
            //    if (currentDic.ContainsKey(item.Key))
            //    {
            //        updateList.Add(currentDic[item.Key]);
            //    }
            //    else
            //    {
            //        deleteList.Add(item.Value);
            //    }
            //}

            //foreach (var item in currentDic)
            //{
            //    if (!oldDic.ContainsKey(item.Key))
            //    {
            //        addList.Add(item.Value);
            //    }
            //}
            Dictionary<int, IGrouping<int, T>> oldDic = originalList.GroupBy(m => m.ID).ToDictionary(m => m.Key);
            Dictionary<int, IGrouping<int, T>> currentDic = currentList.GroupBy(m => m.ID).ToDictionary(m => m.Key);

            foreach (var item in oldDic)
            {
                if (currentDic.ContainsKey(item.Key))
                {
                    foreach (T t in currentDic[item.Key].ToList())
                    {
                        updateList.Add(t);
                    }
                    
                }
                else
                {
                    foreach (T t in oldDic[item.Key].ToList())
                    {
                        deleteList.Add(t);
                    }
                }
            }

            foreach (var item in currentDic)
            {
                if (!oldDic.ContainsKey(item.Key))
                {
                    foreach (T t in currentDic[item.Key].ToList())
                    {
                        addList.Add(t);
                    }
                }
            }
        }
        public static string ToXML(this object o)
        {
            return XmlHelper.SerializeToXml(o);
        }
        public static T ToObject<T>(this string xml)
        {

            return XmlHelper.SerializeToObject<T>(xml);
        }
    }


}
