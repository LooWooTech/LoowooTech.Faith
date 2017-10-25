using LoowooTech.Faith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class GradeHistoryManager:ManagerBase
    {
      
        /// <summary>
        /// 作用：删除信用评级记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var model = Db.GradeHistorys.Find(id);
            if (model != null)
            {
                model.Delete = true;
                Db.SaveChanges();
                return true;
            }
            return false;
        }

        public GradeHistory Get(int id)
        {
            return Db.GradeHistorys.Find(id);
        }

        public int Add(GradeHistory grade)
        {
            Db.GradeHistorys.Add(grade);
            Db.SaveChanges();
            return grade.ID;
        }

        public List<GradeHistory> GetList(int CityId)
        {
            return Db.GradeHistorys.Where(e => e.Delete == false&&e.CityId==CityId).OrderBy(e => e.ID).ToList();
        }

        public bool Edit(GradeHistory grade)
        {
            var model = Db.GradeHistorys.Find(grade.ID);
            if (model != null)
            {
                Db.Entry(model).CurrentValues.SetValues(grade);
                Db.SaveChanges();
                return true;
            }
            return false;
        }

    }
}