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
    public class FlowNodeController : ControllerBase
    {
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult Work()
        {
            var parameter = new FlowNodeConductParameter
            {
                FlowNodeState = DoingState.None
            };
            if (Identity.Role == UserRole.Manager)
            {
                parameter.UserID = Identity.UserID;
            }
            var list = Core.FlowNodeConductManager.Search(parameter);
            ViewBag.List = list;
            return View();
        }

        public ActionResult History(int page=1,int rows=20)
        {
            var pg = new PageParameter(page, rows);
            var histroy = Core.FlowNodeConductManager.SearchHistory(pg,City.ID);
            ViewBag.History = histroy;
            ViewBag.Page = pg;
            return View();
        }
        public ActionResult Create(int infoId)
        {
            var managers = Core.UserManager.GetManager(City.ID);
            ViewBag.Managers = managers;
            var conduct = Core.ConductManager.Get(infoId);
            ViewBag.Conduct = conduct;
            return View();
        }
        [HttpPost]
        public ActionResult Create(FlowNode flowNode)
        {
            if (flowNode == null)
            {
                return ErrorJsonResult("未获取提交申请信息");
            }
            var user = Core.UserManager.Get(flowNode.UserID);
            if (user == null || user.Role != UserRole.Manager)
            {
                return ErrorJsonResult("未找到审批人员信息或者该人员没有审批权限");
            }
            var conduct = Core.ConductManager.Get(flowNode.InfoID);
            if (conduct == null)
            {
                return ErrorJsonResult("未找到诚信行为记录信息,请核对");
            }
            if (flowNode.ID > 0)
            {
                if (!Core.FlowNodeManager.Edit(flowNode))
                {
                    return ErrorJsonResult("更新失败");
                }
            }
            else
            {
                var id = Core.FlowNodeManager.Save(flowNode);
                if (id <= 0)
                {
                    return ErrorJsonResult("提交信息失败");
                }

            }
            Core.ConductManager.ChangeState(flowNode.InfoID, BaseState.Checking);
            return SuccessJsonResult();
        }
        /// <summary>
        /// 作用：审核不通过 填写理由界面
        /// 作者：汪建龙
        /// 编写时间：2017年3月18日18:37:28
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Refuse(int id)
        {
            var model = Core.FlowNodeConductManager.Get(id);
            ViewBag.Model = model;
            return View();
        }

        /// <summary>
        /// 作用：审核
        /// </summary>
        /// <param name="id"></param>
        /// <param name="IsOk"></param>
        /// <returns></returns>
        public ActionResult Verify(int id,bool IsOk,string reason)
        {
            var flowNode = Core.FlowNodeManager.Get(id);
            if (flowNode == null)
            {
                return ErrorJsonResult("审核失败，未找到需要审核的诚信记录信息");
            }
            if (flowNode.Conduct == null)
            {
                flowNode.Conduct = Core.ConductManager.Get(flowNode.InfoID);
            }
            if (flowNode.Conduct == null)
            {
                return ErrorJsonResult("未查询到需要审核的诚信行为记录");
            }

            flowNode.Conduct.State = IsOk ? BaseState.Argee : BaseState.DisAgree;
            flowNode.Conduct.Content = IsOk ? string.Empty : reason;
            flowNode.State = IsOk ? DoingState.Done : DoingState.Roll;
            flowNode.UpdateTime = DateTime.Now;
            if(!Core.FlowNodeManager.Edit(flowNode))
            {
                return ErrorJsonResult("更新审核表失败");
            }
          
            if (IsOk)//审核后通过 
            {
                var land = Core.LandManager.Get(flowNode.Conduct.LandID);
                if (land != null)
                {
                    Core.GradeManager.Grade(land, flowNode.Conduct.ID,GradeAction.Conduct);//对应的企业自然人如果发生信用评级发生改变，更新级别并且发布级别提醒
                    //if (flowNode.Conduct.Degree != CreditDegree.Good)// 并且不是诚信行为
                    //{
                    //    var error = Core.RollManager.Update(land.ELID, land.SystemData, flowNode.Conduct.Degree);
                    //    if (!string.IsNullOrEmpty(error))
                    //    {
                    //        Core.DailyManager.Save(new Daily
                    //        {
                    //            Name = "更新异常/黑名单",
                    //            Description = string.Format("FlowNodeID:{0};ConductID:{1},错误信息：", flowNode.ID, flowNode.Conduct.ID),
                    //            UserID = Identity.UserID
                    //        });
                    //    }
                    //}
                  
                }
                
              
            }
            return SuccessJsonResult();
        }

        public ActionResult List(DoingState? fState = null,BaseState? state=null)
        {
            var parameter = new FlowNodeViewParameter
            {
                fState = fState,
                State=state
            };
            var list = Core.FlowNodeViewManager.Search(parameter);
            ViewBag.List = list;
            return View();
        }

   
    }
}