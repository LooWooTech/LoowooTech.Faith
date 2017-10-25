using LoowooTech.Faith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class RollViewManager:ManagerBase
    {
        /// <summary>
        /// 作用：获取某一类名单列表
        /// 作者：汪建龙
        /// 编写时间：2017年3月8日09:33:37
        /// </summary>
        /// <param name="brenum"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<RollView> GetList(BREnum brenum,string key)
        {
            var query = Db.RollViews.Where(e => e.BREnum == brenum).AsQueryable();
            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(e => e.Name.ToLower().Contains(key.ToLower()));
            }
            return query.ToList();
        }

        public long Count(BREnum brnum,int cityID)
        {
            return GetRollList(brnum, null,cityID).LongCount();
        }

        public List<RollList> GetRollList(BREnum brenum,string key,int cityID, bool isStandard=false)
        {
            return GetList(brenum == BREnum.Black ? GradeDegree.D : GradeDegree.C, key,isStandard,cityID);
        }

        

        public List<RollList> GetList(GradeDegree degree,string key,bool isStandard,int cityID)
        {
            var result = new List<RollList>();
            result.AddRange(GetEnterprise(degree,key,cityID));
            result.AddRange(GetLawyer(degree, key,cityID));
            if (isStandard)
            {
                try
                {
                    foreach (var item in result)
                    {
                        item.ConductStandards = Core.ConductStandardManager.Search(new Parameters.ConductStandardParameter { ELID = item.DataId, SystemData = item.SystemData, State = BaseState.Argee,CityID=cityID });
                    }
                }
                catch(Exception ex)
                {
                    throw new ArgumentException(ex.ToString());
                }
              
            }
            return result;
        }

        public List<RollList> GetEnterprise(GradeDegree degree,string key,int cityID)
        {
            var query = Db.Enterprises.Where(e => e.Deleted == false && e.Degree == degree&&e.CityID==cityID).AsQueryable();
            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(e => e.Name.ToLower().Contains(key.ToLower()));
            }
            return query.ToList().Select(e => new RollList { DataId = e.ID, SystemData = SystemData.Enterprise, Name = e.Name }).ToList();
        }

        public List<RollList> GetLawyer(GradeDegree degree,string key,int cityID)
        {
            var query = Db.Lawyers.Where(e => e.Deleted == false && e.Degree == degree&&e.CityID==cityID).AsQueryable();
            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(e => e.Name.ToLower().Contains(key.ToLower()));
            }
            return query.ToList().Select(e => new RollList { DataId = e.ID, SystemData = SystemData.Lawyer, Name = e.Name }).ToList();
        }


        public List<RollView> GetList()
        {
            return Db.RollViews.ToList();
        }
    }
}