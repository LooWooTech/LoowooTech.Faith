using LoowooTech.Faith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class CityManager:ManagerBase
    {
        public City Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            var model = Db.Citys.FirstOrDefault(e => e.Name.ToLower() == name.ToLower());
            return model;
        }


    }
}