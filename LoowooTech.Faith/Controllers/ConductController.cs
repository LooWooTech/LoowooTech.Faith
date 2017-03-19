﻿using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
using LoowooTech.Faith.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoowooTech.Faith.Controllers
{
    public class ConductController : ControllerBase
    {
        [UserAuthorize]
        public ActionResult Index()
        {
            //var recent = Core.ConductManager.Search(new ConductParameter { Page = new PageParameter(1, 10) });
            //ViewBag.Recent = recent;
            return View();
        }

        /// <summary>
        /// 作用：
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日19:11:31
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(int id=0, int dataId=0,SystemData? systemData=null)
        {
            var conduct = Core.ConductManager.Get(id);
            if (conduct != null)
            {
                ViewBag.Standards = Core.StandardManager.GetList(conduct.Credit);
            }
            ViewBag.Conduct = conduct;
            if (systemData.HasValue)
            {
                if (SystemData.Enterprise == systemData.Value)
                {
                    var model = Core.EnterpriseManager.Get(dataId);
                    ViewBag.Model = model;
                }
                else
                {
                    var model = Core.LawyerManager.Get(dataId);
                    ViewBag.Model = model;
                }
            }
            ViewBag.SystemData = systemData;
          
            return View();
        }
        [HttpPost]
        public ActionResult Create(Conduct conduct)
        {
            #region  验证数据
            if (conduct == null)
            {
                return ErrorJsonResult("未获取诚信记录信息");
            }
            var standard = Core.StandardManager.Get(conduct.StandardId);
            if (standard == null||standard.Credit!=conduct.Credit)
            {
                return ErrorJsonResult(string.Format("未找到ID为{0}的诚信行为，或者当前诚信行为不属于环节{1}", conduct.StandardId, conduct.Credit.GetDescription()));
            }
            if (conduct.SystemData == SystemData.Enterprise)
            {
                var enterprise = Core.EnterpriseManager.Get(conduct.DataId);
                if (enterprise == null)
                {
                    return ErrorJsonResult(string.Format("未找到ID为{0}的企业信息", conduct.DataId));
                }
            }
            else{
                var lawyer = Core.LawyerManager.Get(conduct.DataId);
                if (lawyer == null)
                {
                    return ErrorJsonResult(string.Format("未找到ID为{0}的自然人信息", conduct.DataId));
                }
            }
            #endregion
            conduct.UserID = Identity.UserID;

            if (conduct.ID > 0)
            {
                if (!Core.ConductManager.Edit(conduct))
                {
                    return ErrorJsonResult("编辑更新失败，未找到编辑更新的诚信记录");
                }
            }
            else
            {
                var id = Core.ConductManager.Save(conduct);
                if (id <= 0)
                {
                    return ErrorJsonResult("保存失败");
                }
                if (!Core.ConductManager.UpdateCredit(conduct.DataId,conduct.Degree,conduct.SystemData,true))
                {
                    Core.DailyManager.Save(new Daily
                    {
                        Name = "更新企业自然人诚信次数",
                        Description = string.Format("更新的类型：{0}；ID：{1}", conduct.SystemData.GetDescription(), conduct.DataId),
                        UserID = Identity.UserID
                    });
                }
            }

            return SuccessJsonResult();
        }


        public ActionResult Save(int id=0,int LandID = 0)
        {
            var model = Core.ConductManager.Get(id);
            if (model != null)
            {
                ViewBag.Standards = Core.StandardManager.GetList(model.Credit);
            }
            ViewBag.Model = model;
            var land = Core.LandManager.GetView(LandID);
            if (land == null&&model!=null)
            {
                land = Core.LandManager.GetView(model.LandID);
            }
            ViewBag.Land = land;
            return View();
        }

        [HttpPost]
        public ActionResult Save(Conduct conduct)
        {
            #region   验证数据
            if (conduct == null)
            {
                return ErrorJsonResult("未获取诚信记录信息");
            }
            var standard = Core.StandardManager.Get(conduct.StandardId);
            if (standard == null || standard.Credit != conduct.Credit)
            {
                return ErrorJsonResult(string.Format("未找到ID为{0}的诚信行为，或者当前诚信行为不属于环节{1}", conduct.StandardId, conduct.Credit.GetDescription()));
            }
            var land = Core.LandManager.Get(conduct.LandID);
            if (land == null)
            {
                return ErrorJsonResult(string.Format("未找到ID为{0}的供地信息", conduct.LandID));
            }
            #endregion

            if (conduct.ID > 0)
            {
                if (!Core.ConductManager.Edit(conduct))
                {
                    return ErrorJsonResult("编辑更新失败,请刷新再次尝试");
                }
            }
            else
            {
                var id = Core.ConductManager.Save(conduct);
                if (id <= 0)
                {
                    return ErrorJsonResult("保存诚信记录失败，请刷新再次尝试");
                }
            }
            return SuccessJsonResult(conduct.LandID);
        }
        

        /// <summary>
        /// 作用：获取某一个环节的诚信类型
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日19:11:09
        /// </summary>
        /// <param name="credit"></param>
        /// <returns></returns>
        public ActionResult GetStandard(Credit credit)
        {
            var list = Core.StandardManager.GetList(credit);
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 作用：删除撤销诚信行为记录
        /// 作者：汪建龙
        /// 编写时间：2017年3月7日09:17:56
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            if (!Core.ConductManager.Delete(id))
            {
                return ErrorJsonResult("当前诚信行为已提交或未找到删除的诚信行为记录信息，请核对");
            }
            return SuccessJsonResult();
        }

        public ActionResult Search(SystemData systemData,string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            List<JsonData> list = null;
            if (systemData == SystemData.Enterprise)
            {
                var parameter = new EnterpriseParameter
                {
                    Name = key,
                    Page = new PageParameter(1, 5)
                };
                list = Core.EnterpriseManager.Search(parameter).Select(e => new JsonData { ID=e.ID,Name=e.Name}).ToList(); 
            }
            else
            {
                var parameter = new LawyerParameter
                {
                    Name = key,
                    Page = new PageParameter(1, 5)
                };
                list = Core.LawyerManager.Search(parameter).Select(e => new JsonData { ID=e.ID,Name=e.Name}).ToList();
            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Submit(int id)
        {
            var manager = Core.UserManager.GetManager();
            ViewBag.Managers = manager;
            return View();
        }

        [UserAuthorize(false)]
        public ActionResult Detail(int dataId,SystemData systemData,int? rows=null)
        {
            ViewBag.DataID = dataId;
            ViewBag.SystemData = systemData;
            var conductViews = Core.ConductManager.GetView(dataId, systemData, rows.HasValue ? new PageParameter(1, rows.Value) : null);
            ViewBag.Views = conductViews;
            return View();
        }

        public ActionResult DetailConduct(int id)
        {
            var model = Core.ConductManager.GetView(id);
            ViewBag.Model = model;
            return View();
        }

        /// <summary>
        /// 作用
        /// 作者：汪建龙
        /// 编写时间：2017年3月8日15:12:47
        /// </summary>
        /// <returns></returns>
        public ActionResult List(
            int? UserId=null,
            DateTime? startTime=null,DateTime? EndTime=null,
            CreditDegree? degree=null,string systemData=null,
            string name=null,Credit ?credit=null,
            string sName=null,double? minScore=null,double? maxScore=null,
            BaseState? state=null,string userName=null,
            DateTime? startUpdateTime=null,DateTime? endUpdateTime=null,
            int page=1,int rows=20
            )
        {

            var parameter = new FlowNodeViewParameter
            {
                UserID = UserId,
                StartTime = startTime,
                EndTime = EndTime,
                Degree=degree,
                Credit=credit,
                State=state,
                Name = name,
                sName = sName,
                MinScore = minScore,
                MaxScore = maxScore,
                UserName = userName,
                StartUpdateTime = startUpdateTime,
                EndUpdateTime = endUpdateTime,
                Page = new PageParameter(page, rows)
            };
            if (!string.IsNullOrEmpty(systemData))
            {
                parameter.SystemData = EnumHelper.GetEnum<SystemData>(systemData);
            }
            var List = Core.FlowNodeViewManager.Search(parameter);
            ViewBag.List = List;
            ViewBag.Parameter = parameter;
            ViewBag.Page = parameter.Page;
            return View();
        }

        /// <summary>
        /// 作用：诚信行为记录解除
        /// 作者：汪建龙
        /// 编写时间：2017年3月9日15:50:25
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Relieve(int id)
        {
            if (!Core.ConductManager.Relieve(id))
            {
                return ErrorJsonResult("解除诚信行为失败，未找到诚信行为记录，或已经被撤销");
            }
            return SuccessJsonResult();
        }

        public ActionResult DetailList(int landID)
        {
            var list = Core.ConductStandardManager.GetByLandID(landID);
            ViewBag.List = list;
            ViewBag.LandID = landID;
            return View();
        }
    }
}