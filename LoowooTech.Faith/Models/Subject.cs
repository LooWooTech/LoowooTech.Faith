using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    public class Subject:ScoreBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        #region  信用评级
        /// <summary>
        /// 评级时间
        /// </summary>
        public DateTime? GradeTime { get; set; }
        /// <summary>
        /// 购地次数
        /// </summary>
        public long? Times { get; set; }
        /// <summary>
        /// 诚信行为扣分总和
        /// </summary>
        public double? Values { get; set; }
        /// <summary>
        /// 违法用地扣分记录
        /// </summary>
        public int? Record { get; set; }
        public GradeDegree? Degree { get; set; }
        /// <summary>
        /// 诚信行为扣分记录平均
        /// </summary>
        [NotMapped]
        public int Average
        {
            get
            {
                if (Values.HasValue && Times.HasValue)
                {
                    return (int)((double)Values.Value / Times.Value + 0.5);
                }
                else
                {
                    return 0;
                }
               
            }
        }
        [NotMapped]
        public int DeDuck
        {
            get
            {
                if (Record.HasValue)
                {
                    return  Average + Record.Value;
                }
                return Average;
            }
        }
        #endregion

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// 删除备注
        /// </summary>
        public string Remark { get; set; }
    }
}