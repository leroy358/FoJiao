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
    public class AudioController : Controller
    {
        private FoJiaoDbContext db = new FoJiaoDbContext();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "Id":5,
        ///             "CreateTime":"2016-03-17",
        ///             "Title":"第一个音乐",
        ///             "PicAD":"/UserFiles/images/00e9df2850f1a4be0c0873333d09a54a.jpg",
        ///             "PicWidth":640,
        ///             "PicHeight":355,
        ///             "Level":"资深",
        ///             "Tags":"第一个音乐|音乐"
        ///         },
        ///         {
        ///             "Id":4,
        ///             "CreateTime":"2016-03-17",
        ///             "Title":"第一个音频",
        ///             "PicAD":"/UserFiles/images/QQ截图20150901175856.png",
        ///             "PicWidth":1104,
        ///             "PicHeight":585,
        ///             "Level":"进阶",
        ///             "Tags":"音频|第一"
        ///         },
        ///         {
        ///             "Id":2,
        ///             "CreateTime":"2016-03-16",
        ///             "Title":"2015美国最新奥斯卡获奖《聚焦 Spotlight》BD高清 中英双字",
        ///             "PicAD":"/UserFiles/images/QQ截图20150901175856.png",
        ///             "PicWidth":1104,
        ///             "PicHeight":585,
        ///             "Level":null,
        ///             "Tags":"萨芬的给"
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetAudioList()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            int pageIndex = Convert.ToInt32(jo["pageIndex"].ToString());
            int category = Convert.ToInt32(jo["category"].ToString());
            List<AudioCollections> audioCollections = new List<AudioCollections>();
            List<AudioListData> audioListData = new List<AudioListData>();
            if (pageIndex == 0)
            {
                audioCollections = db.AudioCollections.Where(item => item.Category == category && item.IsPublished).OrderByDescending(item => item.Id).Take(10).ToList();
            }
            else
            {
                audioCollections = db.AudioCollections.Where(item => item.Category == category && item.Id < pageIndex && item.IsPublished).OrderByDescending(item => item.Id).Take(10).ToList();
            }
            foreach (AudioCollections audeoCollection in audioCollections)
            {
                int originalWidth = 0;
                int originalHeight = 0;
                if (audeoCollection.PicIndex != null)
                {
                    System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(Server.MapPath(HttpUtility.UrlDecode(audeoCollection.PicIndex)));
                    originalWidth = imgOriginal.Width;
                    originalHeight = imgOriginal.Height;
                }
                AudioListData audioData = new AudioListData();
                audioData.Id = audeoCollection.Id;
                audioData.Title = audeoCollection.Title;
                audioData.CreateTime = audeoCollection.CreateTime.ToString("yyyy-MM-dd");
                if (audeoCollection.PicIndex != null)
                {
                    audioData.PicAD = audeoCollection.PicIndex;
                }
                else
                {
                    audioData.PicAD = "";
                }
                audioData.Tags = audeoCollection.Tags;
                audioData.Level = audeoCollection.Grade;
                audioData.PicWidth = originalWidth;
                audioData.PicHeight = originalHeight;
                audioListData.Add(audioData);
            }
            return Json(new { data = audioListData });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":
        ///     {
        ///         "Id":5,
        ///         "Title":"第一个音乐",
        ///         "CreateTime":"2016-03-17",
        ///         "Level":"资深",
        ///         "Tags":"第一个音乐|音乐",
        ///         "Content":"/admin/audio/preview/5",
        ///         "AudioDetail":[
        ///             {
        ///                 "Id":4,
        ///                 "PicAD":"/UserFiles/files/746052bc98f61e17489e5b84da049e85.mp3",
        ///                 "Title":"第一个音乐的第一个音乐",
        ///                 "AudioLink":"/UserFiles/files/746052bc98f61e17489e5b84da049e85.mp3"
        ///             }
        ///         ]
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetAudioDetailList()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            int audeoCollectionId = Convert.ToInt32(jo["audioId"].ToString());
            AudioCollections audeoCollection = db.AudioCollections.Find(audeoCollectionId);
            List<Audios> audios = db.Audios.Where(item => item.AudioCollectionId == audeoCollectionId).ToList();
            AudioDetailData audioDetailData = new AudioDetailData();
            audioDetailData.Id = audeoCollection.Id;
            audioDetailData.Title = audeoCollection.Title;
            audioDetailData.CreateTime = audeoCollection.CreateTime.ToString("yyyy-MM-dd");
            audioDetailData.Level = audeoCollection.Grade;
            audioDetailData.Tags = audeoCollection.Tags;
            audioDetailData.Content = "/admin/audio/preview/" + audeoCollectionId;
            List<AudioDetail> videoDetails = new List<AudioDetail>();
            foreach (Audios audio in audios)
            {
                AudioDetail audioDetail = new AudioDetail();
                audioDetail.Id = audio.Id;
                if (audio.AudioIndex != null)
                {
                    audioDetail.PicAD = audio.AudioIndex;

                }
                else
                {
                    audioDetail.PicAD = "";
                }
                audioDetail.Title = audio.Title;
                if (audio.AudioLink != null)
                {
                    audioDetail.AudioLink = audio.AudioLink;
                }
                else
                {
                    audioDetail.AudioLink = "";
                }
                videoDetails.Add(audioDetail);
            }
            audioDetailData.AudioDetail = videoDetails;
            return Json(new { data = audioDetailData });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":{
        ///         "Id":4,
        ///         "PicAD":"/UserFiles/images/QQ截图20150901175856.png",
        ///         "Title":"现在发132",
        ///         "AudioLink":"/UserFiles/files/746052bc98f61e17489e5b84da049e85.mp3",
        ///         "Content":"/admin/audio/previewaudio/4"
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetAudioDetail()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            int audioId = Convert.ToInt32(jo["audioId"].ToString());
            Audios audio = db.Audios.Where(item => item.Id == audioId).FirstOrDefault();
            AudioDetailDetail audioDetailDetail = new AudioDetailDetail();
            audioDetailDetail.Id = audio.Id;
            audioDetailDetail.Title = audio.Title;
            audioDetailDetail.PicAD = audio.AudioIndex;
            audioDetailDetail.AudioLink =  audio.AudioLink;
            audioDetailDetail.Content = "/admin/audio/previewaudio/" + audioId;

            return Json(new { data = audioDetailDetail });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "Id":4
        ///             "CreateTime":"2016-03-17",
        ///             "Title":"第一个音频",
        ///             "PicAD":"/UserFiles/images/QQ截图20150901175856.png",
        ///             "PicWidth":1104,
        ///             "PicHeight":585,
        ///             "Level":"进阶",
        ///             "Tags":"音频|第一"
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult SearchAudio()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            int pageIndex = Convert.ToInt32(jo["pageIndex"].ToString());
            string tags = jo["keyWord"].ToString();
            string level = jo["level"].ToString();
            int category = Convert.ToInt32(jo["category"].ToString());
            List<AudioCollections> audioCollections = new List<AudioCollections>();
            List<AudioListData> audioListData = new List<AudioListData>();
            if (level == "不限")
            {
                if (pageIndex == 0)
                {
                    audioCollections = db.AudioCollections.Where(item => item.Category == category && item.IsPublished && item.Tags.Contains(tags)).OrderByDescending(item => item.Id).Take(10).ToList();
                }
                else
                {
                    audioCollections = db.AudioCollections.Where(item => item.Category == category && item.Id < pageIndex && item.IsPublished && item.Tags.Contains(tags)).OrderByDescending(item => item.Id).Take(10).ToList();
                }
            }
            else
            {
                if (pageIndex == 0)
                {
                    audioCollections = db.AudioCollections.Where(item => item.Category == category && item.IsPublished && item.Tags.Contains(tags) && item.Grade == level).OrderByDescending(item => item.Id).Take(10).ToList();
                }
                else
                {
                    audioCollections = db.AudioCollections.Where(item => item.Category == category && item.Id < pageIndex && item.IsPublished && item.Tags.Contains(tags) && item.Grade == level).OrderByDescending(item => item.Id).Take(10).ToList();
                }
            }          
            foreach (AudioCollections audeoCollection in audioCollections)
            {
                int originalWidth = 0;
                int originalHeight = 0;
                if (audeoCollection.PicIndex != null)
                {
                    System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(Server.MapPath(HttpUtility.UrlDecode(audeoCollection.PicIndex)));
                    originalWidth = imgOriginal.Width;
                    originalHeight = imgOriginal.Height;
                }
                AudioListData audioData = new AudioListData();
                audioData.Id = audeoCollection.Id;
                audioData.Title = audeoCollection.Title;
                audioData.CreateTime = audeoCollection.CreateTime.ToString("yyyy-MM-dd");
                if (audeoCollection.PicIndex != null)
                {
                    audioData.PicAD = audeoCollection.PicIndex;
                }
                else
                {
                    audioData.PicAD = "";
                }
                audioData.Tags = audeoCollection.Tags;
                audioData.Level = audeoCollection.Grade;
                audioData.PicWidth = originalWidth;
                audioData.PicHeight = originalHeight;
                audioListData.Add(audioData);
            }
            return Json(new { data = audioListData });
        }
    }
}