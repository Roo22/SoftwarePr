﻿using SoftwarePr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftwarePr.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

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
        public ActionResult Signup(SignupLogin signup)
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
        public void SaveSignUpData(SignupLogin signup)
        {
            db.SignupLogin.Add(signup);
            db.SaveChanges();
        }
        public ActionResult IfModelValid(SignupLogin signup)
        {
            var isEmailAlreadyExists = db.SignupLogin.Any(x => x.Email == signup.Email);
            if (isEmailAlreadyExists)
            {
                return IfEmailExists();
            }
            else
            {
                SaveSignUpData(signup);
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
        public ActionResult Login(SignupLogin model)
        {
            var data = GetUserData(model);
            return CheckData(data);
        }
        public IEnumerable<SignupLogin> GetUserData(SignupLogin model)
        {
            var data = db.SignupLogin.Where(s => s.Email.Equals(model.Email) && s.Password.Equals(model.Password)).ToList();
            return data;
        }
        public ActionResult CheckData(IEnumerable<SignupLogin> data)
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
        public void CreateCookie(IEnumerable<SignupLogin> data)
        {
            Session["uid"] = data.FirstOrDefault().userid;
            HttpCookie cookie = new HttpCookie("UserInfo");
            cookie.Values["idUser"] = Convert.ToString(data.FirstOrDefault().userid);
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