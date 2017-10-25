using LoowooTech.Faith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class LandRecordManager:ManagerBase
    {
        /// <summary>
        /// 作用：添加那地记录
        /// 作者：汪建龙
        /// 编写时间：2017年3月16日18:35:37
        /// </summary>
        /// <param name="landRecord"></param>
        /// <returns></returns>
        public int Save(LandRecord landRecord)
        {
            Db.LandRecords.Add(landRecord);
            Db.SaveChanges();
            Grade(landRecord, GradeAction.AddLandRecord);
            return landRecord.ID;
        }

        /// <summary>
        /// 作用：编辑更新拿地记录
        /// 作者：汪建龙
        /// 编写时间：2017年3月16日18:35:58
        /// </summary>
        /// <param name="landRecord"></param>
        /// <returns></returns>
        public bool Edit(LandRecord landRecord)
        {
            var model = Db.LandRecords.Find(landRecord.ID);
            if (model == null)
            {
                return false;
            }
            Db.Entry(model).CurrentValues.SetValues(landRecord);
            Db.SaveChanges();
            Grade(model, GradeAction.EditLandRecord);
            return true;
        }
        /// <summary>
        /// 作用：通过ID获取拿地记录
        /// 作者：汪建龙
        /// 编写时间：2017年3月16日18:36:57
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LandRecord Get(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            var model = Db.LandRecords.Find(id);
            return model;
        }
        /// <summary>
        /// 作用：删除拿地记录
        /// 作者：汪建龙
        /// 编写时间：2017年3月16日18:38:00
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var model = Db.LandRecords.Find(id);
            if (model == null)
            {
                return false;
            }

            Db.LandRecords.Remove(model);
            Db.SaveChanges();
            Grade(model, GradeAction.DeleteLandRecord);
            return true;
        }

        public List<LandRecord> Get(int ELID,SystemData systemData)
        {
            var list = Db.LandRecords.Where(e => e.ELID == ELID && e.SystemData == systemData).ToList();
            return list;
        }
        public long Count(int cityID)
        {
            return Db.LandRecordViews.Where(e=>e.CityID==cityID).LongCount();
        }

        public void Grade(LandRecord record,GradeAction action)
        {
            if (record.SystemData == SystemData.Enterprise)
            {
                Core.EnterpriseManager.Grade(record.ELID, record.ID, action);
            }else if (record.SystemData == SystemData.Lawyer)
            {
                Core.LawyerManager.Grade(record.ELID, record.ID, action);
            }
        }
        public bool Relieve(int id,string remark)
        {
            if (id <= 0)
            {
                return false;
            }
            var model = Db.LandRecords.Find(id);
            if (model == null)
            {
                return false;
            }
            model.State = LandRecordState.Relieve;
            if (!string.IsNullOrEmpty(remark))
            {
                model.Remark += string.Format("解除备注：{0}", remark);
            }
           
            Db.SaveChanges();
            return true;
        }
        public bool CancelRelieve(int id)
        {
            var model = Db.LandRecords.Find(id);
            if (model == null)
            {
                return false;
            }
            model.State = LandRecordState.Enter;
            Db.SaveChanges();
            return true;
        }
    }
}