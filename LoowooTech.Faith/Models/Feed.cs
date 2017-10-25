using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    [Table("feed")]
    public class Feed
    {
        public Feed()
        {
            CreateTime = DateTime.Now;
        }
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ELID { get; set; }
        public SystemData SystemData { get; set; }
        public GradeDegree? Old { get; set; }
        public GradeDegree? New { get; set; }
        public DateTime CreateTime { get; set; }
        public bool HasRead { get; set; }
        public int? ConductID { get; set; }
        public GradeAction Action { get; set; }
        public string Remark { get; set; }
    }

    [Table("feed_view")]
    public class FeedView
    {
        public int ID { get; set; }
        public GradeDegree? Old { get; set; }
        public GradeDegree? New { get; set; }
        public bool HasRead { get; set; }
        public DateTime CreateTime { get; set; }
        public int ELID { get; set; }
        public SystemData SystemData { get; set; }
        public int? ConductID { get; set; }
        public string ELName { get; set; }
        public int? LandID { get; set; }
        public string LandName { get; set; }
        public Credit? Credit { get; set; }
        public CreditDegree? Degree { get; set; }
        public int? StandardID { get; set; }
        public string StandardName { get; set; }
        public GradeAction Action { get; set; }
        public int CityID { get; set; }
    }
    public enum GradeAction
    {
        [Description("年度评级")]
        Year,
        [Description("诚信行为")]
        Conduct,
        [Description("删除诚信行为")]
        DeleteConduct,
        [Description("解除诚信行为")]
        Relieve,
        [Description("撤销解除诚信")]
        CanRelieve,
        [Description("录入违法用地")]
        AddLandRecord,
        [Description("编辑违法用地")]
        EditLandRecord,
        [Description("删除违法用地")]
        DeleteLandRecord,
        [Description("管理员更改")]
        AdministratorChange
    }
}