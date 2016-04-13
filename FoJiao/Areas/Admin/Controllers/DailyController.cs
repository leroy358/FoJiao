using FoJiao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoJiao.Areas.Admin.Controllers
{
    public class DailyController : Controller
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
                    var word = db.DailyWords.Where(item => item.Title.Contains(searchStr));
                    int count = word.Count();
                    word = word.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    InitPage(pageIndex, count, searchStr);

                    return View(word);

                }
                else
                {
                    int count = db.DailyWords.Count();
                    var word = db.DailyWords.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    InitPage(pageIndex, count, searchStr);
                    return View(word);
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
                return View("Edit", new DailyWords());
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
                DailyWords word = db.DailyWords.Find(id);
                return View("Edit", word);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEdit(DailyWords word, bool IsCreate)
        {
            if (Session["admin"] != null)
            {
                if (IsCreate)
                {
                    word.UpdateTime = DateTime.Now;
                    db.Entry(word).State = EntityState.Modified;
                }
                else
                {
                    word.CreateTime = DateTime.Now;
                    word.UpdateTime = DateTime.Now;
                    db.DailyWords.Add(word);
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
                var word = db.DailyWords.Find(id);
                db.DailyWords.Remove(word);
                db.SaveChanges();
                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        private void InitPage(int pageIndex, int count, string searchStr)
        {
            int pageCount = (count % pageSize == 0) ? count / pageSize : count / pageSize + 1;
            if (pageCount == 0)
            {
                pageCount = 1;
            }
            string perPage = Url.Action("List", new { searchStr, pageIndex = (pageIndex < 2) ? 1 : pageIndex - 1 });
            string nextPage = Url.Action("List", new { searchStr, pageIndex = (pageIndex == pageCount) ? pageCount : (pageIndex + 1) });
            string lastPage = Url.Action("List", new { searchStr, pageIndex = pageCount });
            string firstPage = Url.Action("List", new { searchStr, pageIndex = 1 });
            string pageX = Url.Action("List", new { searchStr, pageIndex });
            ViewBag.perPage = perPage;
            ViewBag.nextPage = nextPage;
            ViewBag.pageCount = pageCount;
            ViewBag.pageX = pageX.Substring(pageX.Length - 1, 1);
            ViewBag.lastPage = lastPage;
            ViewBag.firstPage = firstPage;
        }
    }
}