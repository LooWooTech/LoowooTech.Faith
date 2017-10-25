using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    [Table("scores")]
    public class Score
    {
        //[Key]
        public int ID { get; set; }
        public string ELID { get; set; }
        public string ELName { get; set; }
        //public int SystemData { get; set; }
        public SystemData SystemData { get; set; }
        public int? Times { get; set; }
        public int? ScoreValue { get; set; }
        [NotMapped]
        public int Average
        {
            get
            {
                if (ScoreValue.HasValue&&Times.HasValue)
                {
                    return (int)((double)ScoreValue.Value / Times.Value+0.5);
                }
                return 0;
            }
        }
        public int? Record { get; set; }
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
        [NotMapped]
        public GradeDegree Degree
        {
            get
            {
                if (DeDuck == 0)
                {
                    return GradeDegree.A;
                }else if (DeDuck <= 20 && DeDuck > 0)
                {
                    return GradeDegree.B;
                }else if (DeDuck > 20 && DeDuck < 40)
                {
                    return GradeDegree.C;
                }
                return GradeDegree.D;
            }
        }
    }

    public class BaseScore
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public int? Times { get; set; }
        public int? ScoreValue { get; set; }
        public int? Record { get; set; }
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
                return  Average;
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
        public int CityID { get; set; }
 
    }
}