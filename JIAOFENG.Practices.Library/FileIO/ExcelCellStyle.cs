using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.XSSF.UserModel;
namespace JIAOFENG.Practices.Library.FileIO
{
    public class ExcelCellStyle
    {
        public ExcelCellStyle()
        {
            this.FontHeightInPoints = 8;//字号
            this.FillForegroundColor = null;// new HSSFColor.Blue();
            this.DataFormatInfo = "";
            this.BorderTop = BorderStyle.None;
            this.BorderBottom = BorderStyle.None;
            this.BorderLeft = BorderStyle.None;
            this.BorderRight = BorderStyle.None;

            this.DataFormat = ExcelCellDataFormat.Text;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            ExcelCellStyle style = (ExcelCellStyle)obj;
            return this.Alignment == style.Alignment && this.VerticalAlignment == style.VerticalAlignment && this.BorderBottom == style.BorderBottom && this.BorderLeft == style.BorderLeft
                && this.BorderRight == style.BorderRight && this.BorderTop == style.BorderTop && ((this.FillForegroundColor == null && style.FillForegroundColor == null) || (this.FillForegroundColor != null && style.FillForegroundColor != null && this.FillForegroundColor.Indexed == style.FillForegroundColor.Indexed)) && this.DataFormat == style.DataFormat
                && this.DataFormatInfo == style.DataFormatInfo && this.Indention == style.Indention && this.BorderStyle == style.BorderStyle && this.WrapText == style.WrapText
                && this.FontName == style.FontName && ((this.FontColor == null && style.FontColor == null) || (this.FontColor != null && style.FontColor != null && this.FontColor == style.FontColor)) && this.FontHeightInPoints == style.FontHeightInPoints && this.FontBoldweight == style.FontBoldweight;
        }
        public HorizontalAlignment Alignment { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }

        public BorderStyle BorderBottom { get; set; }
        public BorderStyle BorderLeft { get; set; }
        public BorderStyle BorderRight { get; set; }
        public BorderStyle BorderTop { get; set; }

        public XSSFColor FillForegroundColor { get; set; }
        //
        //public HSSFColor FillForegroundColor { get; set; }
        //
        //public FillPatternType FillPattern { get; set; }
        //
        public ExcelCellDataFormat DataFormat { get; set; }
        public string DataFormatInfo { get; set; }
        public short Indention { get; set; }
        //
        //public bool HasLeftBorder { get; set; }
        //public bool HasRightBorder { get; set; }
        //public bool HasTopBorder { get; set; }
        //public bool HasBottomBorder { get; set; }
        public BorderStyle borderStyle = BorderStyle.Thin;
        public BorderStyle BorderStyle
        {
            get
            {
                return borderStyle;
            }
            set
            {
                borderStyle = value;
            }
        }
        //public bool IsHidden { get; set; }
        //
        //public bool IsLocked { get; set; }
        //
        //public HSSFColor LeftBorderColor { get; set; }
        ////
        //public HSSFColor RightBorderColor { get; set; }
        //public HSSFColor TopBorderColor { get; set; }
        //public HSSFColor BottomBorderColor { get; set; }
        //
        //public short Rotation { get; set; }
        //public bool ShrinkToFit { get; set; }
        //

        //public short HeightInPoints { get; set; }
        
        //
        public bool WrapText { get; set; }
        private string fontName = "Arial";
        public string FontName 
        { 
            get
            {
                return fontName;
            }
            set
            {
                fontName = value;
            }
        }
        public XSSFColor FontColor { get; set; }
        public short FontHeightInPoints { get; set; }

        public short FontBoldweight { get; set; }
    }
}
