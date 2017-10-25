using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Parameters
{
    public class ParameterBase
    {
        public ParameterBase()
        {
            SearchStartTime = DateTime.Now;
        }
        public PageParameter Page { get; set; }
        public DateTime SearchStartTime { get; set; }
        public DateTime SearchEndTime { get; set; }
        public string TimeSpan
        {
            get
            {
                var span = SearchEndTime - SearchStartTime;
                return string.Format("{0}分{1}秒", span.Minutes, span.Seconds);
            }
        }
        public int ? CityID { get; set; }
    }
}