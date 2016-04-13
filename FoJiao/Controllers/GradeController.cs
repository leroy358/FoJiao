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

    public class GradeController : Controller
    {
        private FoJiaoDbContext db = new FoJiaoDbContext();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "Id":1,
        ///             "GradeName":"初级
        ///         },
        ///         {
        ///             "Id":2,
        ///             "GradeName":"中级"
        ///         },
        ///         {
        ///             "Id":3
        ///             "GradeName":"进阶"
        ///         },
        ///         {
        ///             "Id":4,
        ///             "GradeName":"资深"
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetGradeList()
        {
            List<GradeData> gradeDataList = new List<GradeData>();
            GradeData extra = new GradeData();
            extra.Id = 0;
            extra.GradeName = "不限";
            gradeDataList.Add(extra);
            foreach (Grades grade in db.Grades)
            {
                GradeData gradeData = new GradeData();
                gradeData.Id = grade.Id;
                gradeData.GradeName = grade.Title;
                gradeDataList.Add(gradeData);
            }
            return Json(new { data = gradeDataList });

        }
        [HttpPost]
        public ActionResult CheckGrade()
        {
            var stream = HttpContext.Request.InputStream;
            string requestJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            JObject jo = (JObject)JsonConvert.DeserializeObject(requestJson);
            string name = jo["gradeName"].ToString();
            int gradeId = Convert.ToInt32(jo["gradeId"].ToString());
            Grades grade = db.Grades.Where(item => item.Title == name).FirstOrDefault();
            if (grade != null)
            {
                if (grade.Id == gradeId)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Error");
                }
            }
            else
            {
                return Json("Success");
            }
        }
    }
}