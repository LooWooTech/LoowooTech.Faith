using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    [Table("conduct_standard_view")]
    public class ConductStandard
    {
        public int ID { get; set; }
        /// <summary>
        /// 供地ID
        /// </summary>
        public int LandID { get; set; }
        /// <summary>
        /// 供地项目名称
        /// </summary>
        public string LandName { get; set; }
        /// <summary>
        /// 宗地编号
        /// </summary>
        public string LandNumber { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractNumber { get; set; }
        /// <summary>
        /// 工地面积
        /// </summary>
        public double Area { get; set; }
        /// <summary>
        /// 诚信环节
        /// </summary>
        public Credit Credit { get; set; }
        /// <summary>
        /// 诚信等级
        /// </summary>
        public CreditDegree Degree { get; set; }
        /// <summary>
        /// 诚信行为ID
        /// </summary>
        public int standardID { get; set; }
        /// <summary>
        /// 诚信行为名称
        /// </summary>
        public string StandardName { get; set; }
        /// <summary>
        /// 诚信编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 分值
        /// </summary>
        public double Score { get; set; }
        public BaseState? State { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public int ELID { get; set; }
        public SystemData SystemData { get; set; }
        /// <summary>
        /// 受让人
        /// </summary>
        public string ELName { get; set; }
        public string Remark { get; set; }
        public int CityID { get; set; }
    }
}