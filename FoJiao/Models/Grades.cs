using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoJiao.Models
{
    public class Grades
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsPublished { get; set; }
    }
}