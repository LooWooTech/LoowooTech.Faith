using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
using LoowooTech.Faith.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class ConductStandardManager:ManagerBase
    {
        /// <summary>
        /// 作用：获取某一个供地的诚信记录
        /// 作者：汪建龙
        /// 编写时间：2017年3月18日16:15:44
        /// </summary>
        /// <param name="landID"></param>
        /// <returns></returns>
        public List<ConductStandard> GetByLandID(int landID)
        {
            var list = Db.ConductStandards.Where(e => e.LandID == landID).ToList();
            return list;
        }
        /// <summary>
        /// 作用：
        /// 作者：汪建龙
        /// 编写时间：2017年3月20日11:07:41
        /// </summary>
        /// <param name="dict">受让人->供地项目名称->扣分详情</param>
        /// <param name="userID"></param>
        public void AddRange(Dictionary<string,Dictionary<string,List<ConductStandard>>> dict,int userID,int cityId)
        {
            var inputs = new List<Conduct>();
            var dailys = new List<Daily>();
            foreach(var entry in dict)
            {
                #region  获取受让人信息
                var elid = 0;
                SystemData? systemdata=null;
                var enterprise = Core.EnterpriseManager.Get(entry.Key,cityId);
                if (enterprise == null)
                {
                    var lawyer = Core.LawyerManager.Get(entry.Key,cityId);
                    if (lawyer != null)
                    {
                        elid = lawyer.ID;
                        systemdata = SystemData.Lawyer;
                    }
                }
                else
                {
                    elid = enterprise.ID;
                    systemdata = SystemData.Enterprise;
                }
                if (elid == 0 || systemdata == null)
                {
                    dailys.Add(new Daily
                    {
                        Name = "文件导入诚信行为",
                        Description = string.Format("未找到名称为{0}的企业或者自然人信息", entry.Key),
                        UserID = userID,
                        CityID=cityId
                    });
                    continue;
                }
                #endregion


                foreach (var item in entry.Value)
                {
                    #region  获取供地信息
                    var land = Core.LandManager.Get(item.Key);
                    if (land == null)
                    {
                        dailys.Add(new Daily
                        {
                            Name = "文件导入诚信记录",
                            Description = string.Format("未找到项目名称为{0}的供地信息", item.Key),
                            UserID = userID
                        });
                        continue;
                    }
                    if (land.ELID != elid || land.SystemData != systemdata)
                    {
                        dailys.Add(new Daily
                        {
                            Name = "文件导入诚信记录",
                            Description = string.Format("项目名称：{0} 对应的{1}企业自然人不符", item.Key, entry.Key),
                            UserID = userID
                        });
                        continue;
                    }
                    #endregion
                    foreach (var model in item.Value)
                    {
                        #region 获取诚信行为
                        var standard = Core.StandardManager.Get(model.StandardName);
                        if (standard == null)
                        {
                            dailys.Add(new Daily
                            {
                                Name = "文件导入诚信记录",
                                Description = string.Format("未找到名称为{0}的诚信行为", model.StandardName),
                                UserID = userID
                            });
                            continue;
                        }
                        #endregion
                        inputs.Add(new Conduct
                        {
                            Remark = "文件导入",
                            LandID = land.ID,
                            Credit = standard.Credit,
                            Degree = model.Score <= 10 ? CreditDegree.Good : model.Score <= 20 ? CreditDegree.Less : CreditDegree.Bad,
                            StandardId = standard.ID,
                            Score = model.Score,
                            State = BaseState.Argee,
                            UserID = userID
                        });
                    }
                }

            }

            if (inputs.Count > 0)
            {
                Db.Conducts.AddRange(inputs);
                Db.SaveChanges();
            }
            if (dailys.Count > 0)
            {
                Core.DailyManager.AddRange(dailys);
            }
        }
        /// <summary>
        /// 作用：查询
        /// 作者：汪建龙
        /// 编写时间：2017年3月20日16:15:13
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List<ConductStandard> Search(ConductStandardParameter parameter)
        {
            var query = Db.ConductStandards.AsQueryable();
            if (parameter.CityID.HasValue)
            {
                query = query.Where(e => e.CityID == parameter.CityID.Value);
            }
            if (parameter.ELID.HasValue)
            {
                query = query.Where(e => e.ELID == parameter.ELID.Value);
            }


            if (parameter.SystemData.HasValue)
            {
                query = query.Where(e => e.SystemData == parameter.SystemData.Value);
            }
            if (parameter.StartTime.HasValue)
            {
                query = query.Where(e => e.CreateTime >= parameter.StartTime.Value);
            }
            if (parameter.EndTime.HasValue)
            {
                query = query.Where(e => e.CreateTime <= parameter.EndTime.Value);
            }
            if (parameter.Degree.HasValue)
            {
                query = query.Where(e => e.Degree == parameter.Degree.Value);
            }
            if (!string.IsNullOrEmpty(parameter.ELName))
            {
                query = query.Where(e => e.ELName.ToLower().Contains(parameter.ELName.ToLower()));
            }
            if (parameter.Credit.HasValue)
            {
                query = query.Where(e => e.Credit == parameter.Credit.Value);
            }

            if (!string.IsNullOrEmpty(parameter.StandardName))
            {
                query = query.Where(e => e.StandardName.ToLower().Contains(parameter.StandardName.ToLower()));
            }
            if (parameter.MinScore.HasValue)
            {
                query = query.Where(e => e.Score >= parameter.MinScore.Value);
            }
            if (parameter.MaxScore.HasValue)
            {
                query = query.Where(e => e.Score <= parameter.MaxScore.Value);
            }
            if (parameter.State.HasValue)
            {
                query = query.Where(e => e.State == parameter.State.Value);
            }

            query = query.OrderByDescending(e => e.CreateTime).SetPage(parameter.Page);
            return query.ToList();
        }

        public List<ConductStandard> Get(int elID,SystemData systemdata,PageParameter page = null)
        {
            var query = Db.ConductStandards.Where(e => e.ELID == elID && e.SystemData == systemdata).OrderByDescending(e => e.CreateTime).AsQueryable();
            if (page != null)
            {
                query = query.SetPage(page);
            }
            return query.ToList();
        }

        public ConductStandard Get(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            var model = Db.ConductStandards.Find(id);
            return model;
        }
        public long Count(int cityID)
        {
            return Db.ConductStandards.Where(e => e.CityID == cityID).LongCount();
        }
    }
}