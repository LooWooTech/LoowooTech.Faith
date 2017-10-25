using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    [Table("flowstep")]
    public class FlowStep
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public int PreId { get; set; }
        [ForeignKey("PreId")]
        public virtual FlowStep Prev { get; set; }
        public int NextId { get; set; }
        [ForeignKey("NextId")]
        public virtual FlowStep Next { get; set; }
    }
}