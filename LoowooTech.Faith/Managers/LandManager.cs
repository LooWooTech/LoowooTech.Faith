using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
using LoowooTech.Faith.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class LandManager:ManagerBase
    {
        /// <summary>
        /// 作用：保存供地信息
        /// 作者：汪建龙
        /// 编写时间：2017年3月16日19:44:20
        /// </summary>
        /// <param name="land"></param>
        /// <returns></returns>
        public int Save(Land land)
        {
            Db.Lands.Add(land);
            Db.SaveChanges();
            return land.ID;
        }

        /// <summary>
        /// 作用：编辑更新供地信息
        /// 作者：汪建龙
        /// 编写时间：2017年3月16日19:44:01
        /// </summary>
        /// <param name="land"></param>
        /// <returns></returns>
        public bool Edit(Land land)
        {
            var model = Db.Lands.Find(land.ID);
            if (model == null)
            {
                return false;
            }
            Db.Entry(model).CurrentValues.SetValues(land);
            Db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 作用：获取供地信息
        /// 作者：汪建龙
        /// 编写时间：2017年3月16日19:43:41
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Land Get(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            var model = Db.Lands.Find(id);
            return model;
        }

    

        public LandView GetView(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            var model = Db.LandViews.Find(id);
            return model;
        }

        /// <summary>
        /// 作用：获取供地信息
        /// 作者：汪建龙
        /// 编写时间：2017年3月17日16:10:20
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Land Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            var model = Db.Lands.FirstOrDefault(e => e.Name.ToLower() == name.ToLower());
            return model;
        }

        /// <summary>
        /// 作用：删除供地信息
        /// 作者：汪建龙
        /// 编写时间：2017年3月16日19:40:19
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            Land model = Db.Lands.Find(id);
            if (model == null)
            {
                return false;
            }
            Db.Lands.Remove(model);
            Db.SaveChanges();
            return true;
        }
        /// <summary>
        /// 作用：批量导入供地信息
        /// 作者：汪建龙
        /// 编写时间：2017年3月16日20:22:37
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userid"></param>
        public void AddRange(List<Land> list,int userid,int cityID)
        {
            var daily = new List<Daily>();
            var inputs = new List<Land>();
            foreach(var item in list)
            {
                var enterprise = Core.EnterpriseManager.Get(item.ELName,cityID);
                if (enterprise == null)
                {
                    var lawyer = Core.LawyerManager.Get(item.ELName,cityID);
                    if (lawyer != null)
                    {
                        item.ELID = lawyer.ID;
                        item.SystemData = SystemData.Lawyer;
                    }
                }
                else
                {
                    item.ELID = enterprise.ID;
                    item.SystemData = SystemData.Enterprise;
                }

                if (item.ELID == 0)
                {
                    daily.Add(new Daily
                    {
                        Name = "批量导入供地信息",
                        Description = string.Format("未查询到名称为{0}的企业或者自然人信息",item.ELName),
                        UserID = userid,
                        CityID=cityID
                    });
                    continue;
                }
                inputs.Add(item);
            }
            if (inputs.Count > 0)
            {
                Db.Lands.AddRange(inputs);
                Db.SaveChanges();
            }
            if (daily.Count > 0)
            {
                Core.DailyManager.AddRange(daily);
            }
            
        }
        /// <summary>
        /// 作用：查询
        /// 作者：汪建龙
        /// 编写时间：2017年3月16日20:41:13
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List<LandView> Search(LandViewParameter parameter)
        {
            var query = Db.LandViews.AsQueryable();
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
            if (!string.IsNullOrEmpty(parameter.Name))
            {
                query = query.Where(e => e.Name.ToLower().Contains(parameter.Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.sName))
            {
                query = query.Where(e => e.sName.ToLower().Contains(parameter.sName.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.Number))
            {
                query = query.Where(e => e.Number.ToLower().Contains(parameter.Number.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.ContractNumber))
            {
                query = query.Where(e => e.ContractNumber.ToLower().Contains(parameter.ContractNumber.ToLower()));
            }
            if (parameter.Way.HasValue)
            {
                query = query.Where(e => e.Way == parameter.Way.Value);
            }
            if (parameter.Order.HasValue)
            {
                switch (parameter.Order.Value)
                {
                    case LandOrder.ContractNumber:
                        query = query.OrderBy(e => e.ContractNumber);
                        break;
                    case LandOrder.Name:
                        query = query.OrderBy(e => e.Name);
                        break;
                    case LandOrder.sName:
                        query = query.OrderBy(e => e.sName);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(e => e.CreateTime);
            }
            query = query.SetPage(parameter.Page);
            return query.ToList();
        }
        /// <summary>
        /// 作用：验证是否存在项目名称记录
        /// 作者：汪建龙
        /// 编写时间：2017年3月17日16:11:08
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exist(string name)
        {
            var model = Get(name);
            return model != null;
        }
        /// <summary>
        /// 作用：获取供地信息数量
        /// 作者：汪建龙
        /// 编写时间：2017年3月18日14:07:44
        /// </summary>
        /// <returns></returns>
        public long Count(int cityID)
        {
            return Db.LandViews.Where(e=>e.CityID==cityID).LongCount();
        }


        public long Count(int ELID,SystemData systemData)
        {
            return Db.Lands.Where(e => e.ELID == ELID && e.SystemData == systemData).LongCount();
        }
    }
}