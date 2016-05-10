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
    public class VideoController : Controller
    {
        private FoJiaoDbContext db = new FoJiaoDbContext();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "Id":2,
        ///             "CreateTime":"2016-03-17",
        ///             "Title":"第一个视频",
        ///             "PicAD":"/UserFiles/images/QQ截图20150901175856.png",
        ///             "PicWidth":1104,
        ///             "PicHeight":585,
        ///             "Level":"中级",
        ///             "Tags":"初级视频|佛"
        ///         },
        ///         {
        ///             "Id":1,
        ///             "CreateTime":"2016-03-16",
        ///             "Title":"sad是否",
        ///             "PicAD":"/UserFiles/images/00e9df2850f1a4be0c0873333d09a54a.jpg",
        ///             "PicWidth":640,
        ///             "PicHeight":355,
        ///             "Level":null,
        ///             "Tags":"倒萨风电|撒旦撒"
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetVideoList()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            int pageIndex = Convert.ToInt32(jo["pageIndex"].ToString());
            List<VideoCollections> videoCollections = new List<VideoCollections>();
            List<VideoListData> videoDataList = new List<VideoListData>();
            if (pageIndex == 0)
            {
                videoCollections = db.VideoCollections.Where(item => item.IsPublished).OrderByDescending(item => item.Id).Take(10).ToList();
            }
            else
            {
                videoCollections = db.VideoCollections.Where(item => item.Id < pageIndex && item.IsPublished).OrderByDescending(item => item.Id).Take(10).ToList();
            }
            foreach (VideoCollections videoCollection in videoCollections)
            {
                int originalWidth = 0;
                int originalHeight = 0;
                if (videoCollection.PicIndex != null)
                {
                    System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(Server.MapPath(HttpUtility.UrlDecode(videoCollection.PicIndex)));
                    originalWidth = imgOriginal.Width;
                    originalHeight = imgOriginal.Height;
                }
                VideoListData videoData = new VideoListData();
                videoData.Id = videoCollection.Id;
                videoData.Title = videoCollection.Title;
                videoData.CreateTime = videoCollection.CreateTime.ToString("yyyy-MM-dd");
                if (videoCollection.PicIndex != null)
                {
                    videoData.PicAD =  videoCollection.PicIndex;
                }
                else
                {
                    videoData.PicAD = "";
                }
                videoData.Tags = videoCollection.Tags;
                videoData.Level = videoCollection.Grade;
                videoData.PicWidth = originalWidth;
                videoData.PicHeight = originalHeight;
                videoDataList.Add(videoData);
            }
            return Json(new { data = videoDataList });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":{
        ///         "Id":2,
        ///         "Title":"第一个视频",
        ///         "CreateTime":"2016-03-17",
        ///         "Level":"中级",
        ///         "Tags":"初级视频|佛",
        ///         "Content":"/admin/video/preview/1",
        ///         "VideoDetail":[
        ///             {
        ///                 "Id":6,
        ///                 "PicAD":"/UserFiles/images/00e9df2850f1a4be0c0873333d09a54a.jpg",
        ///                 "Title":"第一个视频",
        ///                 "VideoLink":"/UserFiles/files/v1.mp4"
        ///             }
        ///         ]
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetVideoDetail()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            int videoCollectionId = Convert.ToInt32(jo["videoId"].ToString());
            VideoCollections videoCollection = db.VideoCollections.Find(videoCollectionId);
            List<Videos> videos = db.Videos.Where(item => item.VideoCollectionId == videoCollectionId).ToList();
            VideoDetailData videoDetailData = new VideoDetailData();
            videoDetailData.Id = videoCollection.Id;
            videoDetailData.Title = videoCollection.Title;
            videoDetailData.CreateTime = videoCollection.CreateTime.ToString("yyyy-MM-dd");
            videoDetailData.Level = videoCollection.Grade;
            videoDetailData.Tags = videoCollection.Tags;
            videoDetailData.Content = "/admin/video/preview/" + videoCollectionId;
            List<VideoDetail> videoDetails = new List<VideoDetail>();
            foreach (Videos video in videos)
            {
                VideoDetail videoDetail = new VideoDetail();
                videoDetail.Id = video.Id;
                int originalWidth = 0;
                int originalHeight = 0;

                if (video.VideoIndex != null)
                {
                    videoDetail.PicAD = video.VideoIndex;
                    System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(Server.MapPath(HttpUtility.UrlDecode(videoCollection.PicIndex)));
                    originalWidth = imgOriginal.Width;
                    originalHeight = imgOriginal.Height;
                }
                else
                {
                    videoDetail.PicAD = "";
                }
                videoDetail.PicWidth = originalWidth;
                videoDetail.PicHeight = originalHeight;
                videoDetail.Title = video.Title;
                if (video.VideoLink != null)
                {
                    videoDetail.VideoLink = video.VideoLink;
                }
                else
                {
                    videoDetail.VideoLink = "";
                }
                videoDetails.Add(videoDetail);
            }
            videoDetailData.VideoDetail = videoDetails;
            return Json(new { data = videoDetailData });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "Id":2,
        ///             "CreateTime":"2016-03-17",
        ///             "Title":"第一个视频",
        ///             "PicAD":"/UserFiles/images/QQ截图20150901175856.png",
        ///             "PicWidth":1104,
        ///             "PicHeight":585,
        ///             "Level":"中级"
        ///             "Tags":"初级视频|佛"
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult SearchVideo()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            int pageIndex = Convert.ToInt32(jo["pageIndex"].ToString());
            string tags = jo["keyWord"].ToString();
            string level = jo["level"].ToString();
            List<VideoCollections> videoCollections = new List<VideoCollections>();
            List<VideoListData> videoDataList = new List<VideoListData>();
            if (level == "不限")
            {
                if (pageIndex == 0)
                {
                    videoCollections = db.VideoCollections.Where(item => item.IsPublished && item.Tags.Contains(tags)).OrderByDescending(item => item.Id).Take(10).ToList();
                }
                else
                {
                    videoCollections = db.VideoCollections.Where(item => item.Id < pageIndex && item.IsPublished && item.Tags.Contains(tags)).OrderByDescending(item => item.Id).Take(10).ToList();
                }
            }
            else
            {
                if (pageIndex == 0)
                {
                    videoCollections = db.VideoCollections.Where(item => item.IsPublished && item.Tags.Contains(tags) && item.Grade == level).OrderByDescending(item => item.Id).Take(10).ToList();
                }
                else
                {
                    videoCollections = db.VideoCollections.Where(item => item.Id < pageIndex && item.IsPublished && item.Tags.Contains(tags) && item.Grade == level).OrderByDescending(item => item.Id).Take(10).ToList();
                }
            }
            
            foreach (VideoCollections videoCollection in videoCollections)
            {
                int originalWidth = 0;
                int originalHeight = 0;
                if (videoCollection.PicIndex != null)
                {
                    System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(Server.MapPath(HttpUtility.UrlDecode(videoCollection.PicIndex)));
                    originalWidth = imgOriginal.Width;
                    originalHeight = imgOriginal.Height;
                }
                VideoListData videoData = new VideoListData();
                videoData.Id = videoCollection.Id;
                videoData.Title = videoCollection.Title;
                videoData.CreateTime = videoCollection.CreateTime.ToString("yyyy-MM-dd");
                if (videoCollection.PicIndex != null)
                {
                    videoData.PicAD = videoCollection.PicIndex;

                }
                else
                {
                    videoData.PicAD = "";
                }
                videoData.Tags = videoCollection.Tags;
                videoData.Level = videoCollection.Grade;
                videoData.PicWidth = originalWidth;
                videoData.PicHeight = originalHeight;
                videoDataList.Add(videoData);
            }
            return Json(new { data = videoDataList });
        }
    }
}