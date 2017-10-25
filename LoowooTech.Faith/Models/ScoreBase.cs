using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    public class ScoreBase
    {
        /// <summary>
        /// 企业名称/自然人姓名
        /// </summary>
        [Description("企业名称/自然人名称")]
        public string Name { get; set; }
        public int Good { get; set; }
        public int Less { get; set; }
        public int Bad { get; set; }
        [NotMapped]
        public string Description
        {
            get
            {
                return string.Format("诚信：{0}；失信：{1}；严重失信：{2}", Good, Less, Bad);
            }
        }
    }

    public enum BaseState
    {
        [Description("等待审核")]
        Checking,
        [Description("审核不通过")]
        DisAgree,
        [Description("审核通过")]
        Argee,
        [Description("解除")]
        Relieve
    }
}