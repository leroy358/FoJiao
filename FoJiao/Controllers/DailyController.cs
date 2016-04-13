using FoJiao.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoJiao.Controllers
{
    public class DailyController : Controller
    {
        private FoJiaoDbContext db = new FoJiaoDbContext();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "result":"Success",
        ///     "data":{
        ///         "Id":3,
        ///         "Title":"第一句",
        ///         "CreateTime":"2016-03-17"
        ///     }
        /// }
        /// 
        /// {
        ///     "result":"Error"
        ///     "data":""
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetDailyWord()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            //DateTime dataStr = Convert.ToDateTime(jo["dataStr"].ToString());
            int selectRow = Convert.ToInt32(jo["selectRow"].ToString());
            //DateTime selectTime = dataStr.AddDays(-selectRow + 1);

            //DailyWords dailyWord = db.DailyWords.Where(item => item.CreateTime < selectTime).OrderByDescending(item => item.CreateTime).FirstOrDefault();
            //if (dailyWord != null)
            //{
            //    DailyWordsData dailyWordsData = new DailyWordsData();
            //    dailyWordsData.CreateTime = dailyWord.CreateTime.ToString("yyyy-MM-dd");
            //    dailyWordsData.Id = dailyWord.Id;
            //    dailyWordsData.Title = dailyWord.Title;
            //    return Json(new { result = "Success", data = dailyWordsData });
            //}
            //else
            //{
            //    return Json(new { result = "Error", data = "" });
            //}
            DailyWords word = db.DailyWords.OrderByDescending(item => item.Id).Skip(selectRow).FirstOrDefault();
            if (word != null)
            {
                DailyWordsData dailyWordsData = new DailyWordsData();
                dailyWordsData.CreateTime = word.CreateTime.ToString("yyyy-MM-dd");
                dailyWordsData.Id = word.Id;
                dailyWordsData.Title = word.Title;
                return Json(new { result = "Success", data = dailyWordsData });
            }
            else
            {
                return Json(new { result = "Error", data = "" });
            }

        }

    }
}