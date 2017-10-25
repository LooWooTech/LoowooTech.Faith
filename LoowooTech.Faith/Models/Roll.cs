using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    [Table("roll")]
    public class Roll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int DataId { get; set; }
        public SystemData SystemData { get; set; }
        public BREnum BREnum { get; set; }

    }

    public class RollList:Roll
    {
        public string Name { get; set; }
        [NotMapped]
        public List<ConductStandard> ConductStandards { get; set; }

    }

    public enum BREnum
    {
        [Description("黑名单")]
        Black,
        [Description("异常名单")]
        Red
    }


}