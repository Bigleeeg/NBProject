using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace JIAOFENG.Practices.Library.FileIO
{
    public class ExcelHeadStyle : ExcelCellStyle
    {
        public ExcelHeadStyle()
        {
            this.Alignment = HorizontalAlignment.Center;

            this.FontHeightInPoints = 10;//字号
            this.FontBoldweight = 600;//加粗

            this.FillForegroundColor = null;// new HSSFColor.LightBlue();
        }
    }
}
