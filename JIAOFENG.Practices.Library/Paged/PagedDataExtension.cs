using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Library.Common
{
    /// <summary>
    /// 分页数据的扩展方法
    /// </summary>
    public static class PagedDataExtension
    {
        /// <summary>
        /// IQueryable对象的分页查询扩展方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="allItems">Queryable对象</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>分页数据</returns>
        public static PagedList<T> ToPagedList<T>
            (
                this IQueryable<T> allItems,
                int pageIndex,
                int pageSize
            )
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;          
            var totalItemCount = allItems.Count();

            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);

            return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }
    }
}
