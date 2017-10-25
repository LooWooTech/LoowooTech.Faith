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
    /// 诚信行为记录
    /// </summary>
    [Table("conduct")]
    public class Conduct
    {
        public Conduct()
        {
            CreateTime = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime CreateTime { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 备注(针对录入人员)
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 行为接触备注（）
        /// </summary>
        public string Memo { get; set; }
        public int DataId { get; set; }
        public SystemData SystemData { get; set; }

        [NotMapped]
        public ScoreBase ScoreBase { get; set; }
        /// <summary>
        /// 对应的供地信息
        /// </summary>
        public int LandID { get; set; }
        public virtual Land Land { get; set; }

        public Credit Credit { get; set; }
        public CreditDegree Degree { get; set; }
        public int StandardId { get; set; }
        [ForeignKey("StandardId")]
        public Standard Standard { get; set; }
        public double Score { get; set; }
        /// <summary>
        /// 审核结果  空  表示未审核  Checking 正在审核 
        /// </summary>
        public BaseState? State { get; set; }
        /// <summary>
        /// 录入人的ID
        /// </summary>
        public int UserID { get; set; }
        
    }

    public enum SystemData
    {
        [Description("企业")]
        Enterprise,
        [Description("自然人")]
        Lawyer
    }

    public enum CreditDegree
    {
        [Description("轻微不诚信")]
        Good,
        [Description("不诚信行为")]
        Less,
        [Description("严重不诚信")]
        Bad
    }

    public enum Credit
    {
        [Description("竞买")]
        JingMai,
        [Description("开发利用")]
        KaiFaLiYong,
        [Description("违规违法")]
        WeiGui,
        [Description("合同履约")]
        HeTongLvYue,
        [Description("其他")]
        Other
    }
}