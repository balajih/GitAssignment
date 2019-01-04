using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TwitterCloneWebApp.Models;
using TwitterCloneWebApp.DataAccess;

namespace TwitterCloneWebApp.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {       
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =  DataAccessLayer.IsValidUser(model.UserId, model.Password);
                if (result.Any())
                {
                    var person = result.FirstOrDefault();

                    Session["UserName"] = person.UserId;
                    Session["FullName"] = person.FullName;
                    
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Erros = "Invalid login attempt";
            }
            return View();
        }

        [CustomAuthFilter]
        [HttpGet]
        public JsonResult SearchProfile(string userId)
        {
            var result = DataAccessLayer.IsValidUser(userId);
            string msg = "fail";
            if(result.Any())
            {
                msg = "success";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        
        [CustomAuthFilter]
        [ActionName("Profile")]
        public ActionResult UserProfile(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                Person person = DataAccessLayer.GetPersonAsync(userName);
                return View("UserProfile", person);
            }
            else
            {
                return Content("No valid user found");
            }
        }

        public ActionResult Follow(string followUserId)
        {
            DataAccessLayer.FollowUser(followUserId
                , (string)Session["UserName"]);

            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Person
                {
                    UserId = model.Username,
                    Pwd = model.Password,
                    FullName = model.FullName,
                    active = true,
                    Joined = DateTime.Now,
                    Email = model.Email
                };

                var result = await DataAccessLayer.CreatePersonAsync(user);

                if (result > 0)
                {
                    Session["UserName"] = model.Username;
                    Session["FullName"] = model.FullName;
                    return RedirectToAction("Index", "Home");
                }
                {
                    ViewBag.Errors = "Unable to Signup the user.";
                }
            }
            
            return View();
        }
        
        public ActionResult Signout()
        {
            Session["UserName"] = "";
            return RedirectToAction("Login", "Account");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
               
            }

            base.Dispose(disposing);
        }

        #region Helpers

      

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}