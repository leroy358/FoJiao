using FoJiao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoJiao.Areas.Admin.Controllers
{
    public class AudioController : Controller
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
                        var audioCollections = db.AudioCollections.Where(item => item.Id == iSearch);
                        return View(audioCollections);
                    }
                    else
                    {
                        var audioCollections = db.AudioCollections.Where(item => item.Category==category && item.Title.Contains(searchStr));
                        int count = audioCollections.Count();
                        audioCollections = audioCollections.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                        InitPage(category, pageIndex, count, searchStr);
                        return View(audioCollections);
                    }
                }
                else
                {
                    var audioCollections = db.AudioCollections.Where(item => item.Category == category).OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    int count = db.AudioCollections.Where(item => item.Category == category).Count();
                    InitPage(category, pageIndex, count, searchStr);
                    return View(audioCollections);
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
                AudioCollections audioCollections = new AudioCollections();
                audioCollections.Category = category;
                return View("Edit", audioCollections);
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
                AudioCollections audioCollections = db.AudioCollections.Find(id);
                return View("Edit", audioCollections);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEdit(AudioCollections audioCollections, bool IsCreate)
        {
            if (Session["admin"] != null)
            {
                if (IsCreate)
                {
                    audioCollections.UpdateTime = DateTime.Now;
                    db.Entry(audioCollections).State = EntityState.Modified;
                }
                else
                {
                    audioCollections.CreateTime = DateTime.Now;
                    audioCollections.UpdateTime = DateTime.Now;
                    db.AudioCollections.Add(audioCollections);
                }
                db.SaveChanges();
                return RedirectToAction("List", new { category = audioCollections.Category });
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
                var audioCollections = db.AudioCollections.Find(id);
                audioCollections.IsPublished = true;
                audioCollections.UpdateTime = DateTime.Now;
                db.Entry(audioCollections).State = EntityState.Modified;
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
                var audioCollections = db.AudioCollections.Find(id);
                audioCollections.IsPublished = false;
                audioCollections.UpdateTime = DateTime.Now;
                db.Entry(audioCollections).State = EntityState.Modified;
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
                var audioCollections = db.AudioCollections.Find(id);
                db.AudioCollections.Remove(audioCollections);
                db.SaveChanges();
                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        public ActionResult CellList(int collectionId, string searchStr, int pageIndex = 1)
        {
            if (Session["admin"] != null)
            {
                ViewBag.searchStr = searchStr;

                AudioCollections audioCollection = db.AudioCollections.Find(collectionId);
                ViewBag.CollectionName = audioCollection.Title;
                ViewBag.Category = audioCollection.Category;
                ViewBag.Category = audioCollection.Category;
                if (!string.IsNullOrEmpty(searchStr))
                {
                    var audios = db.Audios.Where(item => item.AudioCollectionId == collectionId && item.Title.Contains(searchStr));
                    int count = audios.Count();
                    audios = audios.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    CellInitPage(collectionId, pageIndex, count);
                    return View(audios);
                }
                else
                {
                    var audios = db.Audios.Where(item => item.AudioCollectionId == collectionId).OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    int count = db.Audios.Where(item => item.AudioCollectionId == collectionId).Count();
                    CellInitPage(collectionId, pageIndex, count);
                    return View(audios);
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
                AudioCollections audioCollection = db.AudioCollections.Find(collectionId);
                ViewBag.CollectionName = audioCollection.Title;
                ViewBag.Category = audioCollection.Category;
                ViewBag.IsCreate = false;
                Audios audio = new Audios();
                audio.AudioCollectionId = collectionId;
                return View("CellEdit", audio);
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
                Audios audio = db.Audios.Find(id);
                AudioCollections audioCollection = db.AudioCollections.Find(audio.AudioCollectionId);
                ViewBag.CollectionName = audioCollection.Title;
                ViewBag.Category = audioCollection.Category;
                return View("CellEdit", audio);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveCellEdit(Audios audio, bool IsCreate)
        {
            if (Session["admin"] != null)
            {
                if (IsCreate)
                {
                    audio.UpdateTime = DateTime.Now;
                    db.Entry(audio).State = EntityState.Modified;
                }
                else
                {
                    audio.CreateTime = DateTime.Now;
                    audio.UpdateTime = DateTime.Now;
                    db.Audios.Add(audio);
                }
                db.SaveChanges();
                return RedirectToAction("CellList", new { collectionId = audio.AudioCollectionId });
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
                var audio = db.Audios.Find(id);
                db.Audios.Remove(audio);
                db.SaveChanges();
                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        private void InitPage(int category,int pageIndex, int count, string searchStr)
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
        private void CellInitPage(int collectionId, int pageIndex, int count)
        {
            int pageCount = (count % pageSize == 0) ? count / pageSize : count / pageSize + 1;
            if (pageCount == 0)
            {
                pageCount = 1;
            }
            string perPage = Url.Action("CellList", new { collectionId, pageIndex = (pageIndex < 2) ? 1 : pageIndex - 1 });
            string nextPage = Url.Action("CellList", new { collectionId, pageIndex = (pageIndex == pageCount) ? pageCount : (pageIndex + 1) });
            string lastPage = Url.Action("CellList", new { collectionId, pageIndex = pageCount });
            string firstPage = Url.Action("CellList", new { collectionId, pageIndex = 1 });
            string pageX = Url.Action("CellList", new { collectionId, pageIndex });
            ViewBag.perPage = perPage;
            ViewBag.nextPage = nextPage;
            ViewBag.pageCount = pageCount;
            ViewBag.pageX = pageX.Substring(pageX.Length - 1, 1);
            ViewBag.lastPage = lastPage;
            ViewBag.firstPage = firstPage;
        }
        public ActionResult Preview(int id)
        {
            AudioCollections article = db.AudioCollections.Find(id);
            return View(article);
        }
        public ActionResult PreviewAudio(int id)
        {
            Audios audio = db.Audios.Find(id);
            return View(audio);
        }
        public ActionResult MobilePreview(int id)
        {
            ViewBag.audioId = id;
            return View();
        }
        public ActionResult AudioPreview(int id)
        {
            Audios audio = db.Audios.Find(id);
            return View(audio);
        }
    }
}