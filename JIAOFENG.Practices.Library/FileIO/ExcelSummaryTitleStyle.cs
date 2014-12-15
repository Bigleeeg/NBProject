using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Library.FileIO
{
    public class ExcelSummaryTitleStyle : ExcelCellStyle
    {
        public ExcelSummaryTitleStyle()
        {
            this.Alignment = HorizontalAlignment.Right;

            //this.FontBoldweight = 600;//加粗
        }
    }
}
