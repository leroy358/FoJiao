using FoJiao.Models;
using System.Web;
using System.Web.Mvc;

namespace FoJiao
{
    public class FilterConfig
    {
        //public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        //{
        //    filters.Add(new HandleErrorAttribute());
        //}
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new MyHandleExceptionAttribute());
        }
    }
}
