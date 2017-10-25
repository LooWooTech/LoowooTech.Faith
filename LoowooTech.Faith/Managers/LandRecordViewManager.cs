using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
using LoowooTech.Faith.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class LandRecordViewManager:ManagerBase
    {
        /// <summary>
        /// 作用：查询
        /// 作者：汪建龙
        /// 编写时间：2017年3月19日15:34:38
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List<LandRecordView> Search(LandRecordViewParameter parameter)
        {
            var query = Db.LandRecordViews.AsQueryable();
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
            if (parameter.State.HasValue)
            {
                query = query.Where(e => e.State == parameter.State.Value);
            }
            if (!string.IsNullOrEmpty(parameter.Code))
            {
                query = query.Where(e => e.Code.ToLower().Contains(parameter.Code.ToLower()));
            }
            if (!string.IsNullOrEmpty(parameter.ELName))
            {
                query = query.Where(e => e.ELName.ToLower().Contains(parameter.ELName.ToLower()));
            }
            if (parameter.MinScore.HasValue)
            {
                query = query.Where(e => e.Score >= parameter.MinScore.Value);
            }

            if (parameter.MaxScore.HasValue)
            {
                query = query.Where(e => e.Score <= parameter.MaxScore.Value);
            }

            query = query.OrderByDescending(e => e.CreateTime).SetPage(parameter.Page);
            return query.ToList();
        }
        public LandRecordView Get(int id)
        {
            var model = Db.LandRecordViews.Find(id);
            return model;
        }
        public void AddRange(List<LandRecordView> list,int userID)
        {
            var inputs = new List<LandRecord>();
            var dailys = new List<Daily>();
            foreach(var item in list)
            {
                
                var enterprise = Core.EnterpriseManager.Get(item.ELName,item.CityID);
                if (enterprise == null)
                {
                    var lawyer = Core.LawyerManager.Get(item.ELName,item.CityID);
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
                    dailys.Add(new Daily
                    {
                        Name = "文件导入违法用地",
                        Description = string.Format("未找到名称为{0}的企业或者自然人信息", item.ELName),
                        UserID = userID,
                        CityID=item.CityID
                    });
                    continue;
                }
                inputs.Add(new LandRecord
                {
                    ELID = item.ELID,
                    SystemData = item.SystemData,
                    Code = item.Code,
                    IllegalArea = item.IllegalArea,
                    Area = item.Area,
                    Score = item.Score,
                    UserID = userID,
                    Remark=item.Remark
                });
            }
            if (inputs.Count > 0)
            {
                Db.LandRecords.AddRange(inputs);
                Db.SaveChanges();
            }
            if (dailys.Count > 0)
            {
                Core.DailyManager.AddRange(dailys);
            }

        }
    }
}