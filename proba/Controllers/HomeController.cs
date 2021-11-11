using proba.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace proba.Controllers
{
    public class HomeController : Controller
    {

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult AboutUs()
        {
            return View();
        }

        public ViewResult Contact()
        {
            return View();
        }

        public ViewResult Gallery()
        {
            return View();
        }

        public ViewResult Webshop()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        MMEntities entity = new MMEntities();
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Customer customer)
        {
            if (ModelState.IsValid)
            {
                using (entity)
                {
                    var v = entity.Customer.Where(user => user.username.Equals(customer.username) && user.password.Equals(customer.password)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["LoggedId"] = v.id.ToString();
                        Session["LoggedName"] = v.name; 
                        FormsAuthentication.SetAuthCookie(customer.username, false);
                        return RedirectToAction("Index","Home");
                    }
                }
            }
            ModelState.AddModelError(string.Empty,"Rossz jelszó vagy felhasználónév");
            return View(customer);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Customer customer)
        {
            if(ModelState.IsValid)
            {
                entity.Customer.Add(customer);
                entity.SaveChanges();
                return RedirectToAction("Login");
            }
            ModelState.AddModelError(string.Empty, "Rosszul kitöltött adatok!");
            return View(customer);
        }

        public ActionResult CartContent()
        {
            return View(entity.Cart.ToList().Where(customer => customer.customerId.Equals(int.Parse(Session["LoggedId"].ToString()))));
        }
    }
}
