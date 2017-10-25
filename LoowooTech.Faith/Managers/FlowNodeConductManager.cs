using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
using LoowooTech.Faith.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class FlowNodeConductManager:ManagerBase
    {
        /// <summary>
        /// 作用：查询
        /// 作者：汪建龙
        /// 编写时间：2017年3月18日17:14:57
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List<FlowNodeConduct> Search(FlowNodeConductParameter parameter)
        {
            var query = Db.FlowNodeConducts.AsQueryable();
            if (parameter.UserID.HasValue)
            {
                query = query.Where(e => e.UserID == parameter.UserID.Value);
            }
            if (parameter.FlowNodeState.HasValue)
            {
                query = query.Where(e => e.FlowNodeState == parameter.FlowNodeState.Value);
            }
            query = query.OrderByDescending(e => e.CreateTime).SetPage(parameter.Page);
            return query.ToList();
        }

        /// <summary>
        /// 作用：获取
        /// 作者：汪建龙
        /// 编写时间：2017年3月18日18:35:59
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FlowNodeConduct Get(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            var model = Db.FlowNodeConducts.Find(id);
            return model;
        }

        public List<FlowNodeConduct> SearchHistory(PageParameter page,int CityId)
        {
            var query = Db.FlowNodeConducts.Where(e => e.FlowNodeState != DoingState.None&&e.CityId==CityId).AsQueryable();
            query = query.OrderByDescending(e => e.CreateTime).SetPage(page);
            return query.ToList();
        }
    }
}