using FoJiao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoJiao.Areas.Admin.Controllers
{
    public class ConfigController : Controller
    {
        private FoJiaoDbContext db = new FoJiaoDbContext();
        public ActionResult Edit()
        {
            if (Session["admin"] != null)
            {
                Admins admin = db.Admins.FirstOrDefault();
                return View(admin);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        public ActionResult SavEdit(string UserName, string Password, string newPassword, string confirmPassword)
        {
            if (Session["admin"] != null)
            {
                if (!string.IsNullOrEmpty(newPassword.Trim()) && !string.IsNullOrEmpty(confirmPassword.Trim()))
                {
                    if (newPassword == confirmPassword)
                    {
                        if (!string.IsNullOrEmpty(Password))
                        {
                            Admins admin = db.Admins.Where(item => item.AdminName == UserName && item.Password == Password).FirstOrDefault();
                            if (admin != null)
                            {
                                admin.Password = confirmPassword;
                                db.Entry(admin).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                return Content("<script>alert('初始密码输入不正确，请重新输入！');window.location.href='Edit';</script>");
                            }
                        }
                        else
                        {
                            return Content("<script>alert('初始密码不能为空，请重新输入！');window.location.href='Edit';</script>");
                        }

                    }
                    else
                    {
                        return Content("<script>alert('两次密码输入不一致，请重新输入！');window.location.href='Edit';</script>");
                    }
                }
                else
                {
                    return Content("<script>alert('密码不能为空，请重新输入！');window.location.href='Edit';</script>");
                }
                return Content("<script>alert('密码修改成功！');window.location.href='Edit';</script>");
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
    }
}