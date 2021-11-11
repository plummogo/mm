using proba.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace proba.Controllers
{
    public class CustomerController : Controller
    {
        MMEntities entity = new MMEntities();
        public ActionResult CustomerIndex()
        {
            if (Session["LoggedAdminId"] != null)
                return View(entity.Customer.ToList());
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EditCustomer(int? id)
        {

            if (Session["LoggedAdminId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Customer cust = entity.Customer.Find(id);
                if (cust == null)
                {
                    return HttpNotFound();
                }
                return View(cust);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer(Customer cust)
        {
            if (ModelState.IsValid)
            {
                entity.Entry(cust).State = EntityState.Modified;
                entity.SaveChanges();
                return RedirectToAction("CustomerIndex", "Customer");
            }
            return View(cust);
        }

        public ActionResult DeleteCustomer(int? id)
        {

            if (Session["LoggedAdminId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Customer cust = entity.Customer.Find(id);
                if (cust == null)
                {
                    return HttpNotFound();
                }
                return View(cust);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("DeleteCustomer")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer cust = entity.Customer.Find(id);
            entity.Customer.Remove(cust);
            entity.SaveChanges();
            return RedirectToAction("CustomerIndex", "Customer");
        }
    }
}
