using SoftwarePr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftwarePr.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {

            return RedirectingIndex();
        }
        public ActionResult RedirectingIndex()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                return View();
            }
            else
            {
                return RedirectToAnotherController();
            }
        }
        [HttpGet]
        public ActionResult LoginAdmin()
        {

            return RedirectingLoginAdmin();
        }
        public ActionResult RedirectingLoginAdmin()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                return RedirectToAction("Index", "Admin"); ;
            }
            else
            {
                return RedirectToProductsController();
            }
        }
        public ActionResult RedirectToProductsController()
        {
            var userInCookie = Request.Cookies["UserInfo"];
            if (userInCookie != null)
            {
                return RedirectToAction("Index", "Products");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginAdmin(AdminLogin model)
        {
            var data = GetAdminData(model);
            return CheckData(data);
        }
        public List<AdminLogin> GetAdminData(AdminLogin model)
        {
            var data = db.adminLogin.Where(s => s.Email.Equals(model.Email) && s.Password.Equals(model.Password)).ToList();
            return data;
        }
        public ActionResult CheckData(List<AdminLogin> data)
        {
            if (data.Count() > 0)
            {
                CreateCookie(data);
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.Message = "Login failed";
                return RedirectToAction("LoginAdmin");
            }
        }
        public void CreateCookie(List<AdminLogin> data)
        {
            HttpCookie cookie = new HttpCookie("AdminInfo");
            cookie.Values["idAdmin"] = Convert.ToString(data.FirstOrDefault().adminid);
            cookie.Values["Email"] = Convert.ToString(data.FirstOrDefault().Email);
            cookie.Expires = DateTime.Now.AddMonths(1);
            Response.Cookies.Add(cookie);
        }
        public ActionResult LogoutAdmin()
        {
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("AdminInfo"))
            {
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["AdminInfo"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            return RedirectToAction("LoginAdmin");
        }

        public ActionResult ListOfOrders()
        {
            return CheckAdminNullOrders();
        }
        public ActionResult CheckAdminNullOrders()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                var OrderTotal = GetOrderTotal();
                return View(OrderTotal);
            }
            else
            {
                return RedirectToAnotherController();
            }
        }
        public List<Order> GetOrderTotal()
        {
            float sum = 0;
            List<Order> order = db.orders.ToList<Order>();
            foreach (var item in order)
            {
                sum += item.Order_Bill;
            }
            TempData["OrderTotal"] = sum;
            return order;

        }
        public ActionResult ListOfInvoices()
        {
            return CheckAdminNullInvoice();
        }
        public ActionResult CheckAdminNullInvoice()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];

            if (adminInCookie != null)
            {
                var InvoiceTotal = GetTotalBills();
                return View(InvoiceTotal);
            }
            else
            {
                return RedirectToAnotherController();

            }
        }
        public List<InvoiceModel> GetTotalBills()
        {
            float sum = 0;
            List<InvoiceModel> invoice = db.invoiceModel.ToList<InvoiceModel>();
            foreach (var item in invoice)
            {
                sum += item.Total_Bill;


            }
            TempData["InvoiceTotal"] = sum;
            return invoice;
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
                return RedirectToAction("LoginAdmin", "Admin");
            }
        }
    }
}