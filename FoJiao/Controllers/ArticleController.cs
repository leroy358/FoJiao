using FoJiao.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoJiao.Controllers
{
    public class ArticleController : Controller
    {
        private FoJiaoDbContext db = new FoJiaoDbContext();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "Id":6,
        ///             "CreateTime":"2016-03-17",
        ///             "Title":"习近平两会全纪录",
        ///             "PicAD":"/UserFiles/images/00e9df2850f1a4be0c0873333d09a54a.jpg",
        ///             "PicWidth":640,
        ///             "PicHeight":355,
        ///             "Level":"中级",
        ///             "Tags":"白宫|奥巴马"
        ///         },
        ///         {
        ///             "Id":5,
        ///             "CreateTime":"2016-03-16",
        ///             "Title":"111111111111111111111",
        ///             "PicAD":"/UserFiles/images/00e9df2850f1a4be0c0873333d09a54a.jpg",
        ///             "PicWidth":640,
        ///             "PicHeight":355,
        ///             "Level":null,
        ///             "Tags":"12222222222222222"
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public  ActionResult GetArticleList()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            int category = Convert.ToInt32(jo["category"].ToString());
            int pageIndex= Convert.ToInt32(jo["pageIndex"].ToString());
            List<Articles> articleList = new List<Articles>();
            List<ArticleListData> articleDataList = new List<ArticleListData>();
            //!File.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + fileName)
            if (pageIndex == 0)
            {
                articleList = db.Articles.Where(item => item.Category == category && item.IsPublished).OrderByDescending(item => item.Id).Take(10).ToList();
            }
            else
            {
                articleList = db.Articles.Where(item => item.Category == category && item.IsPublished && item.Id < pageIndex).OrderByDescending(item => item.Id).Take(10).ToList();
            }
            foreach(Articles article in articleList)
            {
                int originalWidth = 0;
                int originalHeight = 0;
                if (article.PicIndex != null)
                {
                    System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(Server.MapPath(HttpUtility.UrlDecode(article.PicIndex)));
                    originalWidth = imgOriginal.Width;
                    originalHeight = imgOriginal.Height;
                }
                ArticleListData articleData = new ArticleListData();
                articleData.Id = article.Id;
                articleData.Title = article.Title;
                articleData.CreateTime = article.CreateTime.ToString("yyyy-MM-dd");
                if (article.PicIndex != null)
                {
                    articleData.PicAD = article.PicIndex;
                }
                else
                {
                    articleData.PicAD = "";
                }
                articleData.Tags = article.Tags;
                articleData.Level = article.Grade;
                articleData.PicWidth = originalWidth;
                articleData.PicHeight = originalHeight;
                articleDataList.Add(articleData);
            }
            return Json(new { data = articleDataList });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":{
        ///         "Id":6,
        ///         "Title":"习近平两会全纪录",
        ///         "CreateTime":"2016-03-17",
        ///         "Level":"中级",
        ///         "Tags":"白宫|奥巴马",
        ///         "Content":/admin/Article/ArticleView/6"
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetArticleDetail()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            int articleId = Convert.ToInt32(jo["articleId"].ToString());
            Articles article = db.Articles.Find(articleId);
            ArticleDetailData articleDetail = new ArticleDetailData();
            articleDetail.Id = article.Id;
            articleDetail.Title = article.Title;
            articleDetail.CreateTime=article.CreateTime.ToString("yyyy-MM-dd");
            articleDetail.Level = article.Grade;
            articleDetail.Tags = article.Tags;
            articleDetail.Content = "/admin/Article/preView/" + articleId;
            
            return Json(new { data = articleDetail });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "Id":6,
        ///             "CreateTime":"2016-03-17",
        ///             "Title":"习近平两会全纪录",
        ///             "PicAD":"/UserFiles/images/00e9df2850f1a4be0c0873333d09a54a.jpg",
        ///             "PicWidth":640,
        ///             "PicHeight":355,
        ///             "Level":"中级",
        ///             "Tags":"白宫|奥巴马"
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult SearchArticle()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            int category = Convert.ToInt32(jo["category"].ToString());
            int pageIndex = Convert.ToInt32(jo["pageIndex"].ToString());
            string tags = jo["keyWord"].ToString();
            string level = jo["level"].ToString();
            List<Articles> articleList = new List<Articles>();
            List<ArticleListData> articleDataList = new List<ArticleListData>();
            if (level == "不限")
            {
                if (pageIndex == 0)
                {
                    articleList = db.Articles.Where(item => item.Category == category && item.IsPublished && item.Tags.Contains(tags)).OrderByDescending(item => item.Id).Take(10).ToList();
                }
                else
                {
                    articleList = db.Articles.Where(item => item.Category == category && item.IsPublished && item.Tags.Contains(tags) && item.Id < pageIndex).OrderByDescending(item => item.Id).Take(10).ToList();
                }
            }
            else
            {
                if (pageIndex == 0)
                {
                    articleList = db.Articles.Where(item => item.Category == category && item.IsPublished && item.Tags.Contains(tags) && item.Grade == level).OrderByDescending(item => item.Id).Take(10).ToList();
                }
                else
                {
                    articleList = db.Articles.Where(item => item.Category == category && item.IsPublished && item.Tags.Contains(tags) && item.Grade == level && item.Id < pageIndex).OrderByDescending(item => item.Id).Take(10).ToList();
                }
            }
            
            foreach (Articles article in articleList)
            {
                int originalWidth = 0;
                int originalHeight = 0;
                if (article.PicIndex != null)
                {
                    System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(Server.MapPath(HttpUtility.UrlDecode(article.PicIndex)));
                    originalWidth = imgOriginal.Width;
                    originalHeight = imgOriginal.Height;
                }
                ArticleListData articleData = new ArticleListData();
                articleData.Id = article.Id;
                articleData.Title = article.Title;
                articleData.CreateTime = article.CreateTime.ToString("yyyy-MM-dd");
                if (article.PicIndex != null)
                {
                    articleData.PicAD = article.PicIndex;
                }
                else
                {
                    articleData.PicAD = "";
                }
                articleData.Tags = article.Tags;
                articleData.Level = article.Grade;
                articleData.PicWidth = originalWidth;
                articleData.PicHeight = originalHeight;
                articleDataList.Add(articleData);
            }
            return Json(new { data = articleDataList });
        }
    }
}
