using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Data;
using System.IO;

namespace NPOIHelperTest
{
    public class NPOIHelper
    {
        /// <summary>
        /// DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">保存位置</param>
        public static void DataTableToExcel(DataTable dtSource, string strHeaderText, string strFileName)
        {
            using (MemoryStream ms = DataTableToExcel(dtSource, strHeaderText))
            {
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        public static MemoryStream DataTableToExcel(DataTable dtSource, string strHeaderText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();

            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "东青信息";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "Allen"; //填加xls文件作者信息
                si.ApplicationName = "创建程序信息"; //填加xls文件创建程序信息
                si.LastAuthor = "最后保存者信息"; //填加xls文件最后保存者信息
                si.Comments = "作者信息"; //填加xls文件作者信息
                si.Title = "标题信息"; //填加xls文件标题信息
                si.Subject = "主题信息";//填加文件主题信息
                si.CreateDateTime = System.DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion

            HSSFCellStyle dateStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
            //边框
            dateStyle.BorderBottom = BorderStyle.Thin;
            dateStyle.BorderLeft = BorderStyle.Thin;
            dateStyle.BorderRight = BorderStyle.Thin;
            dateStyle.BorderTop = BorderStyle.Thin;

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = (HSSFSheet)workbook.CreateSheet();
                    }

                    #region 表头及样式
                    {
                        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;


                        HSSFFont font = (HSSFFont)workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;
                        //sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));

                    }
                    #endregion


                    #region 列头及样式
                    {
                        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
                        HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;

                        //边框
                        //headStyle.BorderBottom = BorderStyle.Thin;
                        //headStyle.BorderLeft = BorderStyle.Thin;
                        //headStyle.BorderRight = BorderStyle.Thin;
                        //headStyle.BorderTop = BorderStyle.Thin;

                        HSSFFont font = (HSSFFont)workbook.CreateFont();
                        //font.FontHeightInPoints = 10;
                        //font.Boldweight = 700;
                        //headStyle.SetFont(font);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                        // headerRow.Dispose();
                    }
                    #endregion

                    rowIndex = 1;
                }
                #endregion

                #region 填充内容
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);

                #region 边框,样式

                HSSFCellStyle NewCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                NewCellStyle.Alignment = HorizontalAlignment.Center;

                //边框
                //NewCellStyle.BorderBottom = BorderStyle.Thin;
                //NewCellStyle.BorderLeft = BorderStyle.Thin;
                //NewCellStyle.BorderRight = BorderStyle.Thin;
                //NewCellStyle.BorderTop = BorderStyle.Thin;
                #endregion

                foreach (DataColumn column in dtSource.Columns)
                {
                    HSSFCell newCell = (HSSFCell)dataRow.CreateCell(column.Ordinal);


                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            newCell.CellStyle = NewCellStyle;
                            break;
                        case "System.DateTime"://日期类型
                            System.DateTime dateV;
                            System.DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            newCell.CellStyle = NewCellStyle;
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            newCell.CellStyle = NewCellStyle;
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            newCell.CellStyle = NewCellStyle;
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            newCell.CellStyle = NewCellStyle;
                            break;
                        default:
                            newCell.SetCellValue("");
                            newCell.CellStyle = NewCellStyle;
                            break;
                    }

                }
                #endregion

                rowIndex++;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }


        /// <summary>读取excel
        /// 默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable Import(string strFileName)
        {
            DataTable dt = new DataTable();
            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            HSSFSheet sheet = hssfworkbook.GetSheetAt(0) as HSSFSheet;
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            HSSFRow headerRow = sheet.GetRow(0) as HSSFRow;
            int cellCount = headerRow.LastCellNum;
            for (int j = 0; j < cellCount; j++)
            {
                HSSFCell cell = headerRow.GetCell(j) as HSSFCell;
                dt.Columns.Add(cell.ToString());
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                HSSFRow row = sheet.GetRow(i) as HSSFRow;
                DataRow dataRow = dt.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>读取excel
        /// 默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        //public static DataTable ImportTest(string strFileName)
        //{
        //    var pstr = System.Configuration.ConfigurationManager.AppSettings["pstr"].Split('|').ToList();

        //    DataTable dt = new DataTable();
        //    HSSFWorkbook hssfworkbook;
        //    using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
        //    {
        //        hssfworkbook = new HSSFWorkbook(file);
        //    }
        //    HSSFSheet sheet = hssfworkbook.GetSheetAt(0) as HSSFSheet;
        //    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
        //    HSSFRow headerRow = sheet.GetRow(0) as HSSFRow;
        //    int cellCount = headerRow.LastCellNum;
        //    for (int j = 0; j < cellCount; j++)
        //    {
        //        HSSFCell cell = headerRow.GetCell(j) as HSSFCell;
        //        dt.Columns.Add(cell.ToString() + pstr[j]);
        //    }
        //    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
        //    {
        //        HSSFRow row = sheet.GetRow(i) as HSSFRow;
        //        DataRow dataRow = dt.NewRow();
        //        for (int j = row.FirstCellNum; j < cellCount; j++)
        //        {
        //            if (row.GetCell(j) != null)
        //                dataRow[j] = row.GetCell(j).ToString();
        //        }
        //        dt.Rows.Add(dataRow);
        //    }
        //    return dt;
        //}


    }
}
