using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Library.FileIO
{
    public class ExcelRegion
    {
        public ExcelRegion(int firstRow, int lastRow, int firstCol, int lastCol)
        {
            this.FirstRow = firstRow;
            this.LastRow = lastRow;
            this.FirstColumn = firstCol;
            this.LastColumn = lastCol;
        }
        public int FirstRow;
        public int LastRow;
        public int FirstColumn;
        public int LastColumn;
    }
}
