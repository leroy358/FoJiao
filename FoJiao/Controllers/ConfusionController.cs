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
    public class ConfusionController : Controller
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
        ///             "GropeName":"新中国vbfgjnh",
        ///             "CellList":[
        ///                 {
        ///                     "Id":1,
        ///                     "CellName":"2015美国最新奥斯卡获奖《聚焦 Spotlight》BD高清 中英双字"
        ///                 },
        ///                 {
        ///                     "Id":2,
        ///                     "CellName":"撒的风电行业更健康"
        ///                 }
        ///             ]
        ///         },
        ///         {
        ///             "Id":3,
        ///             "GropeName":"2015美国最新奥斯卡获奖《聚焦 Spotlight》BD高清 中英双字",
        ///             "CellList":[
        ///                 {
        ///                     "Id":3,
        ///                     "CellName":"卧虎藏龙：青冥宝剑  HD高清  中英双字"
        ///                 }
        ///             ]
        ///         },
        ///         {
        ///             "Id":4,
        ///             "GropeName":"第一类",
        ///             "CellList":[
        ///                 {
        ///                     "Id":4,
        ///                     "CellName":"第一类的第一个"
        ///                 }
        ///             ]
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetConfusionCateList()
        {
            List<ConfusionCateData> confusionCateDataList = new List<ConfusionCateData>();
         
            foreach(ConfusionCate confusionCate in db.ConfusionCate)
            {
                ConfusionCateData confusionCateData = new ConfusionCateData();
                confusionCateData.Id = confusionCate.Id;
                confusionCateData.GropeName = confusionCate.Title;
                List<ConfusionSecCate> confusionSecCateList = db.ConfusionSecCate.Where(item => item.ConfusionCateId == confusionCate.Id).ToList();
                List<ConfusionSecData> ConfusionSecDataList = new List<ConfusionSecData>();
                foreach (ConfusionSecCate confusionSecCate in confusionSecCateList)
                {
                    ConfusionSecData confusionSecData = new ConfusionSecData();
                    confusionSecData.Id = confusionSecCate.Id;
                    confusionSecData.CellName = confusionSecCate.Title;
                    ConfusionSecDataList.Add(confusionSecData);
                }
                confusionCateData.CellList = ConfusionSecDataList;
                confusionCateDataList.Add(confusionCateData);
            }
            return Json(new { data = confusionCateDataList });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "Id":1,
        ///             "Title":"但是规范化",
        ///             "Tags":"丰东股份|汇聚语言"
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetConfusionList()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            int cateId = Convert.ToInt32(jo["cateId"].ToString());
            int pageIndex = Convert.ToInt32(jo["pageIndex"].ToString());
            List<ConfusionListData> confusionList = new List<ConfusionListData>();
            List<Confusions> confusions = new List<Confusions>();
            if (pageIndex == 0)
            {
                confusions = db.Confusions.Where(item => item.ConfusionSecId == cateId).OrderByDescending(item => item.Id).Take(10).ToList();
            }
            else
            {
                confusions = db.Confusions.Where(item => item.ConfusionSecId == cateId && item.Id < pageIndex).ToList().OrderByDescending(item => item.Id).Take(10).ToList();
            }
            foreach(Confusions confusion in confusions)
            {
                ConfusionListData confusionData = new ConfusionListData();
                confusionData.Id = confusion.Id;
                confusionData.Tags = confusion.Tags;
                confusionData.Title = confusion.Title;
                confusionList.Add(confusionData);
            }
            return Json(new { data = confusionList });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":{
        ///         "Id":1,
        ///         "Title":"但是规范化",
        ///         "Tags":"丰东股份|汇聚语言",
        ///         "Answers":"/admin/ConfusionCate/Preview/1"
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetConfusionDetail()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            int confusionId = Convert.ToInt32(jo["confusionId"].ToString());
            Confusions confusion = db.Confusions.Find(confusionId);
            ConfusionDetailData confusionDetail = new ConfusionDetailData();
            confusionDetail.Id = confusion.Id;
            confusionDetail.Title = confusion.Title;
            confusionDetail.Tags = confusion.Tags;
            confusionDetail.Answers = "/admin/ConfusionCate/Preview/" + confusionId;
            return Json(new { data = confusionDetail });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "Id":1,
        ///             "Title":"但是规范化",
        ///             "Tags":"丰东股份|汇聚语言"
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult SearchConfusion()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            int cateId = Convert.ToInt32(jo["cateId"].ToString());
            int pageIndex = Convert.ToInt32(jo["pageIndex"].ToString());
            string tags = jo["keyWord"].ToString();
            List<ConfusionListData> confusionList = new List<ConfusionListData>();
            List<Confusions> confusions = new List<Confusions>();
            if (pageIndex == 0)
            {
                confusions = db.Confusions.Where(item => item.ConfusionSecId == cateId && item.Tags.Contains(tags)).OrderByDescending(item => item.Id).Take(10).ToList();
            }
            else
            {
                confusions = db.Confusions.Where(item => item.ConfusionSecId == cateId && item.Id < pageIndex && item.Tags.Contains(tags)).ToList().OrderByDescending(item => item.Id).Take(10).ToList();
            }
            foreach (Confusions confusion in confusions)
            {
                ConfusionListData confusionData = new ConfusionListData();
                confusionData.Id = confusion.Id;
                confusionData.Tags = confusion.Tags;
                confusionData.Title = confusion.Title;
                confusionList.Add(confusionData);
            }
            return Json(new { data = confusionList });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "Id":2,
        ///             "Title":"sad是否",
        ///             "IsAnswered":1
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetMyConfusionList()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            string userId =jo["userId"].ToString();
            int pageIndex = Convert.ToInt32(jo["pageIndex"].ToString());
            List<ConfusionGuestListData> confusionList = new List<ConfusionGuestListData>();
            List<ConfusionGuest> confusions = new List<ConfusionGuest>();
            if (pageIndex == 0)
            {
                confusions = db.ConfusionGuest.Where(item => item.UserId == userId).OrderByDescending(item => item.Id).Take(10).ToList();
            }
            else
            {
                confusions = db.ConfusionGuest.Where(item => item.UserId == userId && item.Id < pageIndex).ToList().OrderByDescending(item => item.Id).Take(10).ToList();
            }
            foreach (ConfusionGuest confusion in confusions)
            {
                ConfusionGuestListData confusionData = new ConfusionGuestListData();
                confusionData.Id = confusion.Id;
                confusionData.IsAnswered = confusion.StateId;
                confusionData.Title = confusion.Title;
                confusionList.Add(confusionData);
            }
            return Json(new { data = confusionList });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":{
        ///         "Id":2,
        ///         "Title":"sad是否"
        ///         "Answers":"/admin/Confusion/Preview/2" ;
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetMyConfusionDetail()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            int confusionId = Convert.ToInt32(jo["confusionId"].ToString());
            ConfusionGuest confusion = db.ConfusionGuest.Find(confusionId);
            ConfusionGuestDetailData confusionDetail = new ConfusionGuestDetailData();
            confusionDetail.Id = confusion.Id;
            if (string.IsNullOrEmpty(confusion.Answers))
            {
                confusionDetail.Answers = "";
            }
            else
            {
                confusionDetail.Answers = "/admin/Confusion/Preview/" + confusionId;
            }
            confusionDetail.Title = confusion.Title;

            return Json(new { data = confusionDetail });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":1
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult UploadMyConfusion()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            string userId = jo["userId"].ToString();
            string title = jo["question"].ToString();
            int source = Convert.ToInt32(jo["source"].ToString());

            ConfusionGuest confusion = new ConfusionGuest();
            confusion.Title = title;
            confusion.UserId = userId;
            confusion.CreateTime = DateTime.Now;
            confusion.UpdateTime = DateTime.Now;
            confusion.StateId = 0;
            confusion.SourceCate = source;
            confusion.IsDeleted = false;
            db.Entry(confusion).State = System.Data.Entity.EntityState.Added;
            int isSuccess = db.SaveChanges();
            return Json(new { data = isSuccess });
        }
    }
}