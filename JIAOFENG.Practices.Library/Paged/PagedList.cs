using System;
using System.Collections.Generic;

namespace JIAOFENG.Practices.Library.Common
{
    public class PagedList<T> : List<T>, IPaged
    {
        /// <summary>
        /// PagedList构造函数
        /// 传入未分页的数据源的总集合，返回分页后的List列表
        /// </summary>
        /// <param name="items">未分页的数据源的总集合</param>
        /// <param name="pageIndex">要返回的页码</param>
        /// <param name="pageSize">每页的记录数</param>
        public PagedList(IList<T> items, int pageIndex, int pageSize)
        {
            TotalItemCount = items.Count;
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
                
            for (int i = StartRecordIndex-1 ; i < EndRecordIndex ; i++ )
            {
                Add(items[i]);
            }
        }

        /// <summary>
        /// PagedList构造函数
        /// </summary>
        /// <param name="items">分页后要返回的记录集</param>
        /// <param name="pageIndex">要返回的页码</param>
        /// <param name="pageSize">每页的记录数</param>
        /// <param name="totalItemCount">符合条件的数据源的总记录数</param>
        public PagedList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {           
            TotalItemCount = totalItemCount;
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;

            AddRange(items);
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPageIndex { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount { get { return (int)Math.Ceiling(TotalItemCount / (double)PageSize); } }

        /// <summary>
        /// 起始行
        /// </summary>
        public int StartRecordIndex { get { return (CurrentPageIndex - 1) * PageSize + 1; } }

        /// <summary>
        /// 结束行
        /// </summary>
        public int EndRecordIndex { get { return (int)(TotalItemCount > CurrentPageIndex * PageSize ? CurrentPageIndex * PageSize : TotalItemCount); } }
    }
}
