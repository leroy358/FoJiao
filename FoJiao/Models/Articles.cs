using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoJiao.Models
{
    public class Articles
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Cont { get; set; }
        public string PicIndex { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Grade { get; set; }
        public string Tags { get; set; }
        /// <summary>
        /// 1、文章阅读2、法师法训3、平台活动
        /// </summary>
        public int Category { get; set; }
        public bool IsPublished { get; set; }

    }
}