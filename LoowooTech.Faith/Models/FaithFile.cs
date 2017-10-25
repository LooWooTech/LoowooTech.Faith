using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    [Table("Faith_files")]
    public class FaithFile
    {
        public FaithFile()
        {
            Time = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public FileType Type { get; set; }
        /// <summary>
        /// 对应的是诚信行为还是违法用地行为ID
        /// </summary>
        public int SID { get; set; }

        public bool Delete { get; set; }

        public string Description { get; set; }

    }

    public enum FileType
    {
        [Description("诚信行为")]
        Conduct,
        [Description("违法用地")]
        LandRecord
    }
}