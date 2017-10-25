using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
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
    public class GradeController : ControllerBase
    {
        // GET: Grade
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 作用：对企业和自然人进行信用评级
        /// 作者：汪建龙
        /// 编写时间：2017年3月28日17:12:26
        /// </summary>
        /// <returns></returns>
        public ActionResult Calculate()
        {
            Core.GradeManager.Grade2(City.ID);
            return SuccessJsonResult();
        }

        public ActionResult Download(GradeDegree? degree=null)
        {
            var enterprise = Core.ScoreManager.GetEnterprise(City.ID);
            var lawyers = Core.ScoreManager.GetLawyer(City.ID);
            if (degree.HasValue)
            {
                enterprise = enterprise.Where(e => e.Degree == degree.Value).ToList();
                lawyers = lawyers.Where(e => e.Degree == degree.Value).ToList();
            }
            IWorkbook workbook = RollExcelManager.SaveScore(enterprise,lawyers);
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, "application/ms-excel", "综合分值表.xls");
        }

        public ActionResult Score(int ELID,SystemData systemData)
        {
            var model = Core.ScoreManager.Get(ELID, systemData);
            ViewBag.Model = model;
            return View();
        }
        public ActionResult ChangeDegree(int id,SystemData data)
        {
            if (Identity.Role != UserRole.Administrator)
            {
                throw new ArgumentException("当前您没有权限更改企业的信用等级评分，如需更改请联系系统管理人员！");
            }
            switch (data)
            {
                case SystemData.Enterprise:
                    break;
                case SystemData.Lawyer:
                    break;
            }

            return View();
        }

        public ActionResult ChangeDegree(CreditDegree degree,string remark)
        {
            return SuccessJsonResult();
        }


        public ActionResult List()
        {
            var enterprises = Core.EnterpriseManager.GetFull(City.ID);
            var workbook = RollExcelManager.SaveJiaXing(enterprises);
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, "application/ms-excel", "嘉兴名单列表.xls");
        }

        public ActionResult Year()
        {
            var list = Core.GradeHistoryManager.GetList(City.ID);
            ViewBag.List = list;
            return View();
        }


        public ActionResult Create(int id=0)
        {
            var model = Core.GradeHistoryManager.Get(id);
            ViewBag.Model = model;
            return View();
        }

        [HttpPost]
        public ActionResult Save(GradeHistory grade)
        {
            grade.CityId = City.ID;
            if (grade.ID > 0)
            {
                if (!Core.GradeHistoryManager.Edit(grade))
                {
                    return ErrorJsonResult("未找到编辑信息");
                }
            }
            else
            {
                Core.GradeManager.Grade2(City.ID);
                var id = Core.GradeHistoryManager.Add(grade);
                if (id > 0)
                {
                    Core.ScoresHistoryManager.Scores(id, City.ID);
                }

            }

            return SuccessJsonResult();
        }

        public ActionResult Detail(int ELID,SystemData systemData)
        {
            var list = Core.ScoresHistoryManager.GetList(ELID, systemData);
            ViewBag.List = list;

            return View();
        }

        public ActionResult DetailGrade(int id)
        {
            var model = Core.GradeHistoryManager.Get(id);
            ViewBag.Model = model;

            return View();
        }

        public ActionResult Delete(int id)
        {
            if (!Core.GradeHistoryManager.Delete(id))
            {
                return ErrorJsonResult("未找到相关信息");
            }
            return SuccessJsonResult();
        }
        

        public ActionResult Statistic()
        {
            var enterprises= Core.EnterpriseManager.GetScores(City.ID);
            var lawyers= Core.LawyerManager.GetScores(City.ID);
            ViewBag.Enterprises = enterprises;
            ViewBag.Lawyers = lawyers;
            return View();
        }


        public ActionResult StatisticEnterprise()
        {
            var list = Core.EnterpriseManager.GetScores(City.ID);
            ViewBag.List = list;
            return View();
        }

        

        public ActionResult StatisticLawyer()
        {
            var list = Core.LawyerManager.GetScores(City.ID);
            ViewBag.List = list;
            return View();
        }

        public ActionResult StatisticYear(int id)
        {
            var history = Core.GradeHistoryManager.Get(id);
            ViewBag.Model = history;
            return View();
        }



        public ActionResult StatisticDegree(List<GradeHistory> list)
        {
            if (list != null)
            {
                var dict = new Dictionary<GradeDegree, Dictionary<string, int>>();
                foreach(GradeDegree degree in Enum.GetValues(typeof(GradeDegree)))
                {
                    dict.Add(degree, list.ToDictionary(e => e.Name, e => e.ScoresHistorys.Where(j => j.Degree == degree).Count()));
                }
                ViewBag.Dict = dict;
            }
            return View();
        }


        
        
    }
}