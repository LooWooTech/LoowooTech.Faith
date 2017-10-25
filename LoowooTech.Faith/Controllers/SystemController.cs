using LoowooTech.Faith.Common;
using LoowooTech.Faith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoowooTech.Faith.Controllers
{
    [UserAuthorize]
    public class SystemController : ControllerBase
    {

        public ActionResult Index(SystemEnum systemenum=SystemEnum.Standard)
        {
            ViewBag.Enum = systemenum;
            return View();
        }

        public ActionResult StandardList()
        {
            ViewBag.Dict = Core.StandardManager.GetList();
            return View();
        }

        public ActionResult CreateStandard(int id=0)
        {
            var model = Core.StandardManager.Get(id);
            ViewBag.Model = model;
            return View();
        }

        [HttpPost]
        public ActionResult CreateStandard(Standard standard)
        {
            if (standard == null)
            {
                return ErrorJsonResult("未获取诚信类型信息");
            }
        
            if (standard.ID > 0)
            {
                if (!Core.StandardManager.Edit(standard))
                {
                    return ErrorJsonResult("更新诚信类型失败，未找到更新的诚信类型信息");
                }
            }
            else
            {
                if (Core.StandardManager.Exist(standard.Name, standard.Credit))
                {
                    return ErrorJsonResult(string.Format("系统中已存在名称：{0}；环节为：{1}的类型记录", standard.Name, standard.Credit.GetDescription()));
                }
                var id = Core.StandardManager.Save(standard);
                if (id <= 0)
                {
                    return ErrorJsonResult("保存类型失败");
                }
            }
            return SuccessJsonResult();
        }

        public ActionResult DeleteStandard(int id)
        {
            if (!Core.StandardManager.Delete(id))
            {
                return ErrorJsonResult("删除失败，未找到诚信行为信息");
            }
            return SuccessJsonResult();
        }

        public ActionResult UserList()
        {
            var list = Core.UserManager.GetList(City.ID);
            ViewBag.List = list;
            return View();
        }

        public ActionResult Approve(int id)
        {
            if (!Core.UserManager.Approve(id))
            {
                return ErrorJsonResult("授权用户登录失败，未找到授权用户信息");
            }
            return SuccessJsonResult();
        }

        public ActionResult ChangeRole(int id)
        {
            var user = Core.UserManager.Get(id);
            ViewBag.User = user;
            return View();
        }

        [HttpPost]
        public ActionResult ChangeRole(int id,UserRole role)
        {
            if (!Core.UserManager.ChangeRole(id, role))
            {
                return ErrorJsonResult("更改角色失败，未找到用户信息");
            }
            return SuccessJsonResult();
        }

        public ActionResult DeleteUser(int id)
        {
            if (!Core.UserManager.Delete(id))
            {
                return ErrorJsonResult("删除用户失败，未找到相关用户");
            }
            return SuccessJsonResult();
        }


        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveUser(User user,string copyPassword)
        {
            if (user == null)
            {
                return ErrorJsonResult("未获取用户相关信息");
            }
            if (string.IsNullOrEmpty(copyPassword))
            {
                return ErrorJsonResult("确认密码不能为空");
            }
            if (user.Password != copyPassword)
            {
                return ErrorJsonResult("输入的两次密码不一致，请核对密码信息");
            }
            if (Core.UserManager.Exist(user.Name,City.ID))
            {
                return ErrorJsonResult("系统中已经存在相同的用户名");
            }
            user.CityID = City.ID;
            var id = Core.UserManager.Save(user);
            if (id <= 0)
            {
                return ErrorJsonResult("保存用户失败");
            }


            return SuccessJsonResult();
        }
        
        

    }
}