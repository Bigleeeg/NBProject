using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Library.FileIO
{
    public class ExcelSheet : List<ExcelRow>
    {
        public ExcelSheet()
        {
            //this.DefaultColumnWidth = 20;
            //this.DefaultRowHeight = 20;
            //this.Alignment = HorizontalAlignment.LEFT;
            //this.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;
            //this.FontHeightInPoints = 11;//字号
        }

        public string ExcelSheetName { get; set; }
        public ExcelCell ExcelSubject { get; set; }
        public ExcelSummary ExcelSummarys = new ExcelSummary();
        public List<int> RowsMerged = new List<int>();
        public List<int> ColumnsMerged = new List<int>();
        public List<ExcelRegion> RegionMerged = new List<ExcelRegion>();
        public int FreezeColumnIndex = 0;
        public int FreezeRowIndex = 0;
        //public int DefaultColumnWidth { get; set; }
        //public short DefaultRowHeight { get; set; }
        //public HorizontalAlignment Alignment { get; set; }
        //public VerticalAlignment VerticalAlignment { get; set; }
        //public short FontHeightInPoints { get; set; }
        //public short FontBoldweight { get; set; }
        //public bool HasBorder { get; set; }

        //public BorderStyle borderStyle = BorderStyle.Thin;
        //public BorderStyle BorderStyle
        //{
        //    get
        //    {
        //        return borderStyle;
        //    }
        //    set
        //    {
        //        borderStyle = value;
        //    }
        //}
        public Dictionary<int, int> ColumnsWidth = new Dictionary<int, int>();
        public Dictionary<int, int> RowsHeight = new Dictionary<int, int>();

        public bool RowStyleFirst = true;
        public Dictionary<int, ExcelCellStyle> ColumnsStyle = new Dictionary<int, ExcelCellStyle>();
        public Dictionary<int, ExcelCellStyle> RowsStyle = new Dictionary<int, ExcelCellStyle>();
        public Dictionary<ExcelRegion, ExcelCellStyle> RegionStyle = new Dictionary<ExcelRegion, ExcelCellStyle>();
        public List<string> GetColumn(int index)
        {
            List<string> ret = new List<string>();
            foreach (ExcelRow er in this)
            {
                ret.Add(er[index].Value.ToString());
            }
            return ret;
        }
        public List<string> GetRow(int index)
        {
            List<string> ret = new List<string>();
            ExcelRow er = this[index];
            foreach (ExcelCell ec in er)
            {
                ret.Add(ec.Value.ToString());
            }
            return ret;
        }
    }
}
