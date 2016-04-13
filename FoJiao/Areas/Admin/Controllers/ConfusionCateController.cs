using FoJiao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoJiao.Areas.Admin.Controllers
{
    public class ConfusionCateController : Controller
    {
        private FoJiaoDbContext db = new FoJiaoDbContext();

        int pageSize = 20;

        public ActionResult List(string searchStr)
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
                        var confusionCate = db.ConfusionCate.Where(item => item.Id == iSearch);
                        return View(confusionCate);
                    }
                    else
                    {
                        var audioCollections = db.ConfusionCate.Where(item => item.Title.Contains(searchStr)).OrderByDescending(item => item.Id);
                        //int count = audioCollections.Count();
                        //audioCollections = audioCollections.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                        //InitPage( pageIndex, count, searchStr);
                        return View(audioCollections);
                    }
                }
                else
                {
                    var audioCollections = db.ConfusionCate.OrderByDescending(item => item.Id);
                    //int count = db.ConfusionCate.Count();
                    //InitPage( pageIndex, count, searchStr);
                    return View(audioCollections);
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
                return View("Edit", new ConfusionCate());
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
                ConfusionCate confusionCate = db.ConfusionCate.Find(id);
                return View("Edit", confusionCate);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEdit(ConfusionCate confusionCate, bool IsCreate)
        {
            if (Session["admin"] != null)
            {
                if (IsCreate)
                {
                    confusionCate.UpdateTime = DateTime.Now;
                    db.Entry(confusionCate).State = EntityState.Modified;
                }
                else
                {
                    confusionCate.CreateTime = DateTime.Now;
                    confusionCate.UpdateTime = DateTime.Now;
                    db.ConfusionCate.Add(confusionCate);
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
                var confusionCate = db.ConfusionCate.Find(id);
                db.ConfusionCate.Remove(confusionCate);
                db.SaveChanges();
                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        public ActionResult CellList(int collectionId, string searchStr)
        {
            if (Session["admin"] != null)
            {
                ViewBag.searchStr = searchStr;

                ConfusionCate confusionCate = db.ConfusionCate.Find(collectionId);
                ViewBag.CollectionName = confusionCate.Title;
                if (!string.IsNullOrEmpty(searchStr))
                {
                    var secCate = db.ConfusionSecCate.Where(item => item.ConfusionCateId == collectionId && item.Title.Contains(searchStr)).OrderByDescending(item => item.Id);
                    return View(secCate);
                }
                else
                {
                    var secCate = db.ConfusionSecCate.Where(item => item.ConfusionCateId == collectionId).OrderByDescending(item => item.Id);
                    return View(secCate);
                }
            }

            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        public ActionResult CellCreate(int collectionId)
        {

            if (Session["admin"] != null)
            {
                ConfusionCate confusionCate = db.ConfusionCate.Find(collectionId);
                ViewBag.CollectionName = confusionCate.Title;
                ViewBag.IsCreate = false;
                ConfusionSecCate secCat = new ConfusionSecCate();
                secCat.ConfusionCateId = collectionId;
                return View("CellEdit", secCat);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }

        }
        public ActionResult CellEdit(int id)
        {
            if (Session["admin"] != null)
            {
                ViewBag.IsCreate = true;
                ConfusionSecCate secCate = db.ConfusionSecCate.Find(id);
                ConfusionCate confusionCate = db.ConfusionCate.Find(secCate.ConfusionCateId);
                ViewBag.CollectionName = confusionCate.Title;
                return View("CellEdit", secCate);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveCellEdit(ConfusionSecCate secCate, bool IsCreate)
        {
            if (Session["admin"] != null)
            {
                if (IsCreate)
                {
                    secCate.UpdateTime = DateTime.Now;
                    db.Entry(secCate).State = EntityState.Modified;
                }
                else
                {
                    secCate.CreateTime = DateTime.Now;
                    secCate.UpdateTime = DateTime.Now;
                    db.ConfusionSecCate.Add(secCate);
                }
                db.SaveChanges();
                return RedirectToAction("CellList", new { collectionId = secCate.ConfusionCateId });
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        public ActionResult CellDelete(int id, string returnURL)
        {
            if (Session["admin"] != null)
            {
                ConfusionSecCate secCate = db.ConfusionSecCate.Find(id);
                db.ConfusionSecCate.Remove(secCate);
                db.SaveChanges();
                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }

        public ActionResult ConfusionList(int cateId,string searchStr, int pageIndex = 1)
        {
            if (Session["admin"] != null)
            {
                ViewBag.searchStr = searchStr;
                ViewBag.LayId = cateId;
                ConfusionSecCate secCate = db.ConfusionSecCate.Find(cateId);
                ViewBag.LayName = secCate.Title;
                ConfusionCate cate = db.ConfusionCate.Find(secCate.ConfusionCateId);
                ViewBag.BuildName = cate.Title;
                ViewBag.BuildId = cate.Id;
                if (!string.IsNullOrEmpty(searchStr))
                {
                    int iSearch = 0;
                    int.TryParse(searchStr, out iSearch);
                    if (iSearch != 0)
                    {
                        ViewBag.pageX = 1;
                        ViewBag.pageCount = 1;
                        var users = db.Confusions.Where(item => item.ConfusionSecId == cateId && item.Id == iSearch);
                        return View(users);
                    }
                    else
                    {
                        var confusions = db.Confusions.Where(item => item.ConfusionSecId == cateId && item.Title.Contains(searchStr));
                        int count = confusions.Count();
                        confusions = confusions.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                        InitPage(cateId, pageIndex, count, searchStr);
                        return View(confusions);
                    }
                }
                else
                {
                    var confusions = db.Confusions.Where(item => item.ConfusionSecId == cateId).OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize); ;
                    int count = db.Confusions.Where(item => item.ConfusionSecId == cateId).Count();
                    InitPage(cateId, pageIndex, count, searchStr);
                    return View(confusions);
                }
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }

        }
        public ActionResult CreateConfusion(int id)
        {
            if (Session["admin"] != null)
            {
                ViewBag.IsCreate = false;
                Confusions confusion = new Confusions();
                confusion.ConfusionSecId = id;
                ConfusionSecCate secCate = db.ConfusionSecCate.Find(id);
                ViewBag.LayName = secCate.Title;
                ViewBag.LayId = id;
                ConfusionCate cate = db.ConfusionCate.Find(secCate.ConfusionCateId);
                ViewBag.BuildId = cate.Id;
                ViewBag.BuildName = cate.Title;
                return View("EditConfusion", confusion);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        public ActionResult EditConfusion(int id)
        {
            if (Session["admin"] != null)
            {
                Confusions confusion = db.Confusions.Find(id);
                ConfusionSecCate secCate = db.ConfusionSecCate.Find(confusion.ConfusionSecId);
                ViewBag.LayId = secCate.Id;
                ViewBag.LayName = secCate.Title;
                ConfusionCate cate = db.ConfusionCate.Find(secCate.ConfusionCateId);
                ViewBag.BuildId = cate.Id;
                ViewBag.BuildName = cate.Title;
                ViewBag.IsCreate = true;
                return View(confusion);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEditConfusion(Confusions confusion, bool IsCreate)
        {
            if (Session["admin"] != null)
            {
                if (IsCreate)
                {
                    confusion.UpdateTime = DateTime.Now;
                    db.Entry(confusion).State = EntityState.Modified;
                }
                else
                {
                    confusion.CreateTime = DateTime.Now;
                    confusion.UpdateTime = DateTime.Now;
                    db.Confusions.Add(confusion);
                }
                db.SaveChanges();
                return RedirectToAction("ConfusionList", new { cateId = confusion.ConfusionSecId });
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }

        }
        public ActionResult DeleteConfusion(int id, string returnURL)
        {
            if (Session["admin"] != null)
            {
                Confusions confusion = db.Confusions.Find(id);
                db.Confusions.Remove(confusion);
                db.SaveChanges();

                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        private void InitPage(int cateId, int pageIndex, int count, string searchStr)
        {
            int pageCount = (count % pageSize == 0) ? count / pageSize : count / pageSize + 1;
            if (pageCount == 0)
            {
                pageCount = 1;
            }
            string perPage = Url.Action("ConfusionList", new { cateId, searchStr, pageIndex = (pageIndex < 2) ? 1 : pageIndex - 1 });
            string nextPage = Url.Action("ConfusionList", new { cateId, searchStr, pageIndex = (pageIndex == pageCount) ? pageCount : (pageIndex + 1) });
            string lastPage = Url.Action("ConfusionList", new { cateId, searchStr, pageIndex = pageCount });
            string firstPage = Url.Action("ConfusionList", new { cateId, searchStr, pageIndex = 1 });
            string pageX = Url.Action("ConfusionList", new { cateId, searchStr, pageIndex });
            ViewBag.perPage = perPage;
            ViewBag.nextPage = nextPage;
            ViewBag.pageCount = pageCount;
            ViewBag.pageX = pageX.Substring(pageX.Length - 1, 1);
            ViewBag.lastPage = lastPage;
            ViewBag.firstPage = firstPage;
        }

        public ActionResult Preview(int id)
        {
            Confusions confusion = db.Confusions.Find(id);
            return View(confusion);
        }
    }
}