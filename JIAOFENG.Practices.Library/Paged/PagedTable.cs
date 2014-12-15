using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Library.Common
{
    /// <summary>
    /// 分页的DataTable
    /// </summary>
    [Serializable]
    public class PagedTable : DataTable, IPaged
    {
        /// <summary>
        /// 构造函数
        /// 传入未分页的数据源的总集合，返回分页后的List列表
        /// </summary>
        public PagedTable() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataTable">未分页的DataTable的总集合</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示记录数</param>
        public PagedTable(DataTable dataTable, int pageIndex, int pageSize)
        {
            TotalItemCount = dataTable.Rows.Count;
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;

            if (string.IsNullOrWhiteSpace(dataTable.TableName))
            {
                dataTable.TableName = "t1";
            }

            MemoryStream ms = new MemoryStream();
            dataTable.WriteXmlSchema(ms, true);
            ms.Seek(0, SeekOrigin.Begin);
            this.ReadXmlSchema(ms);
            ms.Close();

            for (int i = StartRecordIndex - 1; i < EndRecordIndex; i++)
            {
                this.Rows.Add(dataTable.Rows[i].ItemArray);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataTable">分页后的DataTable</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="totalItemCount">总记录数</param>
        public PagedTable(DataTable dataTable, int pageIndex, int pageSize, int totalItemCount)
        {
            TotalItemCount = totalItemCount;
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;

            if (string.IsNullOrWhiteSpace(dataTable.TableName))
            {
                dataTable.TableName = "t1";
            }

            MemoryStream ms = new MemoryStream();
            dataTable.WriteXmlSchema(ms, true);
            ms.Seek(0, SeekOrigin.Begin);
            this.ReadXmlSchema(ms);
            ms.Close();

            foreach (DataRow dr in dataTable.Rows)
            {
                this.Rows.Add(dr.ItemArray);
            }           
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
