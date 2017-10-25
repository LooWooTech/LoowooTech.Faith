using LoowooTech.Faith.Models;
using NPOI.SS.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Common
{
    public static class ExcelManager
    {
        public static NPOI.SS.UserModel.ICell GetCell(IRow row, int line, IRow modelRow = null)
        {
            NPOI.SS.UserModel.ICell cell = row.GetCell(line);
            if (cell == null)
            {
                if (modelRow != null)
                {
                    cell = row.CreateCell(line, modelRow.GetCell(line).CellType);
                    cell.CellStyle = modelRow.GetCell(line).CellStyle;
                }
                else
                {
                    cell = row.CreateCell(line);
                }
            }
            return cell;
        }

        public static XWPFTableCell GetCell(XWPFTableRow row,int line,XWPFTableRow modelRow)
        {
            XWPFTableCell cell= row.GetCell(line);
            if (cell == null)
            {
                cell = row.CreateCell();
                if (modelRow != null)
                {
                    var modelcell = modelRow.GetCell(line);
                    cell.SetVerticalAlignment(modelcell.GetVerticalAlignment());
                    cell.SetColor(modelcell.GetColor());
                } 
            }
            return cell;
        }
        public static XWPFRun GetRun(XWPFTableRow row, int line, XWPFTableRow modelrow)
        {
            var cell = GetCell(row, line, modelrow);
            if (cell.Paragraphs.Count == 0)
            {
                cell.AddParagraph();
            }
            var para = cell.Paragraphs[0];
            XWPFRun run = para.CreateRun();
            run.FontFamily = "仿宋";
            return run;
        }
        public static IWorkbook OpenExcel(this string filePath)
        {
            IWorkbook workbook = null;
            using (var fs=new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                workbook = WorkbookFactory.Create(fs);
            }
            return workbook;
        }

        public static XWPFDocument OpenWord(this string filePath)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                XWPFDocument doc = new XWPFDocument(fs);
                return doc;
            }
        }
        private static NPOI.SS.UserModel.ICell[] GetCells(IRow row,int startline,int endline)
        {
            var cells = new NPOI.SS.UserModel.ICell[endline - startline+1];
            var j = 0;
            for(var i = startline; i <= endline; i++)
            {
                var cell = row.GetCell(i);
                if (cell == null)
                {
                    return null;
                }
                cells[j++] = cell;
            }
            return cells;
        }
        private static string[] GetString(IRow row,int startline,int endline)
        {
            var results = new string[endline - startline+1];
            var j = 0;
            for(var i = startline; i <= endline; i++)
            {
                var cell = row.GetCell(i);
                if (cell == null)
                {
                    return null;
                }
                results[j++] = cell.ToString().Trim();
            }
            return results;
        }

        /// <summary>
        /// 作用：分析一行得到自然人实体类
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日14:52:36
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static Lawyer AnalyzeLawyer(IRow row)
        {
            var cells = new NPOI.SS.UserModel.ICell[11];
            for(var i = 0; i < cells.Length; i++)
            {
                cells[i] = row.GetCell(i);
            }
            var name = cells[0].ToString();
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            var lawyer = new Lawyer
            {
                Name = name,
                BornTime = cells[2].ToString().Trim(),
                Number = cells[4].ToString().Trim(),
                Address = cells[8].ToString().Trim(),
                TelPhone = cells[9].ToString().Trim(),
                EMail=cells[10].ToString().Trim()
            };
            try
            {
                lawyer.Sex = EnumHelper.GetEnum<Sex>(cells[1].ToString().Trim());
                lawyer.Credential = EnumHelper.GetEnum<Credential>(cells[3].ToString().Trim());
            }
            catch
            {

            }
            return lawyer;
        }
        private static LandRecordView AnalyzeLandRecord(IRow row)
        {
            var cells = GetCells(row, 0, 6);
            if (cells == null)
            {
                return null;
            }
            var name = cells[0].ToString().Trim();
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            var a = .0;
            var model = new LandRecordView
            {
                ELName = name,
                Code = cells[1].ToString().Trim(),
                IllegalArea = double.TryParse(cells[2].ToString().Trim(), out a) ? a : .0,
                Area = double.TryParse(cells[3].ToString().Trim(), out a) ? a : .0,
                Score = double.TryParse(cells[5].ToString().Trim(), out a) ? a : .0,
                Remark=cells[6].ToString().Trim()
            };
            return model;
        }
        private static Land AnalyzeLand(IRow row)
        {
            var cells = GetCells(row, 0, 6);
            if (cells == null)
            {
                return null;
            }
            var elName = cells[0].ToString().Trim();
            var name = cells[2].ToString().Trim();
            if (string.IsNullOrEmpty(elName)||string.IsNullOrEmpty(name))
            {
                return null;
            }
            var a = .0;
            DateTime time;
            var land = new Land
            {
                ELName = elName,
                Name = name,
               // Number = cells[7].ToString().Trim(),
                ContractNumber = cells[3].ToString().Trim(),
               // LandNumber = cells[8].ToString().Trim(),
                Area = double.TryParse(cells[5].ToString().Trim(), out a) ? a : .0,
                //Money=double.TryParse(cells[10].ToString().Trim(),out a)?a:.0,
               // Code=cells[10].ToString().Trim(),
                //SignTime=DateTime.TryParse(cells[11].ToString().Trim(),out time)?time:time,
                //ApproveTime=DateTime.TryParse(cells[12].ToString().Trim(),out time)?time:time,
                //Recycle=cells[13].ToString().Trim()=="否"?false:true,
                Location=cells[6].ToString().Trim()
            };
            //var area = cells[8].ToString().Trim();
            //if (!string.IsNullOrEmpty(area))
            //{
            //    if(double.TryParse(area,out a))
            //    {
            //        land.ReplaceArea = a;
            //    }
            //}
            var way = cells[4].ToString().Trim();
            if (!string.IsNullOrEmpty(way))
            {
                try
                {
                    land.Way = EnumHelper.GetEnum<SoldWay>(way);

                }
                catch
                {

                }
            }

            return land;
        }
        private static List<ConductStandard> AnalyzeConductStandard(IRow row,string[] standardName)
        {
            var list = new List<ConductStandard>();
            var cells = GetCells(row, 0, 25);
            if (cells == null)
            {
                return null;
            }
            var landname = cells[2].ToString().Trim();
            var elName = cells[3].ToString().Trim();
            if (string.IsNullOrEmpty(landname)||string.IsNullOrEmpty(elName))
            {
                return null;
            }
            double a = .0;

            for(var i = 4; i <= 25; i++)
            {
                var cell = cells[i];
                if(double.TryParse(cell.ToString().Trim(),out a)&&a>0)
                {
                    list.Add(new ConductStandard
                    {
                        ELName = elName,
                        LandName=landname,
                        Score = a,
                        StandardName = standardName[i - 4]
                    });
                }
            }
            return list;
        }

        /// <summary>
        /// 作用：分析一行 获取企业类型
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日14:59:21
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static Enterprise AnalyzeEnterprise(IRow row)
        {
            var cells = GetCells(row, 0, 16);
            if (cells == null)
            {
                return null;
            }
            var name = cells[0].ToString().Trim();
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            var a = .0;
            var enterprise = new Enterprise
            {
                Name = name,
                OIBC = cells[1].ToString().Trim(),
                USCC = cells[2].ToString().Trim(),
                Address = cells[6].ToString().Trim(),
                Lawyer = cells[7].ToString().Trim(),
                LawNumber = cells[8].ToString().Trim(),
                Number = cells[9].ToString().Trim(),
                Scope = cells[10].ToString().Trim(),
                Type = cells[11].ToString().Trim(),
                Money = double.TryParse(cells[12].ToString(), out a) ? a : .0,
                ContactWay = cells[13].ToString().Trim(),
                Contact = cells[15].ToString().Trim(),
                TelPhone = cells[16].ToString().Trim()
            };
            return enterprise;
        }


        /// <summary>
        /// 作用：分析文件得到自然人列表信息
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日14:51:05
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<Lawyer> AnalyzeLawyer(string filePath,int cityID)
        {
            var list = new List<Lawyer>();
            IWorkbook workbook = filePath.OpenExcel();
            if (workbook != null)
            {
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    for(var i = 1; i <= sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null)
                        {
                            continue;
                        }
                        var lawyer = AnalyzeLawyer(row);
                        if (lawyer != null)
                        {
                            lawyer.CityID = cityID;
                            list.Add(lawyer);
                        }
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 作用：分析文件获取违法用地列表
        /// 作者：汪建龙
        /// 编写时间：2017年3月19日16:44:28
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<LandRecordView> AnalyzeLandRecord(string filePath,int cityID)
        {
            var list = new List<LandRecordView>();
            IWorkbook workbook = filePath.OpenExcel();
            if (workbook != null)
            {
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    for(var i = 1; i <= sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null)
                        {
                            continue;
                        }
                        var landrecord = AnalyzeLandRecord(row);
                        if (landrecord != null)
                        {
                            landrecord.CityID = cityID;
                            list.Add(landrecord);
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 作用：分析获取文件中的供地列表
        /// 作者：汪建龙
        /// 编写时间：2017年3月16日20:09:11
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<Land> AnalyzeLand(string filePath)
        {
            var list = new List<Land>();
            IWorkbook workbook = filePath.OpenExcel();
            if (workbook != null)
            {
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    for(var i = 1; i <= sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null)
                        {
                            continue;
                        }
                        var land = AnalyzeLand(row);
                        if (land != null)
                        {
                            list.Add(land);
                        }
                    }
                }
            }
            return list;
        }

        private static bool IdentifyEnterprise(IRow row)
        {
            var attribues = ClassDescriptionHelper.Enterprises;
            foreach(var att in attribues)
            {

            }
            for(var i = 0; i <= row.LastCellNum; i++)
            {
                var cell = row.GetCell(i);
                if (cell == null)
                {
                    break;
                }
                var str = cell.ToString().Trim();
                if (string.IsNullOrEmpty(str))
                {
                    break;
                }

            }
            return false;
        }

        /// <summary>
        /// 作用：分析企业文件得到企业列表
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日15:00:29
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<Enterprise> AnalyzeEnterprise(string filePath,int cityID)
        {
            var list = new List<Enterprise>();
            IWorkbook workbook = filePath.OpenExcel();
            if (workbook != null)
            {
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    for(var i = 1; i <= sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null)
                        {
                            continue;
                        }
                        try
                        {
                            var enterprise = AnalyzeEnterprise(row);

                            if (enterprise != null)
                            {
                                enterprise.CityID = cityID;
                                list.Add(enterprise);
                            }
                        }
                        catch
                        {

                        }
                      
                    }
                }
            }
            return list;
        }


        public static List<ConductStandard> AnalyzeConduct(string filePath)
        {
            var list = new List<ConductStandard>();
            IWorkbook workbook = filePath.OpenExcel();
            if (workbook != null)
            {
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    var headrow = sheet.GetRow(2);
                    if (headrow == null)
                    {
                        throw new ArgumentException("无诚信行为记录");
                    }
                    var standardNames = GetString(headrow, 4, 24);
                    if (standardNames == null)
                    {
                        throw new ArgumentException("未获取诚信行为记录信息");
                    }
                    for(var i = 3; i <= sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null)
                        {
                            continue;
                        }
                        var temp = AnalyzeConductStandard(row, standardNames);
                        if (temp != null && temp.Count > 0)
                        {
                            list.AddRange(temp);
                        }
                    }
                }
            }
            return list;
        }

        
        public static IWorkbook SaveConduct(List<ConductStandard> list)
        {
            IWorkbook workbook = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Excels", System.Configuration.ConfigurationManager.AppSettings["Conduct"]).OpenExcel();
            if (workbook != null)
            {
                ISheet sheet = workbook.GetSheetAt(0);
                var startline = 1;
                if (sheet != null)
                {
                    var modelrow = sheet.GetRow(startline);
                    foreach(var conduct in list)
                    {
                        var row = sheet.GetRow(startline);
                        if (row == null)
                        {
                            row = sheet.CreateRow(startline);
                        }
                        startline++;
                        var cell = GetCell(row, 0, modelrow);
                        cell.SetCellValue(conduct.Code);
                        GetCell(row, 1, modelrow).SetCellValue(conduct.LandNumber);
                        GetCell(row, 2, modelrow).SetCellValue("330421");
                        GetCell(row, 3, modelrow).SetCellValue(conduct.StandardName);
                        if (conduct.SystemData == SystemData.Enterprise)
                        {
                            GetCell(row, 8, modelrow).SetCellValue(conduct.ELName);
                        }
                        else
                        {
                            GetCell(row, 7, modelrow).SetCellValue(conduct.ELName);
                        }
                        GetCell(row, 9, modelrow).SetCellValue(conduct.LandName);
                    }
                }
            }
            return workbook;
        }

        /// <summary>
        /// 作用：保存企业相关数据
        /// 作者：汪建龙
        /// 编写时间：2017年4月12日13:33:44
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IWorkbook SaveEnterprise(List<Enterprise> list)
        {
            IWorkbook workbook = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Excels", System.Configuration.ConfigurationManager.AppSettings["Enterprise"]).OpenExcel();
            if (workbook != null)
            {
                ISheet sheet = workbook.GetSheetAt(0);
                var startline = 1;
                if (sheet != null)
                {
                    var modelRow = sheet.GetRow(startline);
                    foreach(var enterprise in list)
                    {
                        var row = sheet.GetRow(startline);
                        if (row == null)
                        {
                            row = sheet.CreateRow(startline);
                        }
                        startline++;
                        var cell = GetCell(row, 0, modelRow);
                        cell.SetCellValue(enterprise.Name);
                        GetCell(row, 1, modelRow).SetCellValue(enterprise.OIBC);
                        GetCell(row, 2, modelRow).SetCellValue(enterprise.USCC);
                        GetCell(row, 3, modelRow).SetCellValue("浙江省");
                        GetCell(row, 4, modelRow).SetCellValue("嘉兴市");
                        GetCell(row, 5, modelRow).SetCellValue("嘉善县");
                        GetCell(row, 6, modelRow).SetCellValue(enterprise.Address);
                        GetCell(row, 7, modelRow).SetCellValue(enterprise.Lawyer);
                        GetCell(row, 8, modelRow).SetCellValue(enterprise.LawNumber);
                        GetCell(row, 9, modelRow).SetCellValue(enterprise.Number);
                        GetCell(row, 10, modelRow).SetCellValue(enterprise.Scope);
                        GetCell(row, 11, modelRow).SetCellValue(enterprise.Type);
                        GetCell(row, 12, modelRow).SetCellValue(enterprise.Money);
                        GetCell(row, 13, modelRow).SetCellValue(enterprise.TelPhone);
                        //GetCell(row, 14, modelRow).SetCellValue();
                        GetCell(row, 15, modelRow).SetCellValue(enterprise.Contact);
                        GetCell(row, 16, modelRow).SetCellValue(enterprise.ContactWay);
                    }
                }
            }
            return workbook;
        }
    }
}