using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    public class Message
    {
        public Message()
        {
            CreateTime = DateTime.Now;
        }
        public int ID { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 对应的企业或者自然人
        /// </summary>
        public int ELID { get; set; }
        public SystemData SystemData { get; set; }
        /// <summary>
        /// 原始级别
        /// </summary>
        public GradeDegree Old { get; set; }
        /// <summary>
        /// 现如今级别
        /// </summary>
        public GradeDegree New { get; set; }
        /// <summary>
        /// 消息发送对象
        /// </summary>
        public int UserID { get; set; }
        public virtual User User { get; set; }
        /// <summary>
        /// 是否阅读
        /// </summary>
        public bool HasRead { get; set; }

    }
}