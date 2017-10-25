using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    [Table("standard")]
    public class Standard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public Credit Credit { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// 名称注释
        /// </summary>
        public string Summary { get; set; }
        public double Score { get; set; }
        public CreditDegree Degree { get; set; }
    }
}