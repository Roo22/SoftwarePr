using SoftwarePr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftwarePr.Controllers
{
    public class HomePageTestController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: HomePageTest
        public ActionResult Index()
        {
            var products = GetProducts();
            return View(products);


        }
        public IEnumerable<Products> GetProducts()
        {
            var products = db.Products.ToList<Products>();
            return products;
        }
    }
}