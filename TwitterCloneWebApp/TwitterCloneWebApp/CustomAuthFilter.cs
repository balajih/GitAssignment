using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace TwitterCloneWebApp
{
    public class CustomAuthFilter : FilterAttribute,  IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
           if(string.IsNullOrEmpty((string)filterContext.RequestContext.HttpContext.Session["UserName"]))
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
            }

        }
    }   
}