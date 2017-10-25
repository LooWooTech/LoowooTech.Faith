using LoowooTech.Faith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class FaithFileManager:ManagerBase
    {
        public string GetName(int sid,FileType type)
        {
            var result = string.Empty;
            switch (type)
            {
                case FileType.Conduct:
                    var conduct = Db.ConductStandards.FirstOrDefault(e => e.ID == sid);
                    if (conduct != null)
                    {
                        result = conduct.ELName;
                    }
                    break;
                case FileType.LandRecord:
                    var landRecord = Db.LandRecordViews.FirstOrDefault(e => e.ID == sid);
                    if (landRecord != null)
                    {
                        result = landRecord.ELName;
                    }
                    break;
            }

            return result;
        }

        public int Add(FaithFile file)
        {
            Db.Files.Add(file);
            Db.SaveChanges();
            return file.ID;
        }


        public List<FaithFile> Get(int sid,FileType type)
        {
            return Db.Files.Where(e => e.Delete == false && e.SID == sid && e.Type == type).OrderBy(e=>e.Time).ToList();
        }

        public void Delete(int[] ids)
        {
            foreach(var item in ids)
            {
                Delete(item);
            }
        }

        public bool Delete(int id)
        {
            var entry = Db.Files.Find(id);
            if (entry != null)
            {
                entry.Delete = true;
                Db.SaveChanges();
                return true;
            }

            return false;
        }

        public FaithFile Get(int id)
        {
            return Db.Files.FirstOrDefault(e => e.ID == id);
        }
    }
}