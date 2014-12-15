using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace JIAOFENG.Practices.Library.FileIO
{
    public class FileExporter
    {
        #region 导出到csv格式的文件中
        /// <summary>
        /// DataTable导出到csv格式的文件中 
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <param name="dataTable">数据表</param>
        public static void ExportCSV(string filename, DataTable dataTable)
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException("dataTable");
            }
            StringBuilder sb = ConvertDataTableToStringBuilder(dataTable);
            ExportCSV(filename, sb);
        }
        public static void ExportCSV(string filename, StringBuilder sb)
        {
            ExportCSV(filename, sb.ToString());
        }
        public static void ExportCSV(string filename, string str)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename");
            }
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ".csv");
            HttpContext.Current.Response.Charset = "gb2312";
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(str);
            HttpContext.Current.Response.End();
        }
        private static StringBuilder ConvertDataTableToStringBuilder(System.Data.DataTable tb)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataColumn column in tb.Columns)
            {
                string columnName = column.ColumnName.Trim().Replace("\n", " ");
                if (columnName.IndexOf(',') >= 0 && !(columnName.StartsWith("\"") && columnName.EndsWith("\"")))
                {
                    columnName = "\"" + columnName + "\"";
                }

                sb.Append(columnName + ",");
            }
            sb.Append("\n");

            foreach (DataRow row in tb.Rows)
            {
                foreach (DataColumn column in tb.Columns)
                {
                    string content = row[column].ToString().Trim().Replace("\n", " ");
                    if (content.IndexOf(',') >= 0 && !(content.StartsWith("\"") && content.EndsWith("\"")))
                    {
                        content = "\"" + content + "\"";
                    }
                    sb.Append(content + ",");
                }
                sb.Append("\n");
            }
            return sb;
        }
        #endregion

        #region 导出到excel格式的文件中
        public static void ExportEXCEL(string filename, MemoryStream ms)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ".xlsx");
            HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Filter.Close();
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.Flush();
            ms.Close();
            ms.Dispose();
            HttpContext.Current.Response.End();
        }
        public static void ExportEXCEL(string filename, string filePath)
        {
            ExportEXCEL(filename, new JIAOFENG.Practices.Library.FileIO.FileOperator(filePath).ConvertToStream());
        }
        public static void ExportEXCEL(string filename, DataTable sourceTable)
        {
            ExportEXCEL(filename, NPOIExcel.RenderDataTableToExcelStream(sourceTable));
        }
        public static void ExportEXCEL(string filename, DataSet sourceDataSet)
        {
            ExportEXCEL(filename, NPOIExcel.RenderDataTableToExcelStream(sourceDataSet));
        }
        public static void ExportEXCEL(string filename, ExcelWorkbook excelWorkbook)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = excelWorkbook.ExcelFileName;
            }
            ExportEXCEL(filename, NPOIExcel.RichRenderToExcelStream(excelWorkbook));
        }
        #endregion

        #region 导出到pdf格式的文件中
        public static void ExportPDF(string filename, MemoryStream m)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + ".pdf");
            HttpContext.Current.Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            HttpContext.Current.Response.OutputStream.Flush();
            HttpContext.Current.Response.OutputStream.Close();
        }
        public static void ExportPDF(string filename, System.Data.DataTable dt)
        {
            ExportPDF(filename, ConvertDataTableToPDFStream(dt));
        }
        private static MemoryStream ConvertDataTableToPDFStream(System.Data.DataTable dt)
        {
            MemoryStream m = new MemoryStream();
            Document document = new Document(new Rectangle(dt.Columns.Count * 120, 1000));
            float FontSize = 13.0f;
            PdfWriter.GetInstance(document, m);
            document.Open();
            string fontPath = HttpContext.Current.Server.MapPath("../bin/FileIO/resource/simhei.ttf");
            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, true);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, FontSize);
            PdfPTable table = new PdfPTable(dt.Columns.Count);
            table.SetExtendLastRow(true, false);
            table.HeaderRows = 1;
            
            //打印列名
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                PdfPCell cell = new PdfPCell(new Phrase(dt.Columns[j].ColumnName.ToString()));
                cell.MinimumHeight = 22;//默认高度，当内容过多，高度会自适应
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.YellowGreen);
                table.AddCell(cell);
            }
            //遍历table的内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(dt.Rows[i][j].ToString(), font));
                    cell.MinimumHeight = 15;//默认高度，当内容过多，高度会自适应
                    if (dt.Rows[i].RowState == DataRowState.Added)
                    {
                        cell.BackgroundColor = new BaseColor(System.Drawing.Color.Yellow);
                    }
                    table.AddCell(cell);
                }
            }
            //在目标文档中添加表数据
            document.Add(table);
            document.Close();
            return m;
        }
        #endregion
    }
}
