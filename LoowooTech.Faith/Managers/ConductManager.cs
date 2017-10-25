using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
using LoowooTech.Faith.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class ConductManager:ManagerBase
    {
        /// <summary>
        /// 作用：保存诚信记录
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日20:34:37
        /// </summary>
        /// <param name="conduct"></param>
        /// <returns></returns>
        public int Save(Conduct conduct)
        {
            Db.Conducts.Add(conduct);
            Db.SaveChanges();
            return conduct.ID;
        }
        /// <summary>
        /// 作用：编辑
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日20:38:05
        /// </summary>
        /// <param name="conduct"></param>
        /// <returns></returns>
        public bool Edit(Conduct conduct)
        {
            var model = Db.Conducts.Find(conduct.ID);
            if (model == null)
            {
                return false;
            }
            if (model.Degree != conduct.Degree)
            {
                UpdateCredit(conduct.DataId, model.Degree, conduct.SystemData, false);
                UpdateCredit(conduct.DataId, conduct.Degree, conduct.SystemData, true);
            }
            conduct.LandID = model.LandID;
            conduct.State = model.State;
            Db.Entry(model).CurrentValues.SetValues(conduct);
            Db.SaveChanges();
            
            return true;
        }

        /// <summary>
        /// 作用：删除撤销记录
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日20:35:48
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var model = Db.Conducts.Find(id);
            if (model == null)
            {
                return false;
            }
            try
            {
                Db.Conducts.Remove(model);
                Db.SaveChanges();
            }
            catch
            {
                return false;
            }
            Grade(model, GradeAction.DeleteConduct);
           // UpdateCredit(model.DataId, model.Degree, model.SystemData, false);
            return true;
        }
        public void Grade(Conduct conduct,GradeAction action)
        {
            var land = Core.LandManager.Get(conduct.LandID);
            if (land != null)
            {
                if (land.SystemData == SystemData.Enterprise)
                {
                    Core.EnterpriseManager.Grade(land.ELID, conduct.ID, action);
                }
                else if (land.SystemData == SystemData.Lawyer)
                {
                    Core.LawyerManager.Grade(land.ELID, conduct.ID, action);
                }
            }
           
        }

        /// <summary>
        /// 作用：获取企业或者自然人的诚信列表
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日21:14:39
        /// </summary>
        /// <param name="dataid"></param>
        /// <param name="systemData"></param>
        /// <returns></returns>
        public List<Conduct> Get(int dataid,SystemData systemData)
        {
            var list= Db.Conducts.Where(e => e.DataId == dataid && e.SystemData == systemData).OrderByDescending(e => e.CreateTime).ToList();
            foreach(var item in list)
            {
                Db.Entry(item).Reference(e => e.Standard).Load();
            }
            return list;
        }
        /// <summary>
        /// 作用：获取企业获取自然人的诚信记录信息
        /// 作者：汪建龙
        /// 编写时间：2017年3月11日14:01:27
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="systemdata"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<ConductView> GetView(int dataId,SystemData systemdata,PageParameter page=null)
        {
            var query = Db.ConductView.Where(e => e.DataId == dataId && e.SystemData == systemdata).OrderByDescending(e => e.CreateTime).AsQueryable();
            if (page != null)
            {
                query = query.SetPage(page);
            }
            return query.ToList();
        }

       
        /// <summary>
        /// 作用：获取诚信记录
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日21:47:11
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Conduct Get(int id)
        {
            if (id == 0)
            {
                return null;
            }
            return Db.Conducts.Find(id);
        }
        private T AddCredit<T>(T entey,CreditDegree degree) where T:ScoreBase
        {
            if (entey == null)
            {
                return null;
            }
            switch (degree)
            {
                case CreditDegree.Bad:
                    entey.Bad++;
                    break;
                case CreditDegree.Good:
                    entey.Good++;
                    break;
                case CreditDegree.Less:
                    entey.Less++;
                    break;
            }
            return entey;
        }
        private T ReduceCredit<T>(T entey, CreditDegree degree) where T : ScoreBase
        {
            if (entey == null)
            {
                return null;
            }
            switch (degree)
            {
                case CreditDegree.Bad:
                    entey.Bad--;
                    break;
                case CreditDegree.Good:
                    entey.Good--;
                    break;
                case CreditDegree.Less:
                    entey.Less--;
                    break;
            }
            return entey;
        }
        private bool UpdateCreditEnterprise(int id,CreditDegree degree,bool IsAdd)
        {
            var enterprise = Core.EnterpriseManager.Get(id);
            if (enterprise == null)
            {
                return false;
            }
            if (IsAdd)
            {
                enterprise = AddCredit<Enterprise>(enterprise, degree);
            }
            else
            {
                enterprise = ReduceCredit<Enterprise>(enterprise, degree);
            }
          
            return Core.EnterpriseManager.Edit(enterprise);
        }
        private bool UpdateCreditLawyer(int id,CreditDegree degree,bool IsAdd)
        {
            var lawyer = Core.LawyerManager.Get(id);
            if (lawyer == null)
            {
                return false;
            }
            if (IsAdd)
            {
                lawyer = AddCredit<Lawyer>(lawyer, degree);
            }
            else
            {
                lawyer = ReduceCredit<Lawyer>(lawyer, degree);
            }
         
            return Core.LawyerManager.Edit(lawyer);
        }
        /// <summary>
        /// 作用：更新诚信次数
        /// 作者：汪建龙
        /// 编写时间：2017年3月7日10:25:58
        /// </summary>
        /// <param name="dataId">对应的Id</param>
        /// <param name="degree">诚信、失信、严重失信</param>
        /// <param name="systemData">企业还是自然人</param>
        /// <param name="IsAdd">是否增加</param>
        /// <returns></returns>
        public bool UpdateCredit(int dataId,CreditDegree degree,SystemData systemData,bool IsAdd)
        {
            if (systemData == SystemData.Enterprise)
            {
                return UpdateCreditEnterprise(dataId, degree,IsAdd);
            }

            return UpdateCreditLawyer(dataId, degree,IsAdd);
        }

        /// <summary>
        /// 作用：查询
        /// 作者：汪建龙
        /// 编写时间：2017年3月7日11:55:38
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List<Conduct> Search(ConductParameter parameter)
        {
            var query = Db.Conducts.AsQueryable();
            if (parameter.UserID.HasValue)
            {
                query = query.Where(e => e.UserID == parameter.UserID.Value);
            }
            if (parameter.Credit.HasValue)
            {
                query = query.Where(e => e.Credit == parameter.Credit.Value);
            }
            if (parameter.Degree.HasValue)
            {
                query = query.Where(e => e.Degree == parameter.Degree.Value);
            }
            if (parameter.StartTime.HasValue)
            {
                query = query.Where(e => e.CreateTime >= parameter.StartTime.Value);
            }
            if (parameter.EndTime.HasValue)
            {
                query = query.Where(e => e.CreateTime <= parameter.EndTime.Value);
            }
            if (parameter.MaxScore.HasValue)
            {
                query = query.Where(e => e.Score <= parameter.MaxScore.Value);
            }
            if (parameter.MinScore.HasValue)
            {
                query = query.Where(e => e.Score >= parameter.MinScore.Value);
            }
            if (parameter.State.HasValue)
            {
                query = query.Where(e => e.State == parameter.State.Value);
            }
            query = query.OrderByDescending(e => e.CreateTime).SetPage(parameter.Page);
            foreach(var item in query)
            {
                Db.Entry(item).Reference(e => e.Standard).Load();
            }
            return query.ToList();
        }
        /// <summary>
        /// 作用：更新诚信行为记录状态
        /// 作者：汪建龙
        /// 编写时间：2017年3月7日16:04:48
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool ChangeState(int id,BaseState state)
        {
            var model = Db.Conducts.Find(id);
            if (model == null)
            {
                return false;
            }
            model.State = state;
            Db.SaveChanges();
            return true;
        }
        public ScoreBase GetScoreBase(int dataId,SystemData systemData)
        {
            if (systemData == SystemData.Enterprise)
            {
                return Db.Enterprises.Find(dataId);
            }
            else
            {
                return Db.Lawyers.Find(dataId);
            }
        }
        public List<ConductView> Search(ConductViewParameter parameter) 
        {
            var query = Db.ConductView.AsQueryable();
            if (parameter.UserID.HasValue)
            {
                query = query.Where(e => e.UserID == parameter.UserID.Value);
            }
            if (parameter.Credit.HasValue)
            {
                query = query.Where(e => e.Credit == parameter.Credit.Value);
            }
            if (parameter.Degree.HasValue)
            {
                query = query.Where(e => e.CreditDegree == parameter.Degree.Value);
            }
            if (parameter.StartTime.HasValue)
            {
                query = query.Where(e => e.CreateTime >= parameter.StartTime.Value);
            }
            if (parameter.EndTime.HasValue)
            {
                query = query.Where(e => e.CreateTime <= parameter.EndTime.Value);
            }
            if (parameter.MaxScore.HasValue)
            {
                query = query.Where(e => e.Score <= parameter.MaxScore.Value);
            }
            if (parameter.MinScore.HasValue)
            {
                query = query.Where(e => e.Score >= parameter.MinScore.Value);
            }
            if (!string.IsNullOrEmpty(parameter.Name))
            {
                query = query.Where(e => e.Name.ToLower().Contains(parameter.Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.sName))
            {
                query = query.Where(e => e.sName.ToLower().Contains(parameter.sName.ToLower()));
            }
            if (parameter.State.HasValue)
            {
                query = query.Where(e => e.State == parameter.State.Value);
            }
            query = query.OrderByDescending(e => e.CreateTime).SetPage(parameter.Page);
            return query.ToList();
          
        }

        /// <summary>
        /// 作用：诚信行为解除
        /// 作者：汪建龙
        /// 编写时间：2017年3月9日15:53:08
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Relieve(int id,string memo)
        {
            var model = Db.Conducts.Find(id);
            if (model == null)
            {
                return false;
            }
            model.Memo = memo;
            model.State = BaseState.Relieve;
            Db.SaveChanges();
            Grade(model, GradeAction.Relieve);
            return true;
        }

        /// <summary>
        /// 作用:取消解除
        /// 作者：汪建龙
        /// 编写时间：2017年3月29日16:30:22
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CancelRelieve(int id)
        {
            var model = Db.Conducts.Find(id);
            if (model == null)
            {
                return false;
            }
            model.State = BaseState.Argee;
            Db.SaveChanges();
            Grade(model, GradeAction.CanRelieve);
            return true;
        }
        /// <summary>
        /// 作用：获取诚信行为记录
        /// 作者：汪建龙
        /// 编写时间：2017年3月10日14:56:56
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ConductView GetView(int id)
        {
            var model = Db.ConductView.Find(id);
            return model;
        }

        ///// <summary>
        ///// 作用：统计数量
        ///// 作者：汪建龙
        ///// 编写时间：2017年3月20日16:43:46
        ///// </summary>
        ///// <returns></returns>
        //public long Count()
        //{
        //    return Db.Conducts.LongCount();
        //}

    }
}