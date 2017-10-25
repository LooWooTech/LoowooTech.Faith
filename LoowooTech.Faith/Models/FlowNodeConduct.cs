using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    [Table("flownode_conduct_view")]
    public class FlowNodeConduct
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int InfoID { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime ConductCreateTime { get; set; }
        public Credit Credit { get; set; }
        public CreditDegree Degree { get; set; }
        public string StandardName { get; set; }
        public double Score { get; set; }
        public string UserName { get; set; }
        public DoingState FlowNodeState { get; set; }
        public string LandName { get; set; }
        public int LandID { get; set; }
        public int ELID { get; set; }
        public SystemData SystemData { get; set; }
        public string ELName { get; set; }
        public string Remark { get; set; }
        public int CityId { get; set; }
    }
}