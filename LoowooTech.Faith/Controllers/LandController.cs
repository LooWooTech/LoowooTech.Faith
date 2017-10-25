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
    public class LandController : ControllerBase
    {
        // GET: Land
        public ActionResult Index(
            string name=null,string sName=null,
            string number=null,string contractNumber=null,
            SoldWay? way=null,LandOrder? order=null, int page=1,int rows=20)
        {
            var parameter = new LandViewParameter
            {
                Name=name,
                sName=sName,
                Number=number,
                ContractNumber=contractNumber,
                Way=way,
                CityID=City.ID,
                Order=order,
                Page = new PageParameter(page, rows)
            };
            var list = Core.LandManager.Search(parameter);
            ViewBag.List = list;
            ViewBag.Parameter = parameter;
            return View();
        }

        [ChildActionOnly]
        public ActionResult Widget()
        {
            var parameter = new LandViewParameter
            {
                CityID=City.ID,
                Page = new PageParameter(1, 5)
            };
            var list = Core.LandManager.Search(parameter);
            ViewBag.List = list;
            ViewBag.Parameter = parameter;
            return View();
        }

        public ActionResult Create(int id=0)
        {
            var model = Core.LandManager.GetView(id);
            ViewBag.Model = model;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Land land)
        {
            if (land == null)
            {
                return ErrorJsonResult("未获取供地信息");
            }
            if (land.ID > 0)
            {
                if (!Core.LandManager.Edit(land))
                {
                    return ErrorJsonResult("编辑更新失败，未找到需要编辑更新的供地信息记录");
                }
            }
            else
            {
                if (Core.LandManager.Exist(land.Name))
                {
                    return ErrorJsonResult("系统中存在相同的项目名称的供地信息记录");
                }
                var id = Core.LandManager.Save(land);
                if (id <= 0)
                {
                    return ErrorJsonResult("保存供地信息失败！");
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
            var list = ExcelManager.AnalyzeLand(filePath);
            Core.LandManager.AddRange(list, Identity.UserID,City.ID);
            try
            {
              

            }
            catch
            {

            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 作用：删除供地信息
        /// 作者：汪建龙
        /// 编写时间：2017年3月18日13:11:45
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            if (!Core.LandManager.Delete(id))
            {
                return ErrorJsonResult("删除失败，未找到需要删除的供地信息");
            }
            return SuccessJsonResult();
        }

        public ActionResult DetailList(int elid,SystemData systemData)
        {
            var list = Core.LandManager.Search(new LandViewParameter() { ELID = elid, SystemData = systemData });
            ViewBag.List = list;
            return View();
        }

        public ActionResult Detail(int id)
        {
            var model = Core.LandManager.GetView(id);
            ViewBag.Model = model;
            return View();
        }
    }
}