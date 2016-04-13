using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoJiao.Models
{
    public class MyHandleExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            base.OnException(filterContext);
            logger.Error(filterContext.Exception);
            //filterContext.HttpContext.Response.Redirect("/Error.html");
        }
    }
}