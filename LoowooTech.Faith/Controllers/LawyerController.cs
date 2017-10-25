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
    [UserAuthorize]
    public class LawyerController : ControllerBase
    {
   
        public ActionResult Index(
            string name=null,string sex=null,
            string bornTime=null,string number=null,
            string address=null,string telphone=null,GradeDegree?degree=null,
            string email=null, int page=1,int rows=20)
        {
            var parameter = new LawyerParameter
            {
                Name=name,
                BornTime=bornTime,
                Number=number,
                Address=address,
                TelPhone=telphone,
                Email=email,
                Degree=degree,
                Deleted=false,
                CityID=City.ID,
                Page = new PageParameter(page, rows)
            };
            if (!string.IsNullOrEmpty(sex))
            {
                parameter.Sex = EnumHelper.GetEnum<Sex>(sex);
            }
            var list = Core.LawyerManager.Search(parameter);
            ViewBag.List = list;
            ViewBag.Parameter = parameter;
            ViewBag.Page = parameter.Page;
            return View();
        }

        [ChildActionOnly]
        public ActionResult Widget()
        {
            var parameter = new LawyerParameter
            {
                CityID = City.ID,
                Page = new PageParameter(1, 5)
            };
            var list = Core.LawyerManager.Search(parameter);
            ViewBag.List = list;
            ViewBag.Parameter = parameter;
            return View();
        }

        /// <summary>
        /// 作用：保存或更新自然人信息
        /// 作者：汪建龙
        /// 编写时间：2017年3月5日15:29:15
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(int id=0,string url=null)
        {
            var model = Core.LawyerManager.Get(id);
            ViewBag.Lawyer = model;
            ViewBag.URL = url;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Lawyer lawyer)
        {
            if (lawyer == null)
            {
                return ErrorJsonResult("未获取自然人信息");
            }
            lawyer.CityID = City.ID;
            if (lawyer.ID > 0)
            {
                if (!Core.LawyerManager.Edit(lawyer))
                {
                    return ErrorJsonResult($"更新自然人失败，未找到ID为{lawyer.ID}的自然人信息");
                }
            }
            else
            {
                var id = Core.LawyerManager.Save(lawyer);
                if (id <= 0)
                {
                    return ErrorJsonResult("保存失败");
                }
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
            var list = ExcelManager.AnalyzeLawyer(filePath,City.ID);
            Core.LawyerManager.AddRange(list, Identity.UserID);
            return RedirectToAction("Index");
        }


        public ActionResult Delete(int id)
        {
            var model = Core.LawyerManager.Get(id);
            ViewBag.Model = model;
            return View();
        }




        /// <summary>
        /// 作用：删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id,string remark)
        {
            if (Core.LawyerManager.Used(id))
            {
                return ErrorJsonResult("删除失败，当前自然人已经管理诚信行为以及违法用地记录");
            }
            if (!Core.LawyerManager.Delete(id,remark))
            {
                return ErrorJsonResult("删除失败，未找到删除ID");
            }
            return SuccessJsonResult();
        }

        public ActionResult Recycle(int id)
        {
            if (!Core.LawyerManager.Recycle(id))
            {
                return ErrorJsonResult("还原自然人信息失败，未找到需要还原的自然人信息");
            }
            return SuccessJsonResult();
        }

        public ActionResult Detail(int id)
        {
            var lawyer = Core.LawyerManager.Get(id);
            ViewBag.Model = lawyer;
            return View();
        }

        public ActionResult DetailLawyer(int id)
        {
            var model = Core.LawyerManager.Get(id);
            ViewBag.Model = model;
            return View();
        }

        public ActionResult Translate(int id)
        {
            var model = Core.LawyerManager.Get(id);
            ViewBag.Model = model;
            return View();
        }
    }

}