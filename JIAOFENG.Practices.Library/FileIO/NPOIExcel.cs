using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.Util;
using NPOI.HSSF.UserModel;
using NPOI.POIFS;
using NPOI.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace JIAOFENG.Practices.Library.FileIO
{
    public class NPOIExcel
    {
        public static MemoryStream RenderDataTableToExcelStream(DataTable sourceTable)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = null;
            if (string.IsNullOrWhiteSpace(sourceTable.TableName))
            {
                sheet = workbook.CreateSheet();
            }
            else
            {
                sheet = workbook.CreateSheet(sourceTable.TableName);
            }
            IRow headerRow = sheet.CreateRow(0);
           
            
            #region header style 
            ICellStyle headerstyle = workbook.CreateCellStyle();
            //align
            headerstyle.Alignment = HorizontalAlignment.Left;
            ////cell background
            //headerstyle.FillForegroundColor = HSSFColor.LIGHT_GREEN.index;
            //headerstyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            ////cell border
            //headerstyle.TopBorderColor = HSSFColor.BLACK.index;
            //headerstyle.RightBorderColor = HSSFColor.BLACK.index;
            //headerstyle.BottomBorderColor = HSSFColor.BLACK.index;
            //headerstyle.LeftBorderColor = HSSFColor.BLACK.index;
            //font
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 12;//字号
            font.Boldweight = 600;//加粗
            headerstyle.SetFont(font);
            #endregion

            #region data style
            ICellStyle datastyle = workbook.CreateCellStyle();
            //align
            datastyle.Alignment = HorizontalAlignment.Left;
            //cell background
            datastyle.FillForegroundColor = HSSFColor.Yellow.Index;
            datastyle.FillPattern = FillPattern.SolidForeground;
            //cell border
            datastyle.TopBorderColor = HSSFColor.Blue.Index;
            datastyle.RightBorderColor = HSSFColor.Blue.Index;
            datastyle.BottomBorderColor = HSSFColor.Blue.Index;
            datastyle.LeftBorderColor = HSSFColor.Blue.Index;
            //font
            IFont datafont = workbook.CreateFont();
            datafont.FontHeightInPoints = 11;
            datastyle.SetFont(datafont);
            #endregion

            // handling header.
            foreach (DataColumn column in sourceTable.Columns)
            {
                ICell cl = headerRow.CreateCell(column.Ordinal);
                cl.SetCellValue(column.ColumnName);
                cl.CellStyle = headerstyle;
                //headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }
            // handling value.
            int rowIndex = 1;

            foreach (DataRow row in sourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in sourceTable.Columns)
                {
                    ICell cdl = dataRow.CreateCell(column.Ordinal);
                    cdl.SetCellValue(row[column].ToString());
                    if (row.RowState == DataRowState.Added)
                    {
                        cdl.CellStyle = datastyle;
                    }
                    //dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();
            //ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }
        public static MemoryStream RenderDataTableToExcelStream(DataSet sourceDataSet)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            #region header style
            ICellStyle headerstyle = workbook.CreateCellStyle();
            //align
            headerstyle.Alignment = HorizontalAlignment.Left;
            ////cell background
            //headerstyle.FillForegroundColor = HSSFColor.LIGHT_GREEN.index;
            //headerstyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            ////cell border
            //headerstyle.TopBorderColor = HSSFColor.BLACK.index;
            //headerstyle.RightBorderColor = HSSFColor.BLACK.index;
            //headerstyle.BottomBorderColor = HSSFColor.BLACK.index;
            //headerstyle.LeftBorderColor = HSSFColor.BLACK.index;
            //font
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 12;//字号
            font.Boldweight = 600;//加粗
            headerstyle.SetFont(font);
            #endregion

            #region data style
            ICellStyle datastyle = workbook.CreateCellStyle();
            //align
            datastyle.Alignment = HorizontalAlignment.Left;
            //cell background
            datastyle.FillForegroundColor = HSSFColor.Yellow.Index;
            datastyle.FillPattern = FillPattern.SolidForeground;
            //cell border
            datastyle.TopBorderColor = HSSFColor.Blue.Index;
            datastyle.RightBorderColor = HSSFColor.Blue.Index;
            datastyle.BottomBorderColor = HSSFColor.Blue.Index;
            datastyle.LeftBorderColor = HSSFColor.Blue.Index;
            //font
            IFont datafont = workbook.CreateFont();
            datafont.FontHeightInPoints = 11;
            datastyle.SetFont(datafont);
            #endregion

            MemoryStream ms = new MemoryStream();
            
            
            foreach (DataTable sourceTable in sourceDataSet.Tables)
            {
                ISheet sheet = null;
                if (string.IsNullOrWhiteSpace(sourceTable.TableName))
                {
                    sheet = workbook.CreateSheet();
                }
                else
                {
                    sheet = workbook.CreateSheet(sourceTable.TableName);
                }
                IRow headerRow = sheet.CreateRow(0);
                // handling header.
                foreach (DataColumn column in sourceTable.Columns)
                {
                    ICell cl = headerRow.CreateCell(column.Ordinal);
                    cl.SetCellValue(column.ColumnName);
                    cl.CellStyle = headerstyle;
                    //headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                }

                // handling value.
                int rowIndex = 1;
                foreach (DataRow row in sourceTable.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in sourceTable.Columns)
                    {
                        ICell cdl = dataRow.CreateCell(column.Ordinal);
                        cdl.SetCellValue(row[column].ToString());
                        if (row.RowState == DataRowState.Added)
                        {
                            cdl.CellStyle = datastyle;
                        }
                        //dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }
            }           

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            workbook = null;

            return ms;
        }
        private static ICellStyle CreateStyle(ExcelCellStyle cellStyle, XSSFWorkbook workbook)
        {
            ICellStyle dataStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat(); 
            switch(cellStyle.DataFormat)
            {
                case ExcelCellDataFormat.Text:
                    {
                        break;
                    }
                case ExcelCellDataFormat.Percent:
                    {
                        dataStyle.DataFormat = format.GetFormat(string.IsNullOrWhiteSpace(cellStyle.DataFormatInfo) ? "0.00%" : cellStyle.DataFormatInfo);
                        break;
                    }
                case ExcelCellDataFormat.Currency:
                    {
                        dataStyle.DataFormat = format.GetFormat(string.IsNullOrWhiteSpace(cellStyle.DataFormatInfo) ? "#,##0" : cellStyle.DataFormatInfo);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            
            dataStyle.Alignment = cellStyle.Alignment;
            dataStyle.VerticalAlignment = cellStyle.VerticalAlignment;
            dataStyle.BorderTop = cellStyle.BorderTop;
            dataStyle.BorderBottom = cellStyle.BorderBottom;
            dataStyle.BorderLeft = cellStyle.BorderLeft;
            dataStyle.BorderRight = cellStyle.BorderRight;
            dataStyle.Indention = cellStyle.Indention;
            dataStyle.WrapText = cellStyle.WrapText;

            //cell background            
            if (cellStyle.FillForegroundColor != null)
            {
                dataStyle.FillPattern = FillPattern.SolidForeground;
                XSSFCellStyle style = (XSSFCellStyle)dataStyle;
                style.SetFillForegroundColor(cellStyle.FillForegroundColor);
                //dataStyle.FillForegroundColor = cellStyle.FillForegroundColor.Indexed;//背景色
            }
            
            //if (cell.CellStyle.FillForegroundColor != null)
            //{
            //    dataStyle.FillForegroundColor = cell.CellStyle.FillForegroundColor.GetIndex();
            //    dataStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            //}
            //dataStyle.FillForegroundColor = HSSFColor.WHITE.index;//FillBackgroundColor
            //dataStyle.FillBackgroundColor = cell.CellStyle.FillBackgroundColor.GetIndex();// HSSFColor.YELLOW.index;
            //cell border
            //dataStyle.TopBorderColor = HSSFColor.BLUE.index;
            //dataStyle.RightBorderColor = HSSFColor.BLUE.index;
            //dataStyle.BottomBorderColor = HSSFColor.BLUE.index;
            //dataStyle.LeftBorderColor = HSSFColor.BLUE.index;
            //font
            IFont datafont = workbook.CreateFont();
            datafont.FontName = cellStyle.FontName;
            datafont.FontHeightInPoints = cellStyle.FontHeightInPoints;
            if (cellStyle.FontBoldweight > 0)
            {
                datafont.Boldweight = cellStyle.FontBoldweight;
            }
            if (cellStyle.FontColor != null)
            {
                XSSFFont font = (XSSFFont)datafont;
                font.SetColor(cellStyle.FontColor);
                //datafont.Color = cellStyle.FontColor.Indexed;
            }
            dataStyle.SetFont(datafont);
            return dataStyle;
        }
        private static ICellStyle CreateStyleFrom(ICellStyle cellStyle, XSSFWorkbook workbook)
        {
            ICellStyle dataStyle = workbook.CreateCellStyle();

            dataStyle.Alignment = cellStyle.Alignment;
            dataStyle.VerticalAlignment = cellStyle.VerticalAlignment;
            dataStyle.BorderTop = cellStyle.BorderTop;
            dataStyle.BorderBottom = cellStyle.BorderBottom;
            dataStyle.BorderLeft = cellStyle.BorderLeft;
            dataStyle.BorderRight = cellStyle.BorderRight;
            dataStyle.Indention = cellStyle.Indention;
            dataStyle.WrapText = cellStyle.WrapText;
            IFont datafont = workbook.CreateFont();
            IFont oldFont = cellStyle.GetFont(workbook);
            datafont.FontHeightInPoints = oldFont.FontHeightInPoints;// 11;
            if (oldFont.Boldweight > 0)
            {
                datafont.Boldweight = oldFont.Boldweight;
            }
            dataStyle.SetFont(datafont);
            return dataStyle;
        }
        private static int GetNextValueIndex(List<string> cells, int startIndex)
        {
            string value = cells[startIndex];
            if (string.IsNullOrEmpty(value))
            {
                return startIndex;
            }
            int ret = startIndex;
            for (int cellIndex = startIndex + 1; cellIndex <= cells.Count - 1; cellIndex++)
            {
                if (string.Equals(value, cells[cellIndex], StringComparison.OrdinalIgnoreCase))
                {
                    ret = cellIndex;
                }
                else
                {
                    break;
                }
            }
            return ret;
        }

        private static void CreateCell(IRow dataRow, int cellIndex, int rowIndex, ExcelCell cell, ExcelSheet excelSheet, XSSFWorkbook workbook, Dictionary<int, ICellStyle> columnsStyle, Dictionary<int, ICellStyle> rowsStyle, Dictionary<ExcelRegion, ICellStyle> regionStyle, Dictionary<ExcelCellStyle, ICellStyle> backupStyle)
        {
            ICell cdl = dataRow.CreateCell(cellIndex);
            if (cell.CellType != CellType.Unknown)
            {
                cdl.SetCellType(cell.CellType);
            }            
            switch (cell.CellType)
            {
                case CellType.Numeric:                   
                    cdl.SetCellValue(double.Parse(cell.Value.ToString()));
                    break;
                default:
                    cdl.SetCellValue(cell.Value.ToString());
                    break;
            }
            #region data style
            if (cell.CellStyle != null)
            {
                if (!backupStyle.Keys.Contains(cell.CellStyle))
                {
                    ICellStyle style = CreateStyle(cell.CellStyle, workbook);
                    backupStyle.Add(cell.CellStyle, style);
                }
                cdl.CellStyle = backupStyle[cell.CellStyle];
            }
            else if (regionStyle != null && regionStyle.Keys.Where(r => r.FirstRow<= rowIndex && r.LastRow>= rowIndex && r.FirstColumn<=cellIndex && r.LastColumn>= cellIndex).Any())
            {
                ExcelRegion region = regionStyle.Keys.First(r => r.FirstRow <= rowIndex && r.LastRow >= rowIndex && r.FirstColumn <= cellIndex && r.LastColumn >= cellIndex);
                cdl.CellStyle = regionStyle[region];
            }
            else
            {
                if (excelSheet.RowStyleFirst)
                {
                    if (rowsStyle != null && rowsStyle.Keys.Contains(rowIndex))
                    {
                        cdl.CellStyle = rowsStyle[rowIndex];
                    }
                    else if (columnsStyle != null && columnsStyle.Keys.Contains(cellIndex))
                    {
                        cdl.CellStyle = columnsStyle[cellIndex];
                    }
                }
                else
                {
                    if (columnsStyle != null && columnsStyle.Keys.Contains(cellIndex))
                    {
                        cdl.CellStyle = columnsStyle[cellIndex];
                    }
                    else if (rowsStyle != null && rowsStyle.Keys.Contains(rowIndex))
                    {
                        cdl.CellStyle = rowsStyle[rowIndex];
                    }                    
                }               
            }
            #endregion
        }
        private static void CreateRow(ISheet sheet, int rowIndex, ExcelRow row, ExcelSheet excelSheet, XSSFWorkbook workbook, Dictionary<int, ICellStyle> columnsStyle, Dictionary<int, ICellStyle> rowsStyle, Dictionary<ExcelRegion, ICellStyle> regionStyle, Dictionary<ExcelCellStyle, ICellStyle> backupStyle)
        {
            IRow dataRow = sheet.CreateRow(rowIndex);
            int cellIndex = 0;
            ICell cdlBlack = dataRow.CreateCell(cellIndex);
            cellIndex++;
            foreach (ExcelCell cell in row)
            {
                CreateCell(dataRow, cellIndex, rowIndex, cell, excelSheet, workbook, columnsStyle, rowsStyle, regionStyle, backupStyle);
                cellIndex++;
            }            
        }

        private static void CreateSheet(ExcelSheet excelSheet, XSSFWorkbook workbook)
        {
            Dictionary<ExcelCellStyle, ICellStyle> backup = new Dictionary<ExcelCellStyle, ICellStyle>();
            int maxColumnCount = 0;
            for (int i = 0; i <= Math.Min(4, excelSheet.Count - 1); i++)
            {
                maxColumnCount = Math.Max(maxColumnCount, excelSheet[i].Count);
            }
            ISheet sheet = null;
            if (string.IsNullOrWhiteSpace(excelSheet.ExcelSheetName))
            {
                sheet = workbook.CreateSheet();
            }
            else
            {
                sheet = workbook.CreateSheet(excelSheet.ExcelSheetName);
            }
            
            int rowIndex = 0;
            #region Subject
            if (excelSheet.ExcelSubject != null)
            {
                IRow dataRowSubject = sheet.CreateRow(rowIndex);
                if (excelSheet.RowsHeight.Keys.Contains(1))
                {
                    dataRowSubject.Height = (short)(excelSheet.RowsHeight[1] * 20);
                }
                for (int i = 0; i < maxColumnCount-1; i++)
                {
                    CreateCell(dataRowSubject, i, rowIndex, excelSheet.ExcelSubject, excelSheet, workbook, null, null, null, backup);
                }
                sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 0, maxColumnCount));
                rowIndex++;
            }          
            #endregion

            #region Summary
            int tempCellIndex = 0;
            foreach (string key in excelSheet.ExcelSummarys.Keys)
            {
                IRow dataRowSummary;
                if (tempCellIndex >= maxColumnCount - 1)
                {
                    rowIndex++;
                    tempCellIndex = 0;
                }
                if (tempCellIndex == 0)
                {
                    dataRowSummary = sheet.CreateRow(rowIndex);
                    ICell cdl = dataRowSummary.CreateCell(tempCellIndex);
                    tempCellIndex++;
                }
                else
                {
                    dataRowSummary = sheet.GetRow(rowIndex);
                }
                //ICell cdTitle = dataRowSummary.CreateCell(tempCellIndex);
                //cdTitle.SetCellValue(key);
                ExcelCell cellTitle = new ExcelCell(key, new ExcelSummaryTitleStyle());
                CreateCell(dataRowSummary, tempCellIndex, rowIndex, cellTitle, excelSheet, workbook, null, null, null, backup);
                tempCellIndex++;
                //ICell cdValue = dataRowSummary.CreateCell(tempCellIndex);
                //cdValue.SetCellValue(excelSheet.ExcelSummarys[key]);
                ExcelCell cell = new ExcelCell(excelSheet.ExcelSummarys[key], new ExcelCellStyle());
                CreateCell(dataRowSummary, tempCellIndex, rowIndex, cell, excelSheet, workbook, null, null, null, backup);
                tempCellIndex++;
            }

            #endregion

            #region Data
            if (sheet.GetRow(rowIndex) != null)
            {
                rowIndex++;
            }
            int rowDetailStart = rowIndex;
            

            Dictionary<int, ICellStyle> columnsStyle = new Dictionary<int, ICellStyle>();
            foreach (int key in excelSheet.ColumnsStyle.Keys)
            {
                if (backup.Keys.Contains(excelSheet.ColumnsStyle[key]))
                {
                    columnsStyle.Add(key, backup[excelSheet.ColumnsStyle[key]]);
                }
                else
                {
                    ICellStyle style = CreateStyle(excelSheet.ColumnsStyle[key], workbook);
                    backup.Add(excelSheet.ColumnsStyle[key], style);
                    columnsStyle.Add(key, style);
                }
            }
            Dictionary<int, ICellStyle> rowsStyle = new Dictionary<int, ICellStyle>();
            foreach (int key in excelSheet.RowsStyle.Keys)
            {
                if (backup.Keys.Contains(excelSheet.RowsStyle[key]))
                {
                    rowsStyle.Add(key + rowDetailStart - 1, backup[excelSheet.RowsStyle[key]]);
                }
                else
                {
                    ICellStyle style = CreateStyle(excelSheet.RowsStyle[key], workbook);
                    backup.Add(excelSheet.RowsStyle[key], style);
                    rowsStyle.Add(key + rowDetailStart - 1, style);
                }
            }
            Dictionary<ExcelRegion, ICellStyle> regionStyle = new Dictionary<ExcelRegion, ICellStyle>();
            foreach (ExcelRegion key in excelSheet.RegionStyle.Keys)
            {
                if (backup.Keys.Contains(excelSheet.RegionStyle[key]))
                {
                    regionStyle.Add(new ExcelRegion(key.FirstRow + rowDetailStart - 1, key.LastRow + rowDetailStart - 1, key.FirstColumn, key.LastColumn), backup[excelSheet.RegionStyle[key]]);
                }
                else
                {
                    ICellStyle style = CreateStyle(excelSheet.RegionStyle[key], workbook);
                    backup.Add(excelSheet.RegionStyle[key], style);
                    regionStyle.Add(new ExcelRegion(key.FirstRow + rowDetailStart - 1, key.LastRow + rowDetailStart - 1, key.FirstColumn, key.LastColumn), style);
                }
            }
            foreach (ExcelRow row in excelSheet)
            {
                CreateRow(sheet, rowIndex, row, excelSheet, workbook, columnsStyle, rowsStyle, regionStyle, backup);
                rowIndex++;
            }
            //边框
            //if (excelSheet.HasBorder)
            //{
            //    for (int i = rowDetailStart; i <= rowDetailStart + excelSheet.Count - 1; i++)
            //    {
            //        IRow irow = sheet.GetRow(i);
            //        if (i == rowDetailStart)
            //        {
            //            foreach (ICell icell in irow.Cells)
            //            {
            //                if (icell.ColumnIndex != 0)
            //                {
            //                    ICellStyle dataStyle = CreateStyleFrom(icell.CellStyle, workbook);
            //                    dataStyle.BorderTop = excelSheet.BorderStyle;
            //                    icell.CellStyle = dataStyle;
            //                }
            //            }
            //        }
            //        if (i == (rowDetailStart + excelSheet.Count - 1))
            //        {
            //            foreach (ICell icell in irow.Cells)
            //            {
            //                if (icell.ColumnIndex != 0)
            //                {
            //                    ICellStyle dataStyle = CreateStyleFrom(icell.CellStyle, workbook);
            //                    dataStyle.BorderBottom = excelSheet.BorderStyle;
            //                    icell.CellStyle = dataStyle;
            //                }
            //            }
            //        }
            //        ICell firstCell = irow.GetCell(1);
            //        ICellStyle dataStyleFirst = CreateStyleFrom(firstCell.CellStyle, workbook);
            //        dataStyleFirst.BorderLeft = excelSheet.BorderStyle;
            //        firstCell.CellStyle = dataStyleFirst;

            //        ICell lastCell = irow.GetCell(irow.LastCellNum - 1);
            //        ICellStyle dataStyleLast = CreateStyleFrom(lastCell.CellStyle, workbook);
            //        dataStyleLast.BorderRight = excelSheet.BorderStyle;
            //        lastCell.CellStyle = dataStyleLast;
            //    }
            //}          

            //width,为默认添加的A列增加列宽
            sheet.SetColumnWidth(0, 2 * 256);
            for(int i=1;i<= maxColumnCount ;i++)
            {
                sheet.AutoSizeColumn(i);
            }
            foreach (int columnIndex in excelSheet.ColumnsWidth.Keys)
            {
                sheet.SetColumnWidth(columnIndex, excelSheet.ColumnsWidth[columnIndex] * 256);
            }
            foreach (int rowMergedIndex in excelSheet.RowsMerged)
            {
                List<string> cells = excelSheet.GetRow(rowMergedIndex - 1);
                int cellIndex = 0;
                while (cellIndex <= cells.Count - 1)
                {
                    int indexEnd = GetNextValueIndex(cells, cellIndex);
                    if (indexEnd != cellIndex)
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(rowMergedIndex + rowDetailStart -1, rowMergedIndex + rowDetailStart-1, cellIndex + 1, indexEnd + 1));
                        cellIndex = indexEnd;
                    }
                    cellIndex++;
                }

            }
            foreach (int columnMergedIndex in excelSheet.ColumnsMerged)
            {
                List<string> cells = excelSheet.GetColumn(columnMergedIndex - 1);
                int cellIndex = 0;
                while (cellIndex <= cells.Count - 1)
                {
                    int indexEnd = GetNextValueIndex(cells, cellIndex);
                    if (indexEnd != cellIndex)
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(cellIndex + rowDetailStart, indexEnd + rowDetailStart, columnMergedIndex, columnMergedIndex));
                        cellIndex = indexEnd;
                    }
                    cellIndex++;
                }

            }
            foreach (ExcelRegion region in excelSheet.RegionMerged)
            {
                sheet.AddMergedRegion(new CellRangeAddress(region.FirstRow + rowDetailStart - 1, region.LastRow + rowDetailStart - 1, region.FirstColumn, region.LastColumn));
            }
            if (excelSheet.FreezeColumnIndex >= 0 && excelSheet.FreezeRowIndex>=0)
            {
                sheet.CreateFreezePane(excelSheet.FreezeColumnIndex + 1, excelSheet.FreezeRowIndex + rowDetailStart);
            }           
            #endregion
        }
        public static MemoryStream RichRenderToExcelStream(ExcelWorkbook excelWorkbook)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            foreach (ExcelSheet excelSheet in excelWorkbook)
            {
                CreateSheet(excelSheet, workbook);
            }                 

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            //ms.Position = 0;

            workbook = null;

            return ms;
        }
        public static void RenderDataTableToExcel(DataTable sourceTable, string fileName)
        {
            MemoryStream ms = RenderDataTableToExcelStream(sourceTable);
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            byte[] data = ms.ToArray();
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();

            data = null;
            ms = null;
            fs = null;
        }

        /// <summary>
        /// Render a Excel to DataTable
        /// </summary>
        /// <param name="excelFileStream">ExcelFile Stream</param>
        /// <param name="sheetIndex">start from 1</param>
        /// <param name="headerRowIndex">start from 0</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(Stream excelFileStream, int sheetIndex, int headerRowIndex)
        {
            XSSFWorkbook workbook = new XSSFWorkbook(excelFileStream);
            ISheet sheet = workbook.GetSheetAt(sheetIndex);
            DataTable dt = RenderDataTableFromExcel(sheet, headerRowIndex);
            excelFileStream.Close();
            workbook = null;
            sheet = null;
            return dt;
        }
        public static DataTable RenderDataTableFromExcel(Stream excelFileStream, string sheetName, int headerRowIndex)
        {
            XSSFWorkbook workbook = new XSSFWorkbook(excelFileStream);
            ISheet sheet = workbook.GetSheet(sheetName);
            DataTable dt = RenderDataTableFromExcel(sheet, headerRowIndex);
            excelFileStream.Close();
            workbook = null;
            sheet = null;
            return dt;
        }
        private static DataTable RenderDataTableFromExcel(ISheet sheet, int headerRowIndex)
        {
            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(headerRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            int rowCount = sheet.LastRowNum;

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                table.Rows.Add(dataRow);
            }
            return table;
        }

        /// <summary>
        /// Render a Excel to DataTable
        /// </summary>
        /// <param name="excelFileStream">ExcelFile file path</param>
        /// <param name="sheetIndex">start from 1</param>
        /// <param name="headerRowIndex">start from 0</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(string excelFilePath, int sheetIndex, int headerRowIndex)
        {
            return RenderDataTableFromExcel(new JIAOFENG.Practices.Library.FileIO.FileOperator(excelFilePath).ConvertToStream(), sheetIndex, headerRowIndex);
        }

        public static DataSet GetExcelData(Stream s)
        {
            //FileStream fs = System.IO.File.OpenRead(@"D:\Test.xlsx");
            DataSet ds = new DataSet();
            //根据路径通过已存在的excel来创建XSSFWorkbook，即整个excel文档
            IWorkbook workbook = null;
            try
            {
                workbook = new XSSFWorkbook(s);
            }
            catch (Exception ex)
            {
                workbook = new XSSFWorkbook(s);
            }
            int sheetCount = workbook.NumberOfSheets;
            for (int i = 0; i <= sheetCount - 1; i++)
            {
                ISheet sheet = workbook.GetSheetAt(i);
                DataTable table = CreateDataTableByNPOISheet(sheet);
                if (table != null)
                {
                    ds.Tables.Add(table);
                }                
                sheet = null;
            }
            workbook = null;
            return ds;
        }
        public static DataTable CreateDataTableByNPOISheet(ISheet sheet)
        {
            DataTable table = new DataTable();
            //获取sheet的首行
            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null)
            {
                return null;
            }
            //一行最后一个方格的编号 即总的列数
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            //最后一列的标号  即总的行数
            int rowCount = sheet.LastRowNum;

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                table.Rows.Add(dataRow);
            }
            return table;
        }
    }
}