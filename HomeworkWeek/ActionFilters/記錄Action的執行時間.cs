using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeworkWeek1.ActionFilters {
    public class 記錄Action的執行時間 :ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            //記錄開始時間
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext) {
            //計算執行時間
            TimeSpan executionTime = TimeSpan.FromHours(1);
            filterContext.Controller.ViewBag.執行時間 = executionTime;
            Debug.WriteLine(executionTime.ToString());

            base.OnActionExecuted(filterContext);
            
        }
    }
}