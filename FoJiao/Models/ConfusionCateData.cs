using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoJiao.Models
{
    public class ConfusionCateData
    {
        public int Id { get; set; }
        public string GropeName { get; set; }
        public List<ConfusionSecData> CellList { get; set; }
    }
    public class ConfusionSecData
    {
        public int Id { get; set; }
        public string CellName { get; set; }
    }
}