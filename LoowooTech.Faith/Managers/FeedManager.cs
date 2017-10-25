using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
using LoowooTech.Faith.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class FeedManager:ManagerBase
    {
        public List<FeedView> Search(FeedParameter parameter)
        {
            var query = Db.FeedViews.AsQueryable();
            if (parameter.CityID.HasValue)
            {
                query = query.Where(e => e.CityID == parameter.CityID.Value);
            }
            if (parameter.Old.HasValue)
            {
                query = query.Where(e => e.Old == parameter.Old.Value);
            }
            if (parameter.New.HasValue)
            {
                query = query.Where(e => e.New == parameter.New.Value);
            }
            if (parameter.HasRead.HasValue)
            {
                query = query.Where(e => e.HasRead == parameter.HasRead.Value);
            }
            if (!string.IsNullOrEmpty(parameter.ELName))
            {
                query = query.Where(e => e.ELName.ToLower().Contains(parameter.ELName.ToLower()));
            }


            query = query.OrderByDescending(e => e.CreateTime).SetPage(parameter.Page);
            return query.ToList();
        }

        public long Count(int cityID, bool hasRead=false)
        {
            return Db.FeedViews.Where(e=>e.HasRead==hasRead&&e.CityID==cityID).LongCount();
        }

        /// <summary>
        /// 作用：标记已读
        /// 作者：汪建龙
        /// 编写时间：2017年3月31日16:07:13
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Read(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            var model = Db.Feeds.Find(id);
            if (model == null)
            {
                return false;
            }
            model.HasRead = true;

            Db.SaveChanges();
            return true;
        }


        public int Save(Feed feed)
        {
            Db.Feeds.Add(feed);
            Db.SaveChanges();
            return feed.ID;
        }

        public void AddList(List<Feed> feeds)
        {
            Db.Feeds.AddRange(feeds);
            Db.SaveChanges();
        }
    }
}