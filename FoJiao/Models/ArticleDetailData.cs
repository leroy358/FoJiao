using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoJiao.Models
{
    public class ArticleDetailData
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CreateTime { get; set; }
        public string Level { get; set; }
        public string Tags { get; set; }
        public string Content { get; set; }
    }
}