using FoJiao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoJiao.Areas.Admin.Controllers
{
    public class ConfusionController : Controller
    {
        private FoJiaoDbContext db = new FoJiaoDbContext();

        int pageSize = 20;

        public ActionResult List(int state,int delete, int pageIndex = 1)
        {
            if (Session["admin"] != null)
            {
                if (state != 2 && delete == 0)
                {
                    var users = db.ConfusionGuest.Where(item => item.StateId == state && !item.IsDeleted).OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    int count = db.ConfusionGuest.Where(item => item.StateId == state && !item.IsDeleted).Count();
                    InitPage(state,delete, pageIndex, count);
                    return View(users);
                }
                else if(state != 2 && delete == 1)
                {
                    var users = db.ConfusionGuest.Where(item => item.StateId == state && item.IsDeleted).OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    int count = db.ConfusionGuest.Where(item => item.StateId == state && item.IsDeleted).Count();
                    InitPage(state,delete, pageIndex, count);
                    return View(users);
                }
                else
                {
                    var users = db.ConfusionGuest.Where(item => item.IsDeleted).OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    int count = db.ConfusionGuest.Where(item => item.IsDeleted).Count();
                    InitPage(state,delete, pageIndex, count);
                    return View(users);
                }

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
                ConfusionGuest confusion = db.ConfusionGuest.Find(id);
                return View("Edit", confusion);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEdit(ConfusionGuest confusion,int StateId)
        {
            if (Session["admin"] != null)
            {
                int stateId = StateId;
                confusion.UpdateTime = DateTime.Now;
                confusion.StateId = 1;
                db.Entry(confusion).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("List", new { state = confusion.IsDeleted?2:stateId, delete = confusion.IsDeleted ? 1 : 0 });
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
                var confusion = db.ConfusionGuest.Find(id);
                confusion.IsDeleted = true;
                db.Entry(confusion).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        public ActionResult UnDelete(int id, string returnURL)
        {
            if (Session["admin"] != null)
            {
                var confusion = db.ConfusionGuest.Find(id);
                confusion.IsDeleted = false;
                db.Entry(confusion).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        private void InitPage(int state, int delete, int pageIndex, int count)
        {
            int pageCount = (count % pageSize == 0) ? count / pageSize : count / pageSize + 1;
            if (pageCount == 0)
            {
                pageCount = 1;
            }
            string perPage = Url.Action("List", new { state, delete, pageIndex = (pageIndex < 2) ? 1 : pageIndex - 1 });
            string nextPage = Url.Action("List", new { state, delete, pageIndex = (pageIndex == pageCount) ? pageCount : (pageIndex + 1) });
            string lastPage = Url.Action("List", new { state, delete, pageIndex = pageCount });
            string firstPage = Url.Action("List", new { state, delete, pageIndex = 1 });
            string pageX = Url.Action("List", new { state, delete, pageIndex });
            ViewBag.perPage = perPage;
            ViewBag.nextPage = nextPage;
            ViewBag.pageCount = pageCount;
            ViewBag.pageX = pageX.Substring(pageX.Length - 1, 1);
            ViewBag.lastPage = lastPage;
            ViewBag.firstPage = firstPage;
        }
        public ActionResult Preview(int id)
        {
            ConfusionGuest confusion = db.ConfusionGuest.Find(id);
            return View(confusion);
        }
    }
}