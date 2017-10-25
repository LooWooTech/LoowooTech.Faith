using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    [Table("landrecord_view")]
    public class LandRecordView
    {
        public int ID { get; set; }
        public int ELID { get; set; }
        public SystemData SystemData { get; set; }
        public string ELName { get; set; }
        public DateTime CreateTime { get; set; }
        public string Code { get; set; }
        public double IllegalArea { get; set; }
        public double Area { get; set; }
        public double Score { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Remark { get; set; }
        public LandRecordState State { get; set; }
        public int CityID { get; set; }
    }
}