using LoowooTech.Faith.Models;
using LoowooTech.Faith.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoowooTech.Faith.Controllers
{
    public class FeedController : ControllerBase
    {
        // GET: Feed
        public ActionResult Index(
            string elName=null,bool? hasRead=null, GradeDegree?Old=null,GradeDegree?New=null, 
            int page=1,int rows=20)
        {
             var parameter = new FeedParameter
            {
                ELName=elName,
                Old=Old,
                New=New,
                HasRead = hasRead,
                CityID=City.ID,
                Page = new PageParameter(page, rows)
            };
            var list = Core.FeedManager.Search(parameter);
            ViewBag.List = list;
            ViewBag.Parameter = parameter;
            return View();
        }

        public ActionResult Read(int id)
        {
            if (!Core.FeedManager.Read(id))
            {
                throw new ArgumentException("未找到标记已读的消息");
            }

            return RedirectToAction("Index");
        }




    }
}