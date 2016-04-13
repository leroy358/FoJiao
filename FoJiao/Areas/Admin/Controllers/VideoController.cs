using FoJiao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoJiao.Areas.Admin.Controllers
{
    public class VideoController : Controller
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
                    int iSearch = 0;
                    int.TryParse(searchStr, out iSearch);
                    if (iSearch != 0)
                    {
                        ViewBag.pageX = 1;
                        ViewBag.pageCount = 1;
                        var videoCollections = db.VideoCollections.Where(item => item.Id == iSearch);
                        return View(videoCollections);
                    }
                    else
                    {
                        var VideoCollections = db.VideoCollections.Where(item => item.Title.Contains(searchStr));
                        int count = VideoCollections.Count();
                        VideoCollections = VideoCollections.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                        InitPage(pageIndex, count, searchStr);
                        return View(VideoCollections);
                    }
                }
                else
                {
                    var VideoCollections = db.VideoCollections.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    int count = db.VideoCollections.Count();
                    InitPage(pageIndex, count, searchStr);
                    return View(VideoCollections);
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
                return View("Edit", new VideoCollections());
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
                VideoCollections video = db.VideoCollections.Find(id);
                return View("Edit", video);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEdit(VideoCollections video, bool IsCreate)
        {
            if (Session["admin"] != null)
            {
                if (IsCreate)
                {
                    video.UpdateTime = DateTime.Now;
                    db.Entry(video).State = EntityState.Modified;
                }
                else
                {
                    video.CreateTime = DateTime.Now;
                    video.UpdateTime = DateTime.Now;
                    db.VideoCollections.Add(video);
                }
                db.SaveChanges();
                return RedirectToAction("List");
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
                var video = db.VideoCollections.Find(id);
                video.IsPublished = true;
                video.UpdateTime = DateTime.Now;
                db.Entry(video).State = EntityState.Modified;
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
                var video = db.VideoCollections.Find(id);
                video.IsPublished = false;
                video.UpdateTime = DateTime.Now;
                db.Entry(video).State = EntityState.Modified;
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
                var video = db.VideoCollections.Find(id);
                db.VideoCollections.Remove(video);
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
                VideoCollections videoCollection = db.VideoCollections.Find(collectionId);
                ViewBag.CollectionName = videoCollection.Title;
                if (!string.IsNullOrEmpty(searchStr))
                {
                    var Videos = db.Videos.Where(item => item.VideoCollectionId == collectionId && item.Title.Contains(searchStr));
                    int count = Videos.Count();
                    Videos = Videos.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    CellInitPage(collectionId, pageIndex, count);
                    return View(Videos);
                }
                else
                {
                    var Videos = db.Videos.Where(item => item.VideoCollectionId == collectionId).OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    int count = db.Videos.Where(item => item.VideoCollectionId == collectionId).Count();
                    CellInitPage(collectionId, pageIndex, count);
                    return View(Videos);
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
                VideoCollections videoCollection = db.VideoCollections.Find(collectionId);
                ViewBag.CollectionName = videoCollection.Title;
                ViewBag.IsCreate = false;
                Videos video = new Videos();
                video.VideoCollectionId = collectionId;
                return View("CellEdit", video);
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
                Videos video = db.Videos.Find(id);
                VideoCollections videoCollection = db.VideoCollections.Find(video.VideoCollectionId);
                ViewBag.CollectionName = videoCollection.Title;
                return View("CellEdit", video);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveCellEdit(Videos video, bool IsCreate)
        {
            if (Session["admin"] != null)
            {
                if (IsCreate)
                {
                    video.UpdateTime = DateTime.Now;
                    db.Entry(video).State = EntityState.Modified;
                }
                else
                {
                    video.CreateTime = DateTime.Now;
                    video.UpdateTime = DateTime.Now;
                    db.Videos.Add(video);
                }
                db.SaveChanges();
                return RedirectToAction("CellList",new { collectionId=video.VideoCollectionId});
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
                var video = db.Videos.Find(id);
                db.Videos.Remove(video);
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
            VideoCollections article = db.VideoCollections.Find(id);
            return View(article);
        }
        public ActionResult MobilePreview(int id)
        {
            ViewBag.videoId = id;
            return View();
        }
        public ActionResult VideoPreview(int id)
        {
            Videos video = db.Videos.Find(id);
            return View(video);
        }
    }
}