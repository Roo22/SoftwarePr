using SoftwarePr.InterFaces;
using SoftwarePr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftwarePr.Controllers
{
    public class UserController : Controller,IRedirectControllers, IRedirectUserControllers, ILogOut
    {
        ApplicationDbContext db = new ApplicationDbContext();
        DataBase Data = new DataBase();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Signup()
        {
            return RedirectToAnotherController();
        }
        public ActionResult RedirectToAnotherController()
        {
            var userInCookie = Request.Cookies["UserInfo"];
            if (userInCookie != null)
            {
                return RedirectToAction("Index", "Products");
            }
            else
            {

                return RedirectToAdminController();
            }
        }

        public ActionResult RedirectToAdminController()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(UserLoginSignUp signup)
        {
            if (ModelState.IsValid)
            {
                IfModelValid(signup);
            }
            return View();
        }
        public ActionResult IfEmailExists()
        {
            ViewBag.Message = "Email Already Registered. Please Try Again With Another Email";
            return View();
        }
       
        public ActionResult IfModelValid(UserLoginSignUp signup)
        {
            var isEmailAlreadyExists = Data.GetUserEmail(signup);
            if (isEmailAlreadyExists)
            {
                return IfEmailExists();
            }
            else
            {
                Data.SaveSignUpData(signup);
                return RedirectToAction("Index", "Products");
            }
        }
        [HttpGet]
        public ActionResult Login()
        {
            return RedirectToAnotherController();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginSignUp model)
        {
            var data = Data.GetUserData(model);
            return CheckData(data);
        }
       
        public ActionResult CheckData(IEnumerable<UserLoginSignUp> data)
        {
            if (data.Count() > 0)
            {
                CreateCookie(data);
                return RedirectToAction("Index", "Products");
            }
            else
            {
                ViewBag.Message = "Login failed";
                return RedirectToAction("Login");
            }
        }
        public void CreateCookie(IEnumerable<UserLoginSignUp> data)
        {
            Session["uid"] = data.FirstOrDefault().userId;
            HttpCookie cookie = new HttpCookie("UserInfo");
            cookie.Values["idUser"] = Convert.ToString(data.FirstOrDefault().userId);
            cookie.Values["FullName"] = Convert.ToString(data.FirstOrDefault().Name);
            cookie.Values["Email"] = Convert.ToString(data.FirstOrDefault().Email);
            cookie.Expires = DateTime.Now.AddMonths(1);
            Response.Cookies.Add(cookie);
        }
        public ActionResult Logout()
        {

            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("UserInfo"))
            {
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["UserInfo"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            Session.Clear();
            return RedirectToAction("Login");
        }

      
    }
}