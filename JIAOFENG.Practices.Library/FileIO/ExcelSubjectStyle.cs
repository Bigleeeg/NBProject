using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Library.FileIO
{
    public class ExcelSubjectStyle : ExcelHeadStyle
    {
        public ExcelSubjectStyle()
        {
            this.FontHeightInPoints = 16;//字号
            this.FontBoldweight = 600;//加粗
        }       
    }
}
