using FoJiao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoJiao.Areas.Admin.Controllers
{
    public class GradeController : Controller
    {
        private FoJiaoDbContext db = new FoJiaoDbContext();

        int pageSize = 20;

        public ActionResult List(string searchStr, int pageIndex = 1)
        {
            if (Session["admin"] != null)
            {
                ViewBag.searchStr = searchStr;
                if (!string.IsNullOrEmpty(searchStr))
                {
                    var grade = db.Grades.Where(item => item.Title.Contains(searchStr)).OrderByDescending(item => item.Id);

                    return View(grade);

                }
                else
                {
                    var grade = db.Grades.OrderByDescending(item => item.Id);
                    return View(grade);
                }
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        public ActionResult Create()
        {

            if (Session["admin"] != null)
            {
                ViewBag.IsCreate = false;
                return View("Edit", new Grades());
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }

        }
        public ActionResult Edit(int id)
        {
            if (Session["admin"] != null)
            {
                ViewBag.IsCreate = true;
                Grades grade = db.Grades.Find(id);
                return View("Edit", grade);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEdit(Grades grade, bool IsCreate)
        {
            if (Session["admin"] != null)
            {
                if (IsCreate)
                {
                    grade.UpdateTime = DateTime.Now;
                    db.Entry(grade).State = EntityState.Modified;
                }
                else
                {
                    grade.CreateTime = DateTime.Now;
                    grade.UpdateTime = DateTime.Now;
                    db.Grades.Add(grade);
                }
                db.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        public ActionResult Delete(int id, string returnURL)
        {
            if (Session["admin"] != null)
            {
                var grade = db.Grades.Find(id);
                db.Grades.Remove(grade);
                db.SaveChanges();
                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
    }
}