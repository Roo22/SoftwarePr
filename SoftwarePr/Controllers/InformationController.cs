using SoftwarePr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftwarePr.Controllers
{
    public class InformationController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Information
        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(ContactModel contact)
        {
            SaveContactModelData(contact);
            return View();
        }
        public void SaveContactModelData(ContactModel contact)
        {
            db.contactModels.Add(contact);
            db.SaveChanges();
        }
        public ActionResult MessageList()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                var contacts = GetContacts();
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
        public IEnumerable<ContactModel> GetContacts()
        {
            var contacts = db.contactModels.ToList<ContactModel>();
            return contacts;
        }
    }


}