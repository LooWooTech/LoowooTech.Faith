using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    [Table("enterprise")]
    public class Enterprise:Subject
    {
       
        /// <summary>
        /// 组织机构代码
        /// </summary>
        [Description("组织机构代码")]
        public string OIBC { get; set; }
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        [Description("统一社会信用代码")]
        public string USCC { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [Description("详细地址")]
        public string Address { get; set; }
        /// <summary>
        /// 法人
        /// </summary>
        [Description("企业法人")]
        public string Lawyer { get; set; }
        /// <summary>
        /// 法人身份证号
        /// </summary>
        [Description("企业法人身份证号")]
        public string LawNumber { get; set; }
        /// <summary>
        /// 营业执照/注册号
        /// </summary>
        [Description("营业执照/注册号")]
        public string Number { get; set; }
        /// <summary>
        /// 经营范围
        /// </summary>
        [Description("经营范围")]
        public string Scope { get; set; }
        /// <summary>
        /// 企业类型
        /// </summary>
        [Description("企业类型")]
        public string Type { get; set; }
        /// <summary>
        /// 注册资本
        /// </summary>
        [Description("注册资本")]
        public double Money { get; set; }
        /// <summary>
        /// 企业联系方式
        /// </summary>
        [Description("企业电话/公司电话")]
        public string ContactWay { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [Description("联系人")]
        public string Contact { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Description("联系人电话")]
        public string TelPhone { get; set; }
        [NotMapped]
        public List<Conduct> Conducts { get; set; }
        public int? LawyerID { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityID { get; set; }

        [NotMapped]
        public List<ConductStandard> ConductStandards { get; set; }


    }

    [Table("enterprise_scores")]
    public class EnterpriseScore:BaseScore
    {
 
    }
}