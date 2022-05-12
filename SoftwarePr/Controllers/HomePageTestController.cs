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
        DataBase Data = new DataBase();
        // GET: HomePageTest
        public ActionResult Index()
        {
            var products = Data.GetProductsData();
            return View(products);


        }
        
    }
}