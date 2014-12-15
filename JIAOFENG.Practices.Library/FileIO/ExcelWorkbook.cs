using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Library.FileIO
{
    public class ExcelWorkbook : List<ExcelSheet>
    {
        public ExcelWorkbook()
        {
            this.DefaultCellStyle = new ExcelCellStyle();
        }
        public string ExcelFileName { get; set; }
        public ExcelCellStyle DefaultCellStyle { get; set; }
    }
}
