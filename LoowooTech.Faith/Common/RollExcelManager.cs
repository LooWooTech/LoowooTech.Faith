using LoowooTech.Faith.Models;
using NPOI.SS.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Common
{
    public static class RollExcelManager
    {
        private static string _modelExcelName { get; set; }
        private static string _modelExcelPath { get; set; }


        private static string _modelScorePath { get; set; }
        private static string _modelBlackPath { get; set; }
        private static string _modelNotificationPath { get; set; }
        private static string _modelProtocolPath { get; set; }
        private static string _modeJiaXingPath { get; set; }
        static RollExcelManager()
        {
            _modelExcelName = System.Configuration.ConfigurationManager.AppSettings["Roll"] ?? "Roll.xls";
            _modelExcelPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Excels", _modelExcelName);
            _modelScorePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Excels", System.Configuration.ConfigurationManager.AppSettings["Score"] ?? "Score.xls");
            _modelBlackPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Excels", System.Configuration.ConfigurationManager.AppSettings["BLACK"] ?? "Black.docx");
            _modelNotificationPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Excels", System.Configuration.ConfigurationManager.AppSettings["Notification"] ?? "Notification.docx");
            _modelProtocolPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Excels", System.Configuration.ConfigurationManager.AppSettings["Protocol"] ?? "Protocol.docx");
            _modeJiaXingPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Excels", System.Configuration.ConfigurationManager.AppSettings["JiaXing"] ?? "JiaXing.xls");
        }
        public static IWorkbook SaveJiaXing(List<Enterprise> list)
        {
            if (!System.IO.File.Exists(_modeJiaXingPath))
            {
                return null;
            }
            IWorkbook workbook = _modeJiaXingPath.OpenExcel();
            if (workbook != null)
            {
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    var modelRow = sheet.GetRow(1);
                    var startLine = 1;
                    foreach(var item in list.OrderByDescending(e=>e.Degree))
                    {
                        var row = sheet.GetRow(startLine) ?? sheet.CreateRow(startLine);
                        startLine++;
                        var cell = ExcelManager.GetCell(row, 0, modelRow);
                        cell.SetCellValue(item.Degree.GetDescription());
                        ExcelManager.GetCell(row, 1, modelRow).SetCellValue(item.Name);
                        ExcelManager.GetCell(row, 2, modelRow).SetCellValue(item.USCC);
                        ExcelManager.GetCell(row, 3, modelRow).SetCellValue(item.Lawyer);
                        ExcelManager.GetCell(row, 4, modelRow).SetCellValue(item.LawNumber);
                        ExcelManager.GetCell(row, 5, modelRow);
                        ExcelManager.GetCell(row, 6, modelRow);
                        ExcelManager.GetCell(row, 7, modelRow);
                        ExcelManager.GetCell(row, 8, modelRow).SetCellValue(string.Join(";", item.ConductStandards.Select(e => e.StandardName).ToArray()));
                    }
                }
            }
            return workbook;
        }

        public static IWorkbook SaveScore(List<EnterpriseScore> enterprises,List<LawyerScore> lawyers)
        {
            if (!System.IO.File.Exists(_modelScorePath))
            {
                return null;
            }
            IWorkbook workbook = _modelScorePath.OpenExcel();
            if (workbook != null)
            {
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    var modelrow = sheet.GetRow(0);
                    Write<EnterpriseScore>(enterprises, ref sheet, modelrow);
                }
                var sheet2 = workbook.GetSheetAt(1);
                if (sheet2 != null)
                {
                    var modelRow = sheet2.GetRow(0);
                    Write<LawyerScore>(lawyers, ref sheet2, modelRow);
                }
            }
            return workbook;
        }
        /// <summary>
        /// 作用：生成表单
        /// </summary>
        /// <param name="rollViews"></param>
        /// <returns></returns>
        public static IWorkbook SaveRoll(List<RollList> list)
        {
            if (!System.IO.File.Exists(_modelExcelPath))
            {
                return null;
            }
            IWorkbook workbook = _modelExcelPath.OpenExcel();
            if (workbook == null)
            {
                return null;
            }
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet != null)
            {
                var modelrow = sheet.GetRow(2);
                WriteTime(ref sheet);
                Write(list, ref sheet, modelrow);
            }
            //ISheet sheet2 = workbook.GetSheetAt(1);
            //if (sheet2 != null)
            //{
            //    var modelrow = sheet2.GetRow(2);
            //    WriteTime(ref sheet2);
            //    Write(rollViews.Where(e => e.BREnum == BREnum.Black).ToList(), ref sheet2, modelrow);
            //}
            return workbook;
        }
        /// <summary>
        /// 作用：填写时间：
        /// 作者：汪建龙
        /// 编写时间：2017年3月8日15:48:22
        /// </summary>
        /// <param name="sheet"></param>
        private static void WriteTime(ref ISheet sheet)
        {
            IRow row = sheet.GetRow(0);
            if (row == null)
            {
                row = sheet.CreateRow(0);
            }
            NPOI.SS.UserModel.ICell cell = row.GetCell(3);
            if (cell == null)
            {
                cell = row.CreateCell(3);
            }
            cell.SetCellValue(DateTime.Now.ToString("yyyy-MM-dd"));
        }

        private static void Write(List<RollList> list,ref ISheet sheet,IRow modelRow)
        {
            var startLine = 2;
            var serial = 1;
            foreach(var item in list)
            {
                var row = sheet.GetRow(startLine);
                if (row == null)
                {
                    row = sheet.CreateRow(startLine);
                }
                startLine++;
                var cell = ExcelManager.GetCell(row, 0, modelRow);
                cell.SetCellValue(serial++);
                ExcelManager.GetCell(row, 1, modelRow).SetCellValue(item.Name);
            }
        }

        private static void Write<T>(List<T> list,ref ISheet sheet,IRow modelRow) where T:BaseScore
        {
            var startLine = 1;
            foreach(var item in list)
            {
                var row = sheet.GetRow(startLine);
                if (row == null)
                {
                    row = sheet.CreateRow(startLine);
                }
                startLine++;
                var cell = ExcelManager.GetCell(row, 0, modelRow);
                cell.SetCellValue(item.Name);
                ExcelManager.GetCell(row, 1, modelRow).SetCellValue(item.Times.HasValue ? item.Times.Value : 0);
                ExcelManager.GetCell(row, 2, modelRow).SetCellValue(item.ScoreValue ?? 0);
                ExcelManager.GetCell(row, 3, modelRow).SetCellValue(item.Average);
                ExcelManager.GetCell(row, 4, modelRow).SetCellValue(item.Record ?? 0);
                ExcelManager.GetCell(row, 5, modelRow).SetCellValue(item.DeDuck);
                ExcelManager.GetCell(row, 6, modelRow).SetCellValue(item.Degree.ToString());
            }
        }
        public static XWPFDocument SaveWord(Letter letter)
        {
            XWPFDocument doc=null;
            switch (letter.Book)
            {
                case Book.Notification:
                    doc=_modelNotificationPath.OpenWord();
                    break;
                case Book.Protocol:
                    doc = _modelProtocolPath.OpenWord();
                    break;
            }
            if (doc != null)
            {
                var paras = doc.Paragraphs;
                foreach(var para in paras)
                {
                    var text = para.ParagraphText;
                    var runs = para.Runs;
                    for(var i = 0; i < runs.Count; i++)
                    {
                        var run = runs[i];
                        var str = run.Text
                            .Replace("{Number}", letter.Number)
                            .Replace("{Name}", letter.Name)
                            .Replace("{Credit}", letter.Credit)
                            .Replace("{Description}", letter.Description)
                            .Replace("{Contact}", letter.Contact)
                            .Replace("{TelPhone}", letter.TelPhone)
                            .Replace("{Time}", letter.Time)
                            .Replace("{Signature}", letter.Signature);
                        run.SetText(str);
                    }
                }
            }
            return doc;
        }

        public static XWPFDocument SaveWord(List<RollList> list)
        {
            if (!System.IO.File.Exists(_modelBlackPath))
            {
                return null;
            }
            XWPFDocument doc = _modelBlackPath.OpenWord();
            if (doc != null)
            {
                var paras = doc.Paragraphs;
                foreach(var para in paras)
                {
                    var text = para.ParagraphText;
                    var runs = para.Runs;
                    for(var i = 0; i < runs.Count; i++)
                    {
                        var run = runs[i];
                        var str=run.Text.Replace("{Time}", DateTime.Now.ToLongDateString());
                        run.SetText(str);
                    }
                }
                var tables = doc.Tables;
                var table = tables[0];
              
                var modelrow = table.GetRow(0);
                var startline = 1;
                foreach(var item in list)
                {
                    XWPFTableRow row = table.GetRow(startline);
                    if (row == null)
                    {
                        row = table.CreateRow();
                    }
                    var run = ExcelManager.GetRun(row, 0, modelrow);
                    run.SetText((startline++).ToString());
                    ExcelManager.GetRun(row, 1, modelrow).SetText(item.Name);
                    if (item.ConductStandards != null)
                    {
                        var array = item.ConductStandards.Select(e => e.ContractNumber).Distinct().ToArray();
                        var array2 = item.ConductStandards.Select(e => string.Format("{0},应记{1}分", e.StandardName, e.Score)).ToArray();
                        ExcelManager.GetRun(row, 2, modelrow).SetText(string.Join(";", array));
                        ExcelManager.GetRun(row, 4, modelrow).SetText(item.ConductStandards.Sum(e => e.Area).ToString());
                        ExcelManager.GetRun(row, 5, modelrow).SetText(array2.Length>0? string.Format("涉及{0}", string.Join(";", array2)):"");
                    }


                }
            }
            return doc;
        }
    }
}