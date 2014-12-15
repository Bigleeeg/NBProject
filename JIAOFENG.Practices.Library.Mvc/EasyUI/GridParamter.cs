using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Library.Mvc.EasyUI
{
    public class GridParamter
    {
        private int _page;
        /// <summary>
        /// 当前页
        /// </summary>
        public int page
        {
            set
            {
                _page = value;
            }
            get
            {
                return _page == 0 ? 1 : _page;
            }
        }

        private int _rows;
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        public int rows
        {
            set
            {
                _rows = value;
            }
            get
            {
                return _rows == 0 ? 10 : _rows;
            }
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string sort { set; get; }


        /// <summary>
        /// 排序方式 desc asc
        /// </summary>
        public string order { set; get; }
    }
}
