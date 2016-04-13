using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoJiao.Models
{
    public class ConfusionGuest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Answers { get; set; }
        public string UserId { get; set; }
        /// <summary>
        /// 0、未处理1、已回答2、删除
        /// </summary>
        public int StateId { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 用户来源1苹果，2安卓
        /// </summary>
        public int SourceCate { get; set; }
        //public bool IsAnswered { get; set; }
        //public bool IsDeleted { get; set; }
        //public bool IsPublished { get; set; }
    }
}