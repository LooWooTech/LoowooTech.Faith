using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoowooTech.Faith.Controllers
{
    public class FileController : ControllerBase
    {
        // GET: File
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(int sid, FileType type=FileType.Conduct)
        {
            ViewBag.Type = type;
            return View();
        }




        [HttpPost]
        public ActionResult Upload(int sid,FileType type,string description)
        {
            if (Request.Files.Count == 0)
            {
                throw new ArgumentException("请选择上传文件！");
            }
            var name = Core.FileManager.GetName(sid, type);
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("未找到相关用地主体信息");
            }
            var saveFile = FileManager.Upload2(Request.Files[0], name);
            var id = Core.FileManager.Add(new FaithFile
            {
                Name = System.IO.Path.GetFileNameWithoutExtension(saveFile),
                Path = saveFile,
                SID = sid,
                Type = type,
                Description=description
            });
            if (id > 0)
            {
                return RedirectToAction("Detail", new { SID = sid, Type = type });
            }
            throw new ArgumentException("上传文件失败");
           
        }

        public ActionResult Detail(int SID,FileType type=FileType.Conduct)
        {
            var list = Core.FileManager.Get(SID, type);
            ViewBag.List = list;
            ViewBag.Name = Core.FileManager.GetName(SID, type);
            return View();
        }

        public ActionResult DetailWindows(int sid,FileType type = FileType.Conduct)
        {
            var list = Core.FileManager.Get(sid, type);
            ViewBag.List = list;
            ViewBag.Name = Core.FileManager.GetName(sid, type);
            return View();
        }


        public ActionResult Edit(int sid,FileType type = FileType.Conduct)
        {
            var list = Core.FileManager.Get(sid, type);
            ViewBag.List = list;
            ViewBag.Name = Core.FileManager.GetName(sid, type);
            return View();
        }


        [HttpPost]
        public ActionResult Delete(int[] id)
        {
            Core.FileManager.Delete(id);

            return SuccessJsonResult();
        }

    
        public ActionResult DetailOne(int id)
        {
            var model = Core.FileManager.Get(id);
            ViewBag.Model = model;
            return View();
        }


       
    }
}