using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    public class Letter
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string Credit { get; set; }
        public string Description { get; set; }
        public string Contact { get; set; }
        public string TelPhone { get; set; }
        public string Time { get; set; }
        public Book Book { get; set; }
        public int DataID { get; set; }
        /// <summary>
        /// 签发单位
        /// </summary>
        public string Signature { get; set; }
        public SystemData SystemData { get; set; }
        public List<ConductStandard> Conducts { get; set; }
        public List<LandRecordView> LandRecord { get; set; }

    }

    public enum Book
    {
        [Description("告知书")]
        Notification,
        [Description("认定书")]
        Protocol
    }
}