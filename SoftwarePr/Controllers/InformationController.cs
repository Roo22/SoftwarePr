using SoftwarePr.InterFaces;
using SoftwarePr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftwarePr.Controllers
{
    public class InformationController : Controller, IRedirectControllers
    {
        ApplicationDbContext db = new ApplicationDbContext();
        DataBase Data = new DataBase();
        // GET: Information
        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(ContactModel contact)
        {
            Data.SaveContactModelData(contact);
            return View();
        }
        
        public ActionResult MessageList()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                var contacts = Data.GetContactsData();
                return View(contacts);

            }
            else
            {
                return RedirectToAnotherController();
            }
        }
        public ActionResult RedirectToAnotherController()
        {
            var userInCookie = Request.Cookies["UserInfo"];
            if (userInCookie != null)
            {
                return RedirectToAction("Products", "Index");

            }
            else
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
        }
        public ActionResult AboutUs()
        {

            return View();
        }
        public ActionResult Blogs()
        {

            return View();
        }
        
    }


}