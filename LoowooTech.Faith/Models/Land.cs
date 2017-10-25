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
    /// 拿地记录
    /// </summary>
    [Table("land")]
    public class Land
    {
        public Land()
        {
            CreateTime = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ELID { get; set; }
        public SystemData SystemData { get; set; }
        [NotMapped]
        public string ELName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 电子监管号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractNumber { get; set; }
        /// <summary>
        /// 宗地编号
        /// </summary>
        public string LandNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SoldWay? Way { get; set; }
        /// <summary>
        /// 供应面积(公顷)
        /// </summary>
        public double Area { get; set; }
        /// <summary>
        /// 代征面积
        /// </summary>
        public double? ReplaceArea { get; set; }
        /// <summary>
        /// 出让金额
        /// </summary>
        public double Money { get; set; }
        /// <summary>
        /// 批准文号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 签订时间
        /// </summary>
        public DateTime SignTime { get; set; }
        /// <summary>
        /// 批准时间
        /// </summary>
        public DateTime ApproveTime { get; set; }
        /// <summary>
        /// 是否回收
        /// </summary>
        public bool Recycle { get; set; }
        /// <summary>
        /// 土地坐落
        /// </summary>
        public string Location { get; set; }
        public DateTime CreateTime { get; set; }

        
    }

    [Table("landView")]
    public class LandView
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ELID { get; set; }
        public SystemData SystemData { get; set; }
        public string sName { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 电子监管号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractNumber { get; set; }
        /// <summary>
        /// 宗地编号
        /// </summary>
        public string LandNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SoldWay? Way { get; set; }
        /// <summary>
        /// 供应面积(公顷)
        /// </summary>
        public double Area { get; set; }
        /// <summary>
        /// 代征面积
        /// </summary>
        public double? ReplaceArea { get; set; }
        /// <summary>
        /// 出让金额
        /// </summary>
        public double Money { get; set; }
        /// <summary>
        /// 批准文号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 签订时间
        /// </summary>
        public DateTime SignTime { get; set; }
        /// <summary>
        /// 批准时间
        /// </summary>
        public DateTime ApproveTime { get; set; }
        /// <summary>
        /// 是否回收
        /// </summary>
        public bool Recycle { get; set; }
        /// <summary>
        /// 土地坐落
        /// </summary>
        public string Location { get; set; }
        public DateTime CreateTime { get; set; }
        public int CityID { get; set; }
    }

    public enum SoldWay
    {
        [Description("挂牌出让")]
        GuaPai,
        [Description("拍卖出让")]
        PaiMai,
        [Description("协议出让")]
        XieYi,
        [Description("招标出让")]
        ZhaoBiao
    }
}