using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoJiao.Models
{
    public class Confusions
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Answers { get; set; }
        public string PicIndex { get; set; }
        public string Tags { get; set; }
        public int ConfusionSecId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        //public bool IsPublished { get; set; }
    }
}