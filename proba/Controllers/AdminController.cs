using proba.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace proba.Controllers
{
    public class AdminController : Controller
    {
        public Controller controller;

        MMEntities entity = new MMEntities();

        [HttpGet]
        public ActionResult AdminLogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogIn(Admin admin)
        {
            if (ModelState.IsValid)
            {
                using (entity)
                {
                    var v = entity.Admin.Where(admin_user => admin_user.user_name.Equals(admin.user_name) && admin_user.password.Equals(admin.password)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["LoggedAdminId"] = v.id.ToString();
                        Session["LoggedAdminName"] = v.name;
                        return RedirectToAction("Index", "Admin");
                    }
                }
            }
            return View(admin);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Index()
        {
            if (Session["LoggedAdminId"] != null)
                return View();
            return RedirectToAction("AdminLogIn", "Admin");
        }
        public ActionResult Category()
        {
            if (Session["LoggedAdminId"] != null)
                return RedirectToAction("CategoryIndex", "Category");
            return RedirectToAction("AdminLogIn", "Admin");
        }

        public ActionResult Product()
        {
            if (Session["LoggedAdminId"] != null)
                return RedirectToAction("ProductIndex", "Product");
            return RedirectToAction("AdminLogIn", "Admin");
        }

        public ActionResult Customer()
        {
            if (Session["LoggedAdminId"] != null)
                return RedirectToAction("CustomerIndex", "Customer");
            return RedirectToAction("AdminLogIn", "Admin");
        }

        public ActionResult CarName()
        {
            if (Session["LoggedAdminId"] != null)
                return RedirectToAction("CarNameIndex", "CarName");
            return RedirectToAction("AdminLogIn", "Admin");
        }

        public ActionResult CarType()
        {
            if (Session["LoggedAdminId"] != null)
                return RedirectToAction("CarTypeIndex", "CarType");
            return RedirectToAction("AdminLogIn", "Admin");
        }

        public ActionResult Producer()
        {
            if (Session["LoggedAdminId"] != null)
                return RedirectToAction("ProducerIndex", "Producer");
            return RedirectToAction("AdminLogIn", "Admin");
        }
    }
}
