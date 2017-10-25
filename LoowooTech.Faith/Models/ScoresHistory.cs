using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    /// <summary>
    /// 评分历史表
    /// </summary>
    [Table("scores_history")]
    public class ScoresHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public SystemData SystemData { get; set; }
        public int ELID { get; set; }
        public int? Times { get; set; }
        public int? ScoreValue { get; set; }
        public int? Record { get; set; }
        public int GradeID { get; set; }
        public virtual GradeHistory Grade { get; set; }
        public virtual List<ScoreText> ScoreTexts { get; set; }

        [NotMapped]
        public int Average
        {
            get
            {
                if (ScoreValue.HasValue && Times.HasValue)
                {
                    return (int)((double)ScoreValue.Value / Times.Value + 0.5);
                }
                return 0;
            }
        }
        [NotMapped]
        public int DeDuck
        {
            get
            {
                if (Record.HasValue)
                {
                    return Average + Record.Value;
                }
                return Average;
            }
        }
        [NotMapped]
        public GradeDegree Degree
        {
            get
            {
                if (DeDuck == 0)
                {
                    return GradeDegree.A;
                }
                else if (DeDuck <= 20 && DeDuck > 0)
                {
                    return GradeDegree.B;
                }
                else if (DeDuck > 20 && DeDuck < 40)
                {
                    return GradeDegree.C;
                }
                return GradeDegree.D;
            }
        }
    }
}