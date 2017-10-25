using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    [Table("city")]
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        [ForeignKey("ParentID")]
        public virtual City Parents { get; set; }
        [NotMapped]
        public string DisplayName
        {
            get
            {
                if (Parents != null)
                {
                    return Parents.DisplayName + "-" + Name;
                }
                return Name;
            }
        }
    }
}