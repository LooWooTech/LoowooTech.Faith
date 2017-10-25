using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
using LoowooTech.Faith.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    /// <summary>
    /// 法人管理
    /// </summary>
    public class LawyerManager:ManagerBase
    {
        /// <summary>
        /// 作用：保存法人
        /// 作者：汪建龙
        /// 编写时间：2017年3月5日14:27:21
        /// </summary>
        /// <param name="lawyer"></param>
        /// <returns></returns>
        public int Save(Lawyer lawyer)
        {
            Db.Lawyers.Add(lawyer);
            Db.SaveChanges();
            return lawyer.ID;
        }

        /// <summary>
        /// 作用：编辑更新法人
        /// 作者：汪建龙
        /// 编写时间：2017年3月5日14:28:34
        /// </summary>
        /// <param name="lawyer"></param>
        /// <returns></returns>
        public bool Edit(Lawyer lawyer)
        {
            var model = Db.Lawyers.Find(lawyer.ID);
            if (model == null)
            {
                return false;
            }
            Db.Entry(model).CurrentValues.SetValues(lawyer);
            Db.SaveChanges();
            return true;
        }
        /// <summary>
        /// 作用：获取法人
        /// 作者：汪建龙
        /// 编写时间：2017年3月5日14:29:30
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Lawyer Get(int id)
        {
            if (id == 0)
            {
                return null;
            }
            return Db.Lawyers.Find(id);
        }
        /// <summary>
        /// 作用：通过姓名查询
        /// 作者：汪建龙
        /// 编写时间：2017年3月16日20:15:02
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Lawyer　Get(string name,int cityID)
        {
            var model = Db.Lawyers.FirstOrDefault(e => e.CityID==cityID && e.Name.ToLower() == name.ToLower());
            return model;
        }
        /// <summary>
        /// 作用：删除法人
        /// 作者：汪建龙
        /// 编写时间：2017年3月5日14:30:49
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id,string remark)
        {
            var model = Db.Lawyers.Find(id);
            if (model == null)
            {
                return false;
            }
            model.Deleted = true;
            model.Remark = remark;
            Db.SaveChanges();
            return true;
        }

        public bool Recycle(int id)
        {
            var model = Db.Lawyers.Find(id);
            if (model == null)
            {
                return false;
            }
            model.Deleted = false;
            Db.SaveChanges();
            return true;
        }
        public bool Used(int id)
        {
            return Db.Conducts.Any(e => e.SystemData == SystemData.Lawyer && e.DataId == id) || Db.Lands.Any(e => e.SystemData == SystemData.Lawyer && e.ELID == id);
        }

        /// <summary>
        /// 作用：查询自然人信息
        /// 作者：汪建龙
        /// 编写时间：2017年3月5日15:40:38
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List<Lawyer> Search(LawyerParameter parameter)
        {
            var query = Db.Lawyers.Where(e=>e.Deleted==parameter.Deleted).AsQueryable();
            if (parameter.CityID.HasValue)
            {
                query = query.Where(e => e.CityID == parameter.CityID.Value);
            }
            if (parameter.Degree.HasValue)
            {
                query = query.Where(e => e.Degree == parameter.Degree.Value);
            }
            if (!string.IsNullOrEmpty(parameter.Name))
            {
                query = query.Where(e => e.Name.ToLower().Contains(parameter.Name.ToLower()));
            }
            if (parameter.Sex.HasValue)
            {
                query = query.Where(e => e.Sex == parameter.Sex.Value);
            }
            if (!string.IsNullOrEmpty(parameter.BornTime))
            {
                query = query.Where(e => e.BornTime.ToLower().Contains(parameter.BornTime.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.Number))
            {
                query = query.Where(e => e.Number.ToLower().Contains(parameter.Number.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.Address))
            {
                query = query.Where(e => e.Address.ToLower().Contains(parameter.Address.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.TelPhone))
            {
                query = query.Where(e => e.TelPhone.ToLower().Contains(parameter.TelPhone.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.Email))
            {
                query = query.Where(e => e.EMail.ToLower().Contains(parameter.Email.ToLower()));
            }
            query = query.OrderBy(e => e.ID).SetPage(parameter.Page);
            return query.ToList();
        }
        /// <summary>
        /// 作用：验证系统中是否存在
        /// 作者：汪建龙
        /// 编写时间：2017年3月5日17:43:29
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="sex">性别</param>
        /// <param name="number">证件编号</param>
        /// <returns></returns>
        public bool Exist(string name,Sex sex,string number,int cityId)
        {
            return Db.Lawyers.Any(e => e.Name.ToLower() == name.ToLower() && e.Sex == sex && e.Number.ToLower() == number.ToLower()&&e.CityID==cityId);
        }
        /// <summary>
        /// 作用：批量保存自然人列表
        /// 作者：汪建龙
        /// 编写时间：2017年3月5日19:08:00
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userId"></param>
        public void AddRange(List<Lawyer> list,int userId)
        {
            var name = "文件导入自然人";
            var dailys = new List<Daily>();
            foreach(var item in list)
            {
                Daily daily = null;
                if (Exist(item.Name, item.Sex, item.Number,item.CityID))
                {
                    daily = new Daily
                    {
                        Name = name,
                        Description = string.Format("系统中已存在姓名：{0}；性别：{1} ；证件编号：{2}", item.Name, item.Sex.GetDescription(), item.Number),
                        UserID = userId,
                        CityID=item.CityID
                    };
                }
                else
                {
                    try
                    {
                        var id = Save(item);

                    }
                    catch (Exception ex)
                    {
                        daily = new Daily
                        {
                            Name = name,
                            Description = string.Format("导入姓名：{0}； 性别：{1}；证件编号：{2}；发生错误：{3}", item.Name, item.Sex.GetDescription(), item.Number, ex.Message),
                            UserID = userId,
                            CityID=item.CityID
                        };
                    }
                }
             
                if (daily != null && !string.IsNullOrEmpty(daily.Name))
                {
                    dailys.Add(daily);
                }
            }

            if (dailys.Count > 0)
            {
                Core.DailyManager.AddRange(dailys);
            }
   

        }
        /// <summary>
        /// 作用：获取自然人的数据个数
        /// 作者：汪建龙
        /// 编写时间：2017年3月16日16:52:54
        /// </summary>
        /// <returns></returns>
        public long Count(int cityID)
        {
            return Db.Lawyers.Where(e=>e.Deleted==false&&e.CityID==cityID).LongCount();
        }

        public long CountEnterprise(int cityID)
        {
            return Db.Lawyers.Where(e => e.Deleted == false && e.CityID == cityID && e.EnterpriseID.HasValue).LongCount();
        }
        public void Grade(int cityID)
        {
            var feeds = new List<Feed>();
            var scores = Db.LawyerScores.Where(e=>e.CityID==cityID).ToList();
            foreach(var score in scores)
            {
                var lawyer = Db.Lawyers.Find(score.ID);
                if (lawyer == null)
                {
                    continue;
                }
                if (lawyer.Degree != score.Degree)
                {
                    feeds.Add(new Feed
                    {
                        ELID = lawyer.ID,
                        SystemData = SystemData.Lawyer,
                        Old = lawyer.Degree,
                        New = score.Degree
                    });
                }
                lawyer.Degree = score.Degree;
                lawyer.Times = score.Times;
                lawyer.Values = score.ScoreValue;
                lawyer.Record = score.Record;
                lawyer.GradeTime = DateTime.Now;
            }

            if (feeds.Count > 0)
            {
                Db.Feeds.AddRange(feeds);
            }
            Db.SaveChanges();
        }
        /// <summary>
        /// 作用：对一个自然人进行信用评级
        /// 作者：王健林
        /// 编写时间：2017年4月6日10:58:27；
        /// </summary>
        /// <param name="id"></param>
        /// <param name="conductId"></param>
        /// <param name="action"></param>
        public void Grade(int id,int conductId,GradeAction action)
        {
            var lawyer = Db.Lawyers.Find(id);
            if (lawyer == null)
            {
                return;
            }
            var score = Db.LawyerScores.Find(id);
            if (score == null)
            {
                return;
            }

            Feed feed = null;
            if (lawyer.Degree != score.Degree)
            {
                feed = new Feed
                {
                    ELID = lawyer.ID,
                    SystemData = SystemData.Lawyer,
                    Old = lawyer.Degree,
                    New = score.Degree,
                    ConductID = conductId,
                    Action=action
                };
            }
            lawyer.Degree = score.Degree;
            lawyer.Times = score.Times;
            lawyer.Values = score.ScoreValue;
            lawyer.Record = score.Record;
            lawyer.GradeTime = DateTime.Now;
            if (feed != null)
            {
                Db.Feeds.Add(feed);

            }
            Db.SaveChanges();
        }


        public List<LawyerScore> GetScores(int CityId)
        {
            return Db.LawyerScores.Where(e => e.CityID == CityId && e.Deleted == false).ToList();
        }
    }
}