using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    public class ClassDescription
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Keys
        {
            get
            {
                if (!string.IsNullOrEmpty(Description))
                {
                    return Description.Split('/');
                }
                return null;
            }
        }
        public int Index { get; set; }

    }
}