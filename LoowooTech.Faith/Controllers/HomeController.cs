using LoowooTech.Faith.Common;
using LoowooTech.Faith.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoowooTech.Faith.Controllers
{

    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            if (!Identity.IsAuthenticated)
            {
                return Redirect("/User/Login");
            }


            //ViewBag.ECount = Core.EnterpriseManager.Count(City.ID);
            //ViewBag.LCount = Core.LawyerManager.Count(City.ID);
            //ViewBag.LECount = Core.LawyerManager.CountEnterprise(City.ID);
            //ViewBag.LandCount = Core.LandManager.Count(City.ID);
            //ViewBag.LandRecordCount = Core.LandRecordManager.Count(City.ID);
            //ViewBag.Black = Core.RollViewManager.Count(Models.BREnum.Black,City.ID);
            //ViewBag.Red = Core.RollViewManager.Count(Models.BREnum.Red,City.ID);
            //ViewBag.CCount = Core.ConductStandardManager.Count(City.ID);
            //ViewBag.FCount = Core.FeedManager.Count(City.ID);
            return View();
        }

        public ActionResult Index2()
        {
            if (!Identity.IsAuthenticated)
            {
                return Redirect("/User/Login");
            }


            ViewBag.ECount = Core.EnterpriseManager.Count(City.ID);
            ViewBag.LCount = Core.LawyerManager.Count(City.ID);
            ViewBag.LECount = Core.LawyerManager.CountEnterprise(City.ID);
            ViewBag.LandCount = Core.LandManager.Count(City.ID);
            ViewBag.LandRecordCount = Core.LandRecordManager.Count(City.ID);
            ViewBag.Black = Core.RollViewManager.Count(Models.BREnum.Black, City.ID);
            ViewBag.Red = Core.RollViewManager.Count(Models.BREnum.Red, City.ID);
            ViewBag.CCount = Core.ConductStandardManager.Count(City.ID);
            ViewBag.FCount = Core.FeedManager.Count(City.ID);
            return View();
        }


        public ActionResult Index3()
        {
            return View();
        }


        public ActionResult Search()
        {
            return View();
        }

        public ActionResult SearchEnterprise(string key,int page=1)
        {
            var parameter = new EnterpriseParameter
            {
                Name = key,
                Page = new PageParameter(page,20)
            };
            var list = Core.EnterpriseManager.Search(parameter);
            parameter.SearchEndTime = DateTime.Now;
            ViewBag.List = list;
            ViewBag.Parameter = parameter;
            return View();
        }
        public ActionResult DetailEnterprise(int id)
        {
            var model = Core.EnterpriseManager.Get(id);
            ViewBag.Model = model;
            return View();
        }


        public ActionResult DetailLawyer(int id)
        {
            var model = Core.LawyerManager.Get(id);
            ViewBag.Model = model;
            return View();
        }

        public ActionResult SearchLawyer(string key,int page=1)
        {
            var parameter = new LawyerParameter
            {
                Name = key,
                Page = new PageParameter(page,20),
                
            };
            var list = Core.LawyerManager.Search(parameter);
            parameter.SearchEndTime = DateTime.Now;
            ViewBag.Parameter = parameter;
            ViewBag.List = list;
            return View();
        }
    }
}