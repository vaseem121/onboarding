using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smartTechAuthenticator.Services.Comman.CustomFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string originalPath = HttpContext.Current.Request.Url.PathAndQuery;
            string url = "~/Account/SignIn?returnUrl=" + HttpUtility.UrlEncode(originalPath);
            if (HttpContext.Current.Session["UserId"] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var urlHelper = new UrlHelper(filterContext.RequestContext);
                    filterContext.HttpContext.Response.StatusCode = 451;
                    filterContext.HttpContext.Response.StatusDescription = "Unauthorized Access";
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            Error = "NotAuthorized",
                            LogOnUrl = urlHelper.Action("SignIn", "Account")
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                   // filterContext.Result = new RedirectResult(url);
                    return;
                }
                else
                {
                    filterContext.Result = new RedirectResult(url);
                    return;
                }
            } 
        }
    }
}