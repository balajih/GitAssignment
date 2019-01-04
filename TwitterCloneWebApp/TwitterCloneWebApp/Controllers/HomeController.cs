using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterCloneWebApp.Models;

namespace TwitterCloneWebApp.Controllers
{
    public class HomeController : Controller
    {
        [CustomAuthFilter]
        public ActionResult Index()
        {
            var userName = (string)Session["UserName"];
            var vm = DataAccessLayer.GetTweetsForCurrentUser(userName);
            vm.Username = userName;
            return View(vm);
        }

        public JsonResult UpdateTweet(string msg, string userId)
        {
            try
            {
                bool result = DataAccessLayer.UpdateTweet(msg, userId);
            }
            catch(Exception e)
            {
                throw e;
            }
            return Json(true);
        }
    }
}