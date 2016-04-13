using FoJiao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoJiao.Areas.Admin.Controllers
{
    public class ConsoleController : Controller
    {
        private FoJiaoDbContext db = new FoJiaoDbContext();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            Admins admin = db.Admins.Where(item => item.AdminName == username && item.Password == password).FirstOrDefault();
            if (admin != null)
            {
                Session["admin"] = admin.AdminName;
                Session.Timeout = 2 * 60;
                return Json("Success");
            }
            else
            {
                return Json("Error");
            }
        }
        public ActionResult Main()
        {
            if (Session["admin"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        public ActionResult Exit()
        {
            Session.Remove("admin");
            return RedirectToAction("Login");
        }
    }
}