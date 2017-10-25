using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    [Table("lawyer")]
    public class Lawyer:Subject
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int ID { get; set; }

        public Sex Sex { get; set; }
        public string BornTime { get; set; }
        public Credential Credential { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
        public string TelPhone { get; set; }
        public string EMail { get; set; }
        public int? EnterpriseID { get; set; }
        public int CityID { get; set; }
    }

    public enum Sex
    {
        [Description("男")]
        Male,
        [Description("女")]
        Female
    }

    public enum Credential
    {
        [Description("身份证")]
        Identity,
        [Description("护照")]
        Passport,
        [Description("港澳通信证")]
        EEP,
        [Description("士官证")]
        Soldier,
        [Description("其他")]
        Other
    }

    [Table("lawyer_scores")]
    public class LawyerScore : BaseScore
    {

    }
}