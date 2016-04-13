using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoJiao.Models
{
    public class ArticleListData
    {
        public int Id { get; set; }
        public string CreateTime { get; set; }
        public string Title { get; set; }
        public string PicAD { get; set; }
        public int PicWidth { get; set; }
        public int PicHeight { get; set; }
        public string Level { get; set; }
        public string Tags { get; set; }
    }
}