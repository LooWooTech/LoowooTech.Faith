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
    /// 企业管理
    /// </summary>
    public class EnterpriseManager:ManagerBase
    {
        /// <summary>
        /// 作用：保存企业
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日11:21:34
        /// </summary>
        /// <param name="enterprise"></param>
        /// <returns></returns>
        public int Save(Enterprise enterprise)
        {
            Db.Enterprises.Add(enterprise);
            Db.SaveChanges();
            return enterprise.ID;
        }
        /// <summary>
        /// 作用：编辑更新企业信息
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日11:22:39
        /// </summary>
        /// <param name="enterprise"></param>
        /// <returns></returns>
        public bool Edit(Enterprise enterprise)
        {
            var model = Db.Enterprises.Find(enterprise.ID);
            if (model == null)
            {
                return false;
            }
            Db.Entry(model).CurrentValues.SetValues(enterprise);
            Db.SaveChanges();
            return true;
        }
        /// <summary>
        /// 作用：获取企业
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日11:23:18
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Enterprise Get(int id)
        {
            if (id == 0)
            {
                return null;
            }
            return Db.Enterprises.Find(id);
        }
        /// <summary>
        /// 作用：通过名称查询
        /// 作者：汪建龙
        /// 编写时间：2017年3月16日20:13:41
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Enterprise Get(string name,int cityID)
        {
            var model = Db.Enterprises.FirstOrDefault(e => e.CityID==cityID &&e.Name.ToLower() == name.ToLower());
            return model;
        }
        /// <summary>
        /// 作用：删除企业
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日11:26:03
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id,string remark)
        {
            var model = Db.Enterprises.Find(id);
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
            var model = Db.Enterprises.Find(id);
            if (model == null)
            {
                return false;
            }
            model.Deleted = false;
            Db.SaveChanges();
            return true;
        }

        public List<string> GetEnterpriseType()
        {
            return Db.Enterprises.Select(e => e.Type).Distinct().ToList();
        }
        /// <summary>
        /// 作用：查询企业
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日11:28:14
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List<Enterprise> Search(EnterpriseParameter parameter)
        {
            var query = Db.Enterprises.Where(e=>e.Deleted==parameter.Deleted).AsQueryable();
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
            if (!string.IsNullOrEmpty(parameter.OIBC))
            {
                query = query.Where(e => e.OIBC.ToLower().Contains(parameter.OIBC.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.USCC))
            {
                query = query.Where(e => e.USCC.ToLower().Contains(parameter.USCC.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.Address))
            {
                query = query.Where(e => e.Address.ToLower().Contains(parameter.Address.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.Lawyer))
            {
                query = query.Where(e => e.Lawyer.ToLower().Contains(parameter.Lawyer.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.LawNumber))
            {
                query = query.Where(e => e.LawNumber.ToLower().Contains(parameter.LawNumber.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.Number))
            {
                query = query.Where(e => e.Number.ToLower().Contains(parameter.Number.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.Scope))
            {
                query = query.Where(e => e.Scope.ToLower().Contains(parameter.Scope.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.Type))
            {
                if (parameter.Type == "NULL")
                {
                    query = query.Where(e => string.IsNullOrEmpty(e.Type));
                }
                else
                {
                    query = query.Where(e => e.Type.ToLower().Contains(parameter.Type.ToLower()));
                }
                
            }
            if (!string.IsNullOrEmpty(parameter.Contact))
            {
                query = query.Where(e => e.Contact.ToLower().Contains(parameter.Contact.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.TelPhone))
            {
                query = query.Where(e => e.TelPhone.ToLower().Contains(parameter.TelPhone.ToLower()));
            }
            if (parameter.MinMoney.HasValue)
            {
                query = query.Where(e => e.Money >= parameter.MinMoney.Value);
            }
            if (parameter.MaxMoney.HasValue)
            {
                query = query.Where(e => e.Money <= parameter.MaxMoney.Value);
            }
            query = query.OrderBy(e => e.ID).SetPage(parameter.Page);
            return query.ToList();
        }
        /// <summary>
        /// 作用：以名字检索是否存在
        /// 作者：汪建龙
        /// 编写时间：2017年3月6日15:04:53
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exist(string name,int cityID)
        {
            return Db.Enterprises.Any(e => e.Name.ToLower() == name.ToLower()&&e.CityID==cityID);
        }
        public void AddRange(List<Enterprise> list, int userid)
        {
            var name = "文件导入企业";
            var dailys = new List<Daily>();
            foreach (var item in list)
            {
                Daily daily = null;
                if (Exist(item.Name,item.CityID))
                {
                    daily = new Daily
                    {
                        Name = name,
                        Description = string.Format("系统中已存在企业名称为{0}的记录了", item.Name),
                        UserID = userid,
                        CityID=item.CityID
                    };
                }
                else
                {
                    try
                    {
                        var id = Save(item);
                    } catch (Exception ex)
                    {
                        daily = new Daily
                        {
                            Name = name,
                            Description = string.Format("导入企业{0}；发生错误{1}", item.Name, ex.Message),
                            UserID = userid,
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
        /// 作用：统计企业数量
        /// 作者：汪建龙
        /// 编写时间：2017年3月16日16:08:33
        /// </summary>
        /// <returns></returns>
        public long Count(int cityID)
        {
            //var step = Db.FlowSteps.Find(2);
            return Db.Enterprises.Where(e=>e.Deleted==false&&e.CityID==cityID).LongCount();
        }

        public bool Used(int id)
        {
            return Db.Conducts.Any(e => e.DataId == id && e.SystemData == SystemData.Enterprise) || Db.Lands.Any(e => e.SystemData == SystemData.Enterprise && e.ELID == id);
        }
        /// <summary>
        /// 作用：对所有的企业进行信用评级
        /// 作者：汪建龙
        /// 编写时间：2017年3月31日14:10:47
        /// </summary>
        public void Grade(int cityID)
        {
            var feeds = new List<Feed>();
            var enterprise_scores = Db.EnterpriseScores.Where(e=>e.CityID==cityID).ToList();
            foreach(var score in enterprise_scores)
            {
                var enterprise = Db.Enterprises.Find(score.ID);
                if (enterprise == null)
                {
                    continue;
                }
                if (enterprise.Degree != score.Degree)
                {
                    feeds.Add(new Feed
                    {
                        ELID = enterprise.ID,
                        SystemData = SystemData.Enterprise,
                        Old = enterprise.Degree,
                        New = score.Degree,
                    });
                }
                enterprise.Degree = score.Degree;
                enterprise.Times = score.Times;
                enterprise.Values = score.ScoreValue;
                enterprise.Record = score.Record;
                enterprise.GradeTime = DateTime.Now;
            }
            if (feeds.Count > 0)
            {
                Db.Feeds.AddRange(feeds);
            }
            Db.SaveChanges();
        }

        /// <summary>
        /// 作用：对某一个企业进行信用评级
        /// 作者：汪建龙
        /// 编写时间：2017年3月31日14:11:15
        /// </summary>
        /// <param name="id"></param>
        public void Grade(int id,int conductId,GradeAction action)
        {
            var enterprise = Db.Enterprises.Find(id);
            if (enterprise == null)
            {
                return;
            }
            var score = Db.EnterpriseScores.Find(id);
            if (score == null)
            {
                return ;
            }
            Feed feed = null;
            if (enterprise.Degree != score.Degree)
            {
                feed = new Feed
                {
                    ELID = enterprise.ID,
                    SystemData = SystemData.Enterprise,
                    Old = enterprise.Degree,
                    New = score.Degree,
                    ConductID=conductId,
                    Action=action
                };
            }
            enterprise.Degree = score.Degree;
            enterprise.Times = score.Times;
            enterprise.Values = score.ScoreValue;
            enterprise.Record = score.Record;
            enterprise.GradeTime = DateTime.Now;
            if (feed != null)
            {
                Db.Feeds.Add(feed);
            }
            Db.SaveChanges();
        }

        public List<Enterprise> GetFull(int cityID)
        {
            var list = Db.Enterprises.Where(e => e.CityID == cityID).ToList();
            foreach(var item in list)
            {
                item.ConductStandards = Db.ConductStandards.Where(e => e.ELID == item.ID && e.SystemData == SystemData.Enterprise).ToList();
            }

            return list;
        }


        public List<EnterpriseScore> GetScores(int cityId)
        {
            return Db.EnterpriseScores.Where(e => e.CityID == cityId && e.Deleted == false).ToList();
        }
    }
}