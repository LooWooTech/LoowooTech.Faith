using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    /// <summary>
    /// 违法用地
    /// </summary>
    [Table("landrecord")]
    public class LandRecord
    {
        public LandRecord()
        {
            CreateTime = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 企业或者自然人ID
        /// </summary>
        public int ELID { get; set; }
        public SystemData SystemData { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 查处文号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 违法用地面积
        /// </summary>
        public double IllegalArea { get; set; }
        /// <summary>
        /// 合法用地面积
        /// </summary>
        public double Area { get; set; }
        /// <summary>
        /// 应扣分值
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// 录入人ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public LandRecordState State { get; set; }
    }
    public enum LandRecordState
    {
        [Description("录入")]
        Enter,
        [Description("解除")]
        Relieve
    }
}