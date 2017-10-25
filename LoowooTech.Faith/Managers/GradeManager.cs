using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class GradeManager:ManagerBase
    {
        //public List<Daily> GradeEnterprise(DateTime BeginTime,int userId,List<Grade> grades)
        //{
        //    var dailys = new List<Daily>();
        //    var list = Core.EnterpriseManager.Search(new Parameters.EnterpriseParameter());
        //    foreach(var item in list)
        //    {
        //        var grade = Analyze(grades, BeginTime,SystemData.Enterprise,item.ID);
        //        if (grade == null)
        //        {
        //            dailys.Add(new Daily { Name = "信用评级", Description = string.Format("企业：{0}没有得到评级结果", item.Name), UserID = userId });
        //            continue;
        //        }
        //        item.Level = grade.Name;
        //        item.LevelDescription = grade.Content;
        //        item.GradeTime = DateTime.Now;
        //        if (!Core.EnterpriseManager.Edit(item))
        //        {
        //            dailys.Add(new Daily { Name = "信用评级", Description = string.Format("企业：{0}得到满足{1}的{2}，但是更新失败", item.Name, grade.Content, grade.Name),UserID=userId });
        //        }
        //    }
        //    return dailys;
        //}
        //public List<Daily> GradeLawyer(DateTime BeginTime,int userId,List<Grade> grades)
        //{
        //    var dailys = new List<Daily>();
        //    var list = Core.LawyerManager.Search(new Parameters.LawyerParameter());
        //    foreach(var item in list)
        //    {
        //        var grade = Analyze(grades, BeginTime, SystemData.Lawyer, item.ID);
        //        if (grade == null)
        //        {
        //            dailys.Add(new Daily { Name = "信用评级", Description = string.Format("自然人：{0}没有得到评级结果", item.Name), UserID = userId });
        //            continue;
        //        }
        //        item.Level = grade.Name;
        //        item.LevelDescription = grade.Content;
        //        item.GradeTime = DateTime.Now;
        //        if (!Core.LawyerManager.Edit(item))
        //        {
        //            dailys.Add(new Daily { Name = "信用评级", Description = string.Format("自然人：{0}得到满足{1}的{2}，但是更新失败", item.Name, grade.Content, grade.Name), UserID = userId });
        //        }
        //    }
        //    return dailys;
        //}
       
        /// <summary>
        /// 作用：分析企业/自然人在某段时间之后  符合某一种级别
        /// 作者：汪建龙
        /// 编写时间：2017年3月8日20:37:24
        /// </summary>
        /// <param name="enterprise"></param>
        /// <param name="Grades"></param>
        /// <param name="BeginTime"></param>
        /// <returns></returns>
        //public Grade Analyze(List<Grade> Grades,DateTime BeginTime,SystemData systemData,int dataId)
        //{
        //    var conducts = Db.Conducts.Where(e => e.SystemData == systemData && e.DataId == dataId && e.CreateTime >= BeginTime&&e.State==BaseState.Argee).ToList();
        //    return Analyze(conducts, Grades, BeginTime);
        //}

        //private Grade Analyze(List<Conduct> conducts,List<Grade> grades,DateTime BeginTime)
        //{
        //    foreach (var grade in grades)
        //    {
        //        if (Valiate(conducts, grade))
        //        {
        //            return grade;
        //        }
        //    }
        //    return null;
        //}

        /// <summary>
        /// 作用：验证企业是否属于这一级别
        /// 作者：汪建龙
        /// 编写时间：2017年3月8日20:36:13
        /// </summary>
        /// <param name="conducts"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        //private bool Valiate(List<Conduct> conducts,Grade grade)
        //{
        //    if (grade.Bad.HasValue)
        //    {
        //        if (grade.Bad.Value != 0 && Exist(conducts, CreditDegree.Bad, grade.Bad.Value))
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        if (Exist(conducts, CreditDegree.Bad))
        //        {
        //            return true;
        //        }
        //    }
        //    if (grade.Less.HasValue)
        //    {
        //        if (grade.Less.Value != 0 && Exist(conducts, CreditDegree.Less, grade.Less.Value))
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        if (Exist(conducts, CreditDegree.Less))
        //        {
        //            return true;
        //        }
        //    }

        //    if (grade.Litter.HasValue)
        //    {
        //        if (grade.Litter.Value != 0 && Exist(conducts, CreditDegree.Good, grade.Litter.Value))
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        if (Exist(conducts, CreditDegree.Good))
        //        {
        //            return true;
        //        }
        //    }
        //    var score = 100 - conducts.Where(e => e.Degree == CreditDegree.Good).Sum(e => e.Score);
        //    if (grade.MinScore.HasValue&& score >= grade.MinScore.Value )
        //    {
        //        if (grade.MaxScore.HasValue)
        //        {
        //            if(score <= grade.MaxScore.Value)
        //            {
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            return true;
        //        }
             
        //    }

        //    if (grade.MaxScore.HasValue && score <= grade.MaxScore.Value)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        private bool Exist(List<Conduct> conducts,CreditDegree degree,int? count=null)
        {
            return count.HasValue ? conducts.Where(e => e.Degree == degree).Count() > count.Value : conducts.Any(e => e.Degree == degree);
        }

        public void Grade2(int cityID)
        {
            Core.EnterpriseManager.Grade(cityID);
            Core.LawyerManager.Grade(cityID);
        }

        public void Grade(Land land,int conductId,GradeAction action)
        {
            if (land.SystemData == SystemData.Enterprise)
            {
                Core.EnterpriseManager.Grade(land.ELID, conductId,action);
            }
            else if (land.SystemData == SystemData.Lawyer)
            {
                Core.LawyerManager.Grade(land.ELID, conductId,action);
            }
        }

        public Feed GradeEnterprise(Score score,int id,int conductId)
        {
            Feed feed = null;
            var enterprise = Core.EnterpriseManager.Get(id);
            if (enterprise != null)
            {
                if (enterprise.Degree != score.Degree)
                {
                    feed= new Feed
                    {
                        ELID = id,
                        SystemData = SystemData.Enterprise,
                        Old = enterprise.Degree,
                        New = score.Degree,
                        ConductID = conductId
                    };
                }
                enterprise.Degree = score.Degree;
                enterprise.Times = score.Times;
                enterprise.Values = score.ScoreValue;
                enterprise.Record = score.Record;
                Core.EnterpriseManager.Edit(enterprise);
            }
            return feed;
        }
        public Feed GradeLawyer(Score score,int id,int conductId)
        {
            Feed feed = null;
            var lawyer = Core.LawyerManager.Get(id);
            if (lawyer != null)
            {
                if (lawyer.Degree != score.Degree)
                {
                    feed = new Feed
                    {
                        ELID = id,
                        SystemData = SystemData.Lawyer,
                        Old = lawyer.Degree,
                        New = score.Degree,
                        ConductID = conductId
                    };
                }
                lawyer.Degree = score.Degree;
                lawyer.Times = score.Times;
                lawyer.Values = score.ScoreValue;
                lawyer.Record = score.Record;
                Core.LawyerManager.Edit(lawyer);
            }
          
            return feed;
        }

    }
}