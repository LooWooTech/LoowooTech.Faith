using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.Faith.Managers
{
    public class UserManager:ManagerBase
    {
        /// <summary>
        /// 作用：登录系统  
        /// 作者：汪建龙
        /// 编写时间：2017年3月5日13:36:25
        /// </summary>
        /// <param name="name">登录名</param>
        /// <param name="password">明文密码 无需加密</param>
        /// <returns></returns>
        public User Login(string name,string password,int cityID)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            password = password.MD5();
          
            return Db.Users.FirstOrDefault(e => e.Name.ToLower() == name.ToLower() && e.Password.ToLower() == password.ToLower()&&e.CityID==cityID);
        }
        /// <summary>
        /// 作用：验证系统中是否已存在  登录名不区分大小写
        /// 作者：汪建龙
        /// 编写时间：2017年2月9日11:25:28
        /// </summary>
        /// <param name="name">系统登录名</param>
        /// <returns></returns>
        public bool Exist(string name,int cityID)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            var user = Db.Users.FirstOrDefault(e => e.Name.ToLower() == name.ToLower()&&e.CityID==cityID);
            return user != null;
        }
        /// <summary>
        /// 作用：注册用户
        /// 作者：汪建龙
        /// 编写时间：2017年3月5日13:45:43
        /// </summary>
        /// <param name="name">系统登录名</param>
        /// <param name="displayName">系统名称</param>
        /// <param name="password">密码  明文无需加密</param>
        public void Register(string name, string displayName, string password)
        {
            Db.Users.Add(new User()
            {
                Name = name,
                UserName = displayName,
                Password = password.MD5()
            });
            Db.SaveChanges();
        }
        /// <summary>
        /// 作用：获取所有用户列表
        /// 作者：汪建龙
        /// 编写时间：2017年3月7日13:29:42
        /// </summary>
        /// <returns></returns>
        public List<User> GetList(int cityID)
        {
            return Db.Users.Where(e=>e.CityID==cityID).OrderByDescending(e => e.Role).ToList();
        }
        /// <summary>
        /// 作用：授权用户登录
        /// 作者：汪建龙
        /// 编写时间：2017年3月7日13:45:13
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Approve(int id)
        {
            var model = Db.Users.Find(id);
            if (model == null)
            {
                return false;
            }
            model.Approve = true;
            Db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 作用：获取用户
        /// 作者：汪建龙
        /// 编写时间：2017年3月7日13:47:29
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User Get(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            return Db.Users.Find(id);
        }
        /// <summary>
        /// 作用：更改用户角色
        /// 作者：汪建龙
        /// 编写时间：2017年3月7日13:52:16
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool ChangeRole(int id,UserRole role)
        {
            var model = Db.Users.Find(id);
            if (model == null || model.Role == UserRole.Administrator)
            {
                return false;
            }
            model.Role = role;
            Db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 作用：删除用户
        /// 作者：汪建龙
        /// 编写时间：2017年3月7日13:54:50
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var model = Db.Users.Find(id);
            if (model == null)
            {
                return false;
            }
            Db.Users.Remove(model);
            Db.SaveChanges();
            return true;
        }
        /// <summary>
        /// 作用：获取审核人列表
        /// 作者：汪建龙
        /// 编写时间：2017年3月7日14:50:03
        /// </summary>
        /// <returns></returns>
        public List<User> GetManager(int cityId)
        {
            return Db.Users.Where(e => e.Role == UserRole.Manager&&e.CityID==cityId).ToList();
        }

        /// <summary>
        /// 作用：添加用户
        /// 作者：汪建龙
        /// 编写时间：2017年4月2日19:01:37
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Save(User user)
        {
            user.Password = user.Password.MD5();
            Db.Users.Add(user);
            Db.SaveChanges();
            return user.ID;
        }

    }
}