using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
using LoowooTech.Faith.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoowooTech.Faith.Controllers
{
    public class LandRecordController : ControllerBase
    {
        // GET: LandRecord
        public ActionResult Index(
            string code=null,string ELName=null,
            double? minScore=null,double ? maxScore=null,
            int page=1,int rows=20)
        {
            var parameter = new LandRecordViewParameter
            {
                Code = code,
                ELName = ELName,
                MinScore = minScore,
                MaxScore = maxScore,
                CityID=City.ID,
                Page = new PageParameter(page, rows)
            };
            try
            {
                var list = Core.LandRecordViewManager.Search(parameter);
                ViewBag.List = list;
                ViewBag.Parameter = parameter;
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.ToString());
            }
     
            return View();
        }

        public ActionResult Widget()
        {
            var parameter = new LandRecordViewParameter
            {
                CityID=City.ID,
                Page = new PageParameter(1, 5)
            };
            var list = Core.LandRecordViewManager.Search(parameter);
            ViewBag.List = list;
            ViewBag.Parameter = parameter;
            return View();
        }

        public ActionResult Create(int id=0,SystemData? systemdata=null,int? ELID=null,string name=null)
        {
            var model = Core.LandRecordViewManager.Get(id);

            if (model==null&&systemdata.HasValue && ELID.HasValue)
            {
                model = new LandRecordView
                {
                    ELID = ELID.Value,
                    SystemData = systemdata.Value,
                    ELName = name
                };
            }
            ViewBag.Model = model;
            return View();
        }

        [HttpPost]
        public ActionResult Create(LandRecord record)
        {
            if (record == null)
            {
                return ErrorJsonResult("未获取违法用地信息");
            }
            if (record.IllegalArea > record.Area)
            {
                return ErrorJsonResult("违法用地面积大于合法用地面积，请核对");
            }
            if (record.ID > 0)
            {
                if (!Core.LandRecordManager.Edit(record))
                {
                    return ErrorJsonResult("编辑更新违法用地信息失败");
                }
            }
            else
            {
                var id = Core.LandRecordManager.Save(record);
                if (id <= 0)
                {
                    return ErrorJsonResult("保存违法用地信息失败");
                }
            }
            return SuccessJsonResult();
        }

        public ActionResult DetailList(int ELID,SystemData systemData)
        {
            var list = Core.LandRecordManager.Get(ELID, systemData);
            ViewBag.List = list;
            return View();
        }

        public ActionResult Delete(int id)
        {
            if (!Core.LandRecordManager.Delete(id))
            {
                return ErrorJsonResult("删除失败，未找到相关违法用地信息");
            }
            return SuccessJsonResult();
        }

        public ActionResult File()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AnalyzeFile()
        {
            if (Request.Files.Count == 0)
            {
                throw new ArgumentException("请选择上传文件");
            }
            var file = HttpContext.Request.Files[0];
            var fileName = file.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("请选择上传文件");
            }
            var filePath = FileManager.Upload(file);
            var list = ExcelManager.AnalyzeLandRecord(filePath,City.ID);
            Core.LandRecordViewManager.AddRange(list,Identity.UserID);
            return RedirectToAction("Index");
        }
        public ActionResult Relieve(int id)
        {
            var model = Core.LandRecordViewManager.Get(id);
            ViewBag.Model = model;
            return View();
        }

        [HttpPost]
        public ActionResult Relieve(int id,string remark)
        {
            if (!Core.LandRecordManager.Relieve(id, remark))
            {
                return ErrorJsonResult("解除违法用地失败，未找到违法用地相关信息");
            }
            return SuccessJsonResult();
        }

        public ActionResult CancelRelieve(int id)
        {
            if (!Core.LandRecordManager.CancelRelieve(id))
            {
                return ErrorJsonResult("取消解除失败，未找到违法用地相关信息");
            }
            return SuccessJsonResult();
        }
    }
}