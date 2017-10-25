using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
using LoowooTech.Faith.Parameters;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoowooTech.Faith.Controllers
{
    [UserAuthorize]
    public class EnterpriseController : ControllerBase
    {
        // GET: Enterprise
        public ActionResult Index(
            string name=null,string oibc=null,
            string uscc=null,string address=null,
            string lawyer=null,string lawnumber=null,
            string number=null,string scope=null,
            string type=null,double? minmoney=null,double? maxMoney=null,
            string contact=null,string telphone=null,GradeDegree?degree=null,
            int page=1,int rows=20)
        {
            var parameter = new EnterpriseParameter
            {
                Name = name,
                OIBC = oibc,
                USCC = uscc,
                Address = address,
                Lawyer = lawyer,
                LawNumber = lawnumber,
                Number = number,
                Scope = scope,
                Type = type,
                Contact = contact, TelPhone = telphone,
                MinMoney = minmoney,
                MaxMoney = maxMoney,
                Degree = degree,
                Deleted = false,
                CityID=City.ID,
                Page = new PageParameter(page, rows)
            };
            var list = Core.EnterpriseManager.Search(parameter);
            ViewBag.Types = Core.EnterpriseManager.GetEnterpriseType();
            ViewBag.Parameter = parameter;
            ViewBag.Page = parameter.Page;
            ViewBag.List = list;
            return View();
        }


        public ActionResult Create(int id=0)
        {
            var model = Core.EnterpriseManager.Get(id);
            ViewBag.Model = model;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Enterprise enterprise)
        {
            if (enterprise == null)
            {
                return SuccessJsonResult("未获取企业信息");
            }
            enterprise.CityID = City.ID;
            if (enterprise.ID > 0)
            {
                if (!Core.EnterpriseManager.Edit(enterprise))
                {
                    return ErrorJsonResult($"更新企业信息失败，未找到ID为{enterprise.ID}的企业信息");
                }
            }
            else
            {
                Lawyer lawyer = null;
                if (enterprise.LawyerID.HasValue)
                {
                    lawyer = Core.LawyerManager.Get(enterprise.LawyerID.Value);
                    if (lawyer == null)
                    {
                        return ErrorJsonResult("录入企业的自然人LawyerID不正确");
                    }
                }
                var id = Core.EnterpriseManager.Save(enterprise);
                if (id <= 0)
                {
                    return ErrorJsonResult("保存失败");
                }
                if (lawyer != null)
                {
                    lawyer.EnterpriseID = id;
                    if (!Core.LawyerManager.Edit(lawyer))
                    {
                        return ErrorJsonResult("更新自然人的企业ID失败");
                    }
                }

            }
            return SuccessJsonResult(enterprise.ID);
        }

        [ChildActionOnly]
        public ActionResult Widget()
        {
            var parameter = new EnterpriseParameter
            {
                CityID = City.ID,
                Page = new PageParameter(1, 5)
            };
            var list = Core.EnterpriseManager.Search(parameter);
            ViewBag.List = list;
            ViewBag.Parameter = parameter;
            return View();
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
                throw new AggregateException("请选择上传文件");
            }
            var filePath = FileManager.Upload(file);
            var list = ExcelManager.AnalyzeEnterprise(filePath,City.ID);
            Core.EnterpriseManager.AddRange(list, Identity.UserID);
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int id)
        {
            var model = Core.EnterpriseManager.Get(id);
            if (model != null)
            {
                model.Conducts = Core.ConductManager.Get(model.ID, SystemData.Enterprise);
            }
            ViewBag.Model = model;
            return View();
        }
        public ActionResult DetailEnterprise(int id)
        {
            var model = Core.EnterpriseManager.Get(id);
            ViewBag.Model = model;
            return View();
        }

        public ActionResult Delete(int id)
        {
            var model = Core.EnterpriseManager.Get(id);
            ViewBag.Model = model;
            return View();
        }
        
        [HttpPost]
        public ActionResult Delete(int id,string remark)
        {
            if (Core.EnterpriseManager.Used(id))
            {
                return ErrorJsonResult("删除失败，当前企业已经关联了违法用地或者诚信行为记录");
            }
            if (!Core.EnterpriseManager.Delete(id,remark))
            {
                return ErrorJsonResult("删除失败，未找到需要删除的企业信息");
            }
            return SuccessJsonResult();
        }

        public ActionResult Recycle(int id)
        {
            if (!Core.EnterpriseManager.Recycle(id))
            {
                return ErrorJsonResult("还原企业信息失败，未找到企业相关信息");
            }
            return SuccessJsonResult();
        }

        public ActionResult Download()
        {
            var list = Core.EnterpriseManager.Search(new EnterpriseParameter { Deleted = false,CityID=City.ID });
            IWorkbook workbook = ExcelManager.SaveEnterprise(list);
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, "application/ms-excel", "企业.xls");
        }

        public ActionResult Modify(int id)
        {
            var model = Core.EnterpriseManager.Get(id);
            return View();
        }

        
    }
}