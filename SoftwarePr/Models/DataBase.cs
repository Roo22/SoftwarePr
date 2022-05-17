using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SoftwarePr.Models
{
    public class DataBase
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public List<AdminLogin> GetAdminData(AdminLogin model)
        {
            var dataAdmin = db.adminLogin.Where(s => s.Email.Equals(model.Email) && s.Password.Equals(model.Password)).ToList();
            return dataAdmin;
        }
        public List<Order> GetOrderData()
        {
            var dataOrder = db.orders.ToList<Order>();
            return dataOrder;
        }
        public List<InvoiceModel> GetInvoiceData()
        {
            var dataInvoice = db.invoiceModel.ToList<InvoiceModel>();
            return dataInvoice;
        }
        public IEnumerable<Products> GetProductsData()
        {
            var productsData = db.Products.ToList<Products>();
            return productsData;
        }
        public IEnumerable<ContactModel> GetContactsData()
        {
            var contactsData = db.contactModels.ToList<ContactModel>();
            return contactsData;
        }
        public IEnumerable<UserLoginSignUp> GetUserData(UserLoginSignUp model)
        {
            var UserData = db.SignupLogin.Where(s => s.Email.Equals(model.Email) && s.Password.Equals(model.Password)).ToList();
            return UserData;
        }
        public bool GetUserEmail(UserLoginSignUp signup)
        {
            var email = db.SignupLogin.Any(x => x.Email == signup.Email);
            return email;
        }
        public void AddProductsData(Products products)
        {
            db.Products.Add(products);
            db.SaveChanges();
        }
        public void ModifyProductsData(Products products)
        {
            db.Entry(products).State = EntityState.Modified;
            db.SaveChanges();
        }
        public Products FindProductNullableId(int? Id)
        {
            Products products = db.Products.Single(item => item.ProductId == Id);
            return products;
        }
        public void SaveContactModelData(ContactModel contact)
        {
            db.contactModels.Add(contact);
            db.SaveChanges();
        }
        public void SaveSignUpData(UserLoginSignUp signup)
        {
            db.SignupLogin.Add(signup);
            db.SaveChanges();
        }
        public void RemoveProductsData(Products products)
        {
            db.Products.Remove(products);
            db.SaveChanges();
        }
        public Products FindProductId(int id)
        {
            Products products = db.Products.Single(item => item.ProductId == id);
            return products;
        }
        public void SaveOrder(Order odr)
        {
            db.orders.Add(odr);
            db.SaveChanges();
        } public void SaveInvoice(InvoiceModel invoice)
        {
            db.invoiceModel.Add(invoice);
            db.SaveChanges();
        }
    }
}