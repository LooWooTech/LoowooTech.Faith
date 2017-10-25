using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    [Table("scores_text")]
    public class ScoreText
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }    
        public string Name { get; set; }
        public int CLID { get; set; }
        public double Scores { get; set; }
        public FileType Type { get; set; }
        public int ScoresHistoryId { get; set; }
        public virtual ScoresHistory ScoresHistory { get; set; }
    }
}