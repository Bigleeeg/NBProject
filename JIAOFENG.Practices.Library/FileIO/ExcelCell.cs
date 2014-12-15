using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;

namespace JIAOFENG.Practices.Library.FileIO
{
    public class ExcelCell
    {
        public ExcelCell(object value, CellType cellType, ExcelCellStyle cellStyle)
        {
            this.Value = value;
            this.CellType = cellType;
            this.CellStyle = cellStyle;
        }
        public ExcelCell(object value, ExcelCellStyle cellStyle)
            : this(value, CellType.Unknown, cellStyle)
        {
        }
        public ExcelCell(object value, CellType cellType)
            : this(value, cellType, null)
        {
        }
        public ExcelCell(object value)
            : this(value, CellType.Unknown)
        {
        }
        public object Value;
        public CellType CellType;
        public ExcelCellStyle CellStyle;
    }
}
