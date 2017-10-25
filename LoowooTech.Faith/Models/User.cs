using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        
        public UserRole Role { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string TelPhone { get; set; }

        public bool Approve { get; set; }
        [NotMapped]
        public string AccessToken { get; set; }
        public int CityID { get; set; }
    }

    public enum UserRole
    {
        [Description("游客")]
        Guest,
        [Description("普通用户")]
        Common,
        [Description("审核")]
        Manager,
        [Description("管理员")]
        Administrator
    }

    public enum SystemEnum
    {
        Standard,
        User
    }
}