using SoftwarePr.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftwarePr.Controllers
{
    public class ProductsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public IEnumerable<Products> GetProducts()
        {
            var products = db.Products.ToList<Products>();
            return products;
        }
        public ActionResult Index()
        {
            var products = GetProducts();
            return View(products);

        }
        public ActionResult Details(int id)
        {
            Products products = FindProductId(id);
            return View(products);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Products products = FindProductId(id);
            RemoveProductsData(products);
            return RedirectToAction("Index", "Admin");
        }
        public Products FindProductId(int id)
        {
            Products products = db.Products.Find(id);
            return products;
        }
        public void RemoveProductsData(Products products)
        {
            db.Products.Remove(products);
            db.SaveChanges();
        }
        [HttpGet]
        public ActionResult CreateNewProduct()
        {
            return CheckCookieNullorNot();

        }
        public ActionResult CheckCookieNullorNot()
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
        [HttpPost]
        public ActionResult CreateNewProduct(HttpPostedFileBase file, Products products)
        {
            if (CheckImage(file))
                try
                {
                    SaveImageFile(products, file);
                    ViewBag.Message = "File uploaded successfully";

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View();
        }
        public bool CheckImage(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                return true;
            else
                return false;

        }
        public void SaveImageFile(Products products, HttpPostedFileBase file)
        {
            file = SaveFilePath(file);
            products = SaveProductPictureData(products, file);
            AddProductsData(products);
        }
        public Products SaveProductPictureData(Products products, HttpPostedFileBase file)
        {
            string filename = file.FileName;
            products.ProductPicture = "Images/" + filename;
            return products;

        }
        public HttpPostedFileBase SaveFilePath(HttpPostedFileBase file)
        {
            string path = Path.Combine(Server.MapPath("~/Images"),
            Path.GetFileName(file.FileName));
            file.SaveAs(path);
            return file;
        }
        public void AddProductsData(Products products)
        {
            db.Products.Add(products);
            db.SaveChanges();
        }
        [HttpGet]
        public ActionResult EditProduct(int id)
        {

            return CheckAdminCookieNullorNot(id);
        }
        public ActionResult CheckAdminCookieNullorNot(int id)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                Products products = FindProductId(id);
                return ifProductNullorNot(products);
            }
            else
            {
                return RedirectToAnotherController();
            }
        }
        public ActionResult ifProductNullorNot(Products products)
        {
            if (products == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(products);
            }
        }
        [HttpPost]
        public ActionResult EditProduct(HttpPostedFileBase file, Products products)
        {
            if (CheckImage(file))
                try
                {
                    ModifyImageFile(products, file);
                    ViewBag.Message = "File uploaded successfully";

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("ViewProductsAdmin", "Products");
        }
        public void ModifyImageFile(Products products, HttpPostedFileBase file)
        {
            file = SaveFilePath(file);
            products = SaveProductPictureData(products, file);
            ModifyProductsData(products);
        }

        public void ModifyProductsData(Products products)
        {
            db.Entry(products).State = EntityState.Modified;
            db.SaveChanges();
        }
        public ActionResult ViewProductsAdmin()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                var products = GetProducts();
                return View(products);
            }
            else
            {
                return RedirectToAnotherController();
            }

        }

        public ActionResult addToCart(int? Id)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return CheckIfUserCookieNullorNot(Id);
            }

        }
        public ActionResult CheckIfUserCookieNullorNot(int? Id)
        {
            var userInCookie = Request.Cookies["UserInfo"];
            if (userInCookie != null)
            {

                return FindProductId(Id);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        public ActionResult FindProductId(int? Id)
        {
            Products products = db.Products.Find(Id);
            return View(products);
        }


        [HttpPost]
        public ActionResult addToCart(int Id, string number)
        {
            List<Cart> CartList = new List<Cart>();
            var userInCookie = Request.Cookies["UserInfo"];
            if (userInCookie != null)
            {
                Products products = db.Products.Find(Id);
                Cart cart = new Cart();
                cart = AddProductDataToCart(Id, products, cart, number);
                if (TempData["cart"] == null)
                {
                    CartList.Add(cart);
                    TempData["cart"] = CartList;
                }
                else
                {

                    List<Cart> CartList2 = TempData["cart"] as List<Cart>;
                    int flag = 0;
                    IncreaseProductinCart(CartList2, cart, ref flag);
                    if (flag == 0)
                    {
                        CartList2.Add(cart);
                    }
                    TempData["cart"] = CartList2;
                }

                TempData.Keep();
                return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        public List<Cart> IncreaseProductinCart(List<Cart> CartList, Cart cart, ref int flag)
        {
            foreach (var item in CartList)
            {
                if (item.productId == cart.productId)
                {
                    item.qty += cart.qty;
                    item.bill += cart.bill;
                    flag = 1;
                }
            }
            return CartList;
        }
        public Cart AddProductDataToCart(int Id, Products products, Cart cart, string number)
        {

            cart.productId = products.id;
            cart.productName = products.ProductName;
            cart.productPic = products.ProductPicture;
            cart.price = products.ProductPrice;
            cart.qty = Convert.ToInt32(number);
            cart.bill = cart.price * cart.qty;
            return cart;
        }
        public ActionResult Checkout()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {

                    return IfUserCookienotNull();
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
        }
        public ActionResult IfUserCookienotNull()
        {
            TempData.Keep();
            if (TempData["cart"] != null)
            {

                List<Cart> CartList = TempData["cart"] as List<Cart>;
                var TotalBill = GetTotalBill(CartList);
                TempData["total"] = TotalBill;
            }
            TempData.Keep();
            return View();
        }
        public float GetTotalBill(List<Cart> CartList)
        {
            float TotalBill = 0;
            foreach (var item in CartList)
            {
                TotalBill += item.bill;
            }
            return TotalBill;
        }
        [HttpPost]
        public ActionResult Checkout(Order order)
        {
            var userInCookie = Request.Cookies["UserInfo"];
            int iduser = Convert.ToInt32(userInCookie["idUser"]);
            List<Cart> CartList = TempData["cart"] as List<Cart>;
            InvoiceModel invoice = new InvoiceModel();
            invoice = AddDataToInvoiceModel(iduser, invoice);
            AddOrderFromCart(CartList, invoice);
            TempData.Remove("total");
            TempData.Remove("cart");
            TempData.Keep();
            return RedirectToAction("Index");
        }

        public void AddOrderFromCart(List<Cart> CartList, InvoiceModel invoice)
        {
            foreach (var item in CartList)
            {
                Order odr = new Order();
                odr.FkProdId = item.productId;
                odr.FkInvoiceID = invoice.ID;
                odr.Order_Date = System.DateTime.Now;
                odr.Qty = item.qty;
                odr.Unit_Price = (int)item.price;
                odr.Order_Bill = item.bill;
                db.orders.Add(odr);
                db.SaveChanges();
            }
        }
        public InvoiceModel AddDataToInvoiceModel(int iduser, InvoiceModel invoice)
        {

            invoice.FKUserID = iduser;
            invoice.DateInvoice = System.DateTime.Now;
            invoice.Total_Bill = (float)TempData["Total"];
            db.invoiceModel.Add(invoice);
            db.SaveChanges();
            return invoice;
        }
        public ActionResult Remove(int? id)
        {
            List<Cart> CartList = TempData["cart"] as List<Cart>;
            CartList = RemoveCart(CartList, id);
            return GotToCheckout(CartList);
        }
        public List<Cart> RemoveCart(List<Cart> CartList, int? id)
        {
            Cart cart = CartList.Where(x => x.productId == id).SingleOrDefault();
            CartList.Remove(cart);
            return CartList;
        }
        public ActionResult GotToCheckout(List<Cart> CartList)
        {
            var TotalBill = GetTotalBill(CartList);
            TempData["total"] = TotalBill;
            return RedirectToAction("Checkout");
        }
    }
}