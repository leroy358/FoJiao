using FoJiao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoJiao.Areas.Admin.Controllers
{
    public class ArticleController : Controller
    {
        private FoJiaoDbContext db = new FoJiaoDbContext();

        int pageSize = 20;

        public ActionResult List(int category, string searchStr, int pageIndex = 1)
        {
            if (Session["admin"] != null)
            {
                ViewBag.searchStr = searchStr;
                if (!string.IsNullOrEmpty(searchStr))
                {
                    int iSearch = 0;
                    int.TryParse(searchStr, out iSearch);
                    if (iSearch != 0)
                    {
                        ViewBag.pageX = 1;
                        ViewBag.pageCount = 1;
                        var users = db.Articles.Where(item => item.Category == category && item.Id == iSearch);
                        return View(users);
                    }
                    else
                    {
                        var users = db.Articles.Where(item => item.Category == category && item.Title.Contains(searchStr));
                        int count = users.Count();
                        users = users.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                        InitPage(category, pageIndex, count, searchStr);
                        return View(users);
                    }
                }
                else
                {
                    var users = db.Articles.Where(item => item.Category == category).OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    int count = db.Articles.Where(item => item.Category == category).Count();
                    InitPage(category, pageIndex, count, searchStr);
                    return View(users);
                }
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        public ActionResult Create(int category)
        {

            if (Session["admin"] != null)
            {
                ViewBag.IsCreate = false;
                Articles article = new Articles();
                article.Category = category;
                return View("Edit", article);
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
                Articles article = db.Articles.Find(id);
                return View("Edit", article);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEdit(Articles article, bool IsCreate)
        {
            if (Session["admin"] != null)
            {
                if (IsCreate)
                {
                    article.UpdateTime = DateTime.Now;
                    db.Entry(article).State = EntityState.Modified;
                }
                else
                {
                    article.CreateTime = DateTime.Now;
                    article.UpdateTime = DateTime.Now;
                    db.Articles.Add(article);
                }
                db.SaveChanges();
                return RedirectToAction("List", new { category = article.Category });
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }                
        }
        public ActionResult Publish(int id, string returnURL)
        {
            if (Session["admin"] != null)
            {
                var article = db.Articles.Find(id);
                article.IsPublished = true;
                article.UpdateTime = DateTime.Now;
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        public ActionResult UnPublish(int id, string returnURL)
        {
            if (Session["admin"] != null)
            {
                var article = db.Articles.Find(id);
                article.IsPublished = false;
                article.UpdateTime = DateTime.Now;
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(returnURL);
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
                var article = db.Articles.Find(id);
                db.Articles.Remove(article);
                db.SaveChanges();
                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        private void InitPage(int category, int pageIndex, int count, string searchStr)
        {
            int pageCount = (count % pageSize == 0) ? count / pageSize : count / pageSize + 1;
            if (pageCount == 0)
            {
                pageCount = 1;
            }
            string perPage = Url.Action("List", new { category, searchStr, pageIndex = (pageIndex < 2) ? 1 : pageIndex - 1 });
            string nextPage = Url.Action("List", new { category, searchStr, pageIndex = (pageIndex == pageCount) ? pageCount : (pageIndex + 1) });
            string lastPage = Url.Action("List", new { category, searchStr, pageIndex = pageCount });
            string firstPage = Url.Action("List", new { category, searchStr, pageIndex = 1 });
            string pageX = Url.Action("List", new { category, searchStr, pageIndex });
            ViewBag.perPage = perPage;
            ViewBag.nextPage = nextPage;
            ViewBag.pageCount = pageCount;
            ViewBag.pageX = pageX.Substring(pageX.Length - 1, 1);
            ViewBag.lastPage = lastPage;
            ViewBag.firstPage = firstPage;
        }
        public ActionResult Preview(int id)
        {
            Articles article = db.Articles.Find(id);
            return View(article);
        }
        public ActionResult MobilePreview(int id)
        {
            ViewBag.ArticleId = id;
            return View();
        }
    }
}