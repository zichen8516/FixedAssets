using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FixedAssets.Filter
{
    public class ExceptionFilter: HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Logger.logger log = new Logger.logger();
            log.Log(filterContext.Exception);
            base.OnException(filterContext);
        }
    }
}