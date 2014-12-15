using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace JIAOFENG.Practices.Library.FileIO
{
    public class ExcelCellBorderStyle : ExcelCellStyle
    {
        public ExcelCellBorderStyle()
        {
            this.BorderTop = BorderStyle.Thin;
            this.BorderBottom = BorderStyle.Thin;
            this.BorderLeft = BorderStyle.Thin;
            this.BorderRight = BorderStyle.Thin;
        }
    }
}
