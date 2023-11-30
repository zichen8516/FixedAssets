using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FixedAssets.DataAccess;

namespace FixedAssets.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Sign(string UserName, string Password)
        {
            FixedAssetsData fixedassetsdata = new FixedAssetsData();
            var user = fixedassetsdata.UserInfos.Where(u => u.userName == UserName && u.passWord == Password).FirstOrDefault();
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(UserName, false);
                Session["user"] = true;
                return Json(new
                {
                    Success = true,
                    Massage = "登陆成功！"
                });
            }
            else
            {
                Session["user"] = false;
                return Json(new
                {
                    Success = false,
                    Massage = "用户或密码错误~"
                }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}