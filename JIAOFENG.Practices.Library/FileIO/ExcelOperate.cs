using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Web;

namespace JIAOFENG.Practices.Library.FileIO
{
    /// <summary>
    /// Excel操作函数 by excle COM+
    /// </summary>
    public class ExcelOperate
    {
        private object mValue = System.Reflection.Missing.Value;
        public ExcelOperate()
        { }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objStartCell">开始单元格</param>
        /// <param name="objEndCell">结束单元格</param>
        public void Merge(Excel._Worksheet CurSheet, object objStartCell, object objEndCell)
        {
            CurSheet.get_Range(objStartCell, objEndCell).Merge(mValue);
        }

        /// <summary>
        /// 设置连续区域的字体大小
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="strStartCell">开始单元格</param>
        /// <param name="strEndCell">结束单元格</param>
        /// <param name="intFontSize">字体大小</param>
        public void SetFontSize(Excel._Worksheet CurSheet, object objStartCell, object objEndCell, int intFontSize)
        {
            CurSheet.get_Range(objStartCell, objEndCell).Font.Size = intFontSize.ToString();
        }

        /// <summary>
        /// 在指定单元格插入指定的值
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="Cell">单元格 如Cells[1,1]</param>
        /// <param name="objValue">文本、数字等值</param>
        public void WriteCell(Excel._Worksheet CurSheet, object objCell, object objValue)
        {
            CurSheet.get_Range(objCell, mValue).Value2 = objValue;
        }

        /// <summary>
        /// 在指定Range中插入指定的值
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="StartCell">开始单元格</param>
        /// <param name="EndCell">结束单元格</param>
        /// <param name="objValue">文本、数字等值</param>
        public void WriteRange(Excel._Worksheet CurSheet, object objStartCell, object objEndCell, object objValue)
        {
            CurSheet.get_Range(objStartCell, objEndCell).Value2 = objValue;
        }

        /// <summary>
        /// 合并单元格，并在合并后的单元格中插入指定的值
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objStartCell">开始单元格</param>
        /// <param name="objEndCell">结束单元格</param>
        /// <param name="objValue">文本、数字等值</param>
        public void WriteAfterMerge(Excel._Worksheet CurSheet, object objStartCell, object objEndCell, object objValue)
        {
            CurSheet.get_Range(objStartCell, objEndCell).Merge(mValue);
            CurSheet.get_Range(objStartCell, mValue).Value2 = objValue;
        }

        /// <summary>
        /// 为单元格设置公式
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objCell">单元格</param>
        /// <param name="strFormula">公式</param>
        public void SetFormula(Excel._Worksheet CurSheet, object objCell, string strFormula)
        {
            CurSheet.get_Range(objCell, mValue).Formula = strFormula;
        }

        /// <summary>
        /// 单元格自动换行
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objStartCell">开始单元格</param>
        /// <param name="objEndCell">结束单元格</param>
        public void AutoWrapText(Excel._Worksheet CurSheet, object objStartCell, object objEndCell)
        {
            CurSheet.get_Range(objStartCell, objEndCell).WrapText = true;
        }

        /// <summary>
        /// 设置整个连续区域的字体颜色
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objStartCell">开始单元格</param>
        /// <param name="objEndCell">结束单元格</param>
        /// <param name="clrColor">颜色</param>
        public void SetColor(Excel._Worksheet CurSheet, object objStartCell, object objEndCell, System.Drawing.Color clrColor)
        {
            CurSheet.get_Range(objStartCell, objEndCell).Font.Color = System.Drawing.ColorTranslator.ToOle(clrColor);
        }

        /// <summary>
        /// 设置整个连续区域的单元格背景色
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objStartCell">开始单元格</param>
        /// <param name="objEndCell">结束单元格</param>
        /// <param name="clrColor">颜色</param>
        public void SetBgColor(Excel._Worksheet CurSheet, object objStartCell, object objEndCell, System.Drawing.Color clrColor)
        {
            CurSheet.get_Range(objStartCell, objEndCell).Interior.Color = System.Drawing.ColorTranslator.ToOle(clrColor);
        }


        /// <summary>
        /// 设置连续区域的字体名称
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objStartCell">开始单元格</param>
        /// <param name="objEndCell">结束单元格</param>
        /// <param name="fontname">字体名称 隶书、仿宋_GB2312等</param>
        public void SetFontName(Excel._Worksheet CurSheet, object objStartCell, object objEndCell, string fontname)
        {
            CurSheet.get_Range(objStartCell, objEndCell).Font.Name = fontname;
        }

        /// <summary>
        /// 设置连续区域的字体为粗体
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objStartCell">开始单元格</param>
        /// <param name="objEndCell">结束单元格</param>
        public void SetBold(Excel._Worksheet CurSheet, object objStartCell, object objEndCell)
        {
            CurSheet.get_Range(objStartCell, objEndCell).Font.Bold = true;
        }

        /// <summary>
        /// 设置连续区域的边框：上下左右都为黑色连续边框
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objStartCell">开始单元格</param>
        /// <param name="objEndCell">结束单元格</param>
        public void SetBorderAll(Excel._Worksheet CurSheet, object objStartCell, object objEndCell)
        {
            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeTop].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;

            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeBottom].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;

            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeLeft].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;

            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeRight].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;

            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlInsideHorizontal].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;

            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlInsideVertical].Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;

        }
        public void SetBorderAll(Excel._Worksheet CurSheet, object objStartCell, object objEndCell, System.Drawing.Color col)
        {
            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeTop].Color = System.Drawing.ColorTranslator.ToOle(col);
            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;

            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeBottom].Color = System.Drawing.ColorTranslator.ToOle(col);
            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;

            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeLeft].Color = System.Drawing.ColorTranslator.ToOle(col);
            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;

            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeRight].Color = System.Drawing.ColorTranslator.ToOle(col);
            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;

            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlInsideHorizontal].Color = System.Drawing.ColorTranslator.ToOle(col);
            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;

            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlInsideVertical].Color = System.Drawing.ColorTranslator.ToOle(col);
            CurSheet.get_Range(objStartCell, objEndCell).Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;

        }
        /// <summary>
        /// 设置连续区域水平居中
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objStartCell">开始单元格</param>
        /// <param name="objEndCell">结束单元格</param>
        public void SetHAlignCenter(Excel._Worksheet CurSheet, object objStartCell, object objEndCell)
        {
            CurSheet.get_Range(objStartCell, objEndCell).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        }

        /// <summary>
        /// 设置连续区域水平居左
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objStartCell">开始单元格</param>
        /// <param name="objEndCell">结束单元格</param>
        public void SetHAlignLeft(Excel._Worksheet CurSheet, object objStartCell, object objEndCell)
        {
            CurSheet.get_Range(objStartCell, objEndCell).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
        }

        /// <summary>
        /// 设置连续区域水平居右
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objStartCell">开始单元格</param>
        /// <param name="objEndCell">结束单元格</param>
        public void SetHAlignRight(Excel._Worksheet CurSheet, object objStartCell, object objEndCell)
        {
            CurSheet.get_Range(objStartCell, objEndCell).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        }

        /// <summary>
        /// 设置连续区域的显示格式
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objStartCell">开始单元格</param>
        /// <param name="objEndCell">结束单元格</param>
        /// <param name="strNF">如"#,##0.00"的显示格式</param>
        public void SetNumberFormat(Excel._Worksheet CurSheet, object objStartCell, object objEndCell, string strNF)
        {
            CurSheet.get_Range(objStartCell, objEndCell).NumberFormat = strNF;
        }

        /// <summary>
        /// 设置列宽
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="strColID">列标识，如A代表第一列</param>
        /// <param name="dblWidth">宽度</param>
        public void SetColumnWidth(Excel._Worksheet CurSheet, string strColID, double dblWidth)
        {
            ((Excel.Range)CurSheet.Columns.GetType().InvokeMember("Item", System.Reflection.BindingFlags.GetProperty, null, CurSheet.Columns, new object[] { (strColID + ":" + strColID).ToString() })).ColumnWidth = dblWidth;
        }

        /// <summary>
        /// 设置列宽
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objStartCell">开始单元格</param>
        /// <param name="objEndCell">结束单元格</param>
        /// <param name="dblWidth">宽度</param>
        public void SetColumnWidth(Excel._Worksheet CurSheet, object objStartCell, object objEndCell, double dblWidth)
        {
            CurSheet.get_Range(objStartCell, objEndCell).ColumnWidth = dblWidth;
        }

        /// <summary>
        /// 设置行高
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objStartCell">开始单元格</param>
        /// <param name="objEndCell">结束单元格</param>
        /// <param name="dblHeight">行高</param>
        public void SetRowHeight(Excel._Worksheet CurSheet, object objStartCell, object objEndCell, double dblHeight)
        {
            CurSheet.get_Range(objStartCell, objEndCell).RowHeight = dblHeight;
        }

        /// <summary>
        /// 为单元格添加超级链接
        /// </summary>
        /// <param name="CurSheet">Worksheet</param>
        /// <param name="objCell">单元格</param>
        /// <param name="strAddress">链接地址</param>
        /// <param name="strTip">屏幕提示</param>
        /// <param name="strText">链接文本</param>
        public void AddHyperLink(Excel._Worksheet CurSheet, object objCell, string strAddress, string strTip, string strText)
        {
            CurSheet.Hyperlinks.Add(CurSheet.get_Range(objCell, objCell), strAddress, mValue, strTip, strText);
        }

        /// <summary>
        /// 添加文本文档对象
        /// </summary>
        /// <param name="CurSheet">当前WorkSheet</param>
        /// <param name="filepath">文件路径</param>
        /// <param name="title">图标之下显示的标题</param>
        /// <param name="left">相对于文档的左上角，以磅为单位给出新对象的左上角位置</param>
        /// <param name="top">相对于文档的左上角，以磅为单位给出新对象的左上角位置</param>
        /// <param name="width">以磅为单位给出 OLE 对象的width</param>
        /// <param name="height">以磅为单位给出 OLE 对象的height</param>
        /// <returns></returns>
        public Microsoft.Office.Interop.Excel.Shape AddTxtOLEObject(Excel._Worksheet CurSheet, object filepath,object title,object left,object top,object width,object height)
        {
            return CurSheet.Shapes.AddOLEObject(Type.Missing,
                                                    filepath,
                                                    false,
                                                    true,
                                                    "C:\\Windows\\system32\\packager.dll",
                                                    0,
                                                    title,
                                                    left,
                                                    top,
                                                    width,
                                                    height);
        }
        /// <summary>
        /// 添加Excel对象
        /// </summary>
        /// <param name="CurSheet"></param>
        /// <param name="filepath"></param>
        /// <param name="title"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Microsoft.Office.Interop.Excel.Shape AddExcelOLEObject(Excel._Worksheet CurSheet, object filepath, object title, object left, object top, object width, object height)
        {
            //Workbooks.Open Filename:="C:\Users\LZZ\Desktop\attent\AttentExcel.xlsx"
            //ActiveWindow.Visible = False
            return CurSheet.Shapes.AddOLEObject(Type.Missing,
                                                    filepath,
                                                    false,
                                                    true,
                                                    "C:\\Windows\\Installer\\{90120000-0011-0000-0000-0000000FF1CE}\\xlicons.exe",
                                                    0,
                                                    title,
                                                    left,
                                                    top,
                                                    width,
                                                    height);
        }
        /// <summary>
        /// 添加Word对象
        /// </summary>
        /// <param name="CurSheet"></param>
        /// <param name="filepath"></param>
        /// <param name="title"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Microsoft.Office.Interop.Excel.Shape AddWordOLEObject(Excel._Worksheet CurSheet, object filepath, object title, object left, object top, object width, object height)
        {
            return CurSheet.Shapes.AddOLEObject(Type.Missing,
                                                    filepath,
                                                    false,
                                                    true,
                                                    "C:\\PROGRA~1\\MIF5BA~1\\Office12\\WINWORD.EXE",
                                                    0,
                                                    title,
                                                    left,
                                                    top,
                                                    width,
                                                    height);
        }
        /// <summary>
        /// 添加Pdf对象
        /// </summary>
        /// <param name="CurSheet"></param>
        /// <param name="filepath"></param>
        /// <param name="title"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Microsoft.Office.Interop.Excel.Shape AddPdfOLEObject(Excel._Worksheet CurSheet, object filepath, object title, object left, object top, object width, object height)
        {
            return CurSheet.Shapes.AddOLEObject(Type.Missing,
                                                    filepath,
                                                    false,
                                                    true,
                                                    "C:\\Windows\\Installer\\{AC76BA86-7AD7-2052-7B44-AA1000000001}\\PDFFile_8.ico",
                                                    0,
                                                    title,
                                                    left,
                                                    top,
                                                    width,
                                                    height);
        }

        /// <summary>
        /// 另存为xls文件
        /// </summary>
        /// <param name="CurBook">Workbook</param>
        /// <param name="strFilePath">文件路径</param>
        public void Save(Excel._Workbook CurBook, string strFilePath)
        {
            CurBook.SaveCopyAs(strFilePath);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="CurBook">Workbook</param>
        /// <param name="strFilePath">文件路径</param>
        public void SaveAs(Excel._Workbook CurBook, string strFilePath)
        {
            CurBook.SaveAs(strFilePath, mValue, mValue, mValue, mValue, mValue, Excel.XlSaveAsAccessMode.xlShared, mValue, mValue, mValue, mValue, mValue);
        }

        /// <summary>
        /// 另存为html文件
        /// </summary>
        /// <param name="CurBook">Workbook</param>
        /// <param name="strFilePath">文件路径</param>
        public void SaveHtml(Excel._Workbook CurBook, string strFilePath)
        {
            CurBook.SaveAs(strFilePath, Excel.XlFileFormat.xlHtml, mValue, mValue, mValue, mValue, Excel.XlSaveAsAccessMode.xlNoChange, mValue, mValue, mValue, mValue, mValue);
        }

        /// <summary>
        /// 释放内存
        /// </summary>
        public void Dispose(Excel._Worksheet CurSheet, Excel._Workbook CurBook, Excel._Application CurExcel)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(CurSheet);
                CurSheet = null;
                CurBook.Close(false, mValue, mValue);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(CurBook);
                CurBook = null;

                CurExcel.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(CurExcel);
                CurExcel = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (System.Exception ex)
            {
                HttpContext.Current.Response.Write("在释放Excel内存空间时发生了一个错误:" + ex);
            }
            finally
            {
                foreach (System.Diagnostics.Process pro in System.Diagnostics.Process.GetProcessesByName("Excel"))
                    pro.Kill();
            }
            System.GC.SuppressFinalize(this);
        }	
    }
}
