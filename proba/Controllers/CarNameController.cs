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
    public class CarNameController : Controller
    {
        MMEntities entity = new MMEntities();

        public ActionResult CarNameIndex()
        {
            if (Session["LoggedAdminId"] != null)
                return View(entity.CarName.ToList());
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult CreateCarName()
        {
            if (Session["LoggedAdminId"] != null)
                return View();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult CreateCarName(CarName cat)
        {
            if (ModelState.IsValid && Session["LoggedAdminId"] != null)
            {
                entity.CarName.Add(cat);
                entity.SaveChanges();
                return RedirectToAction("CarNameIndex", "CarName");
            }
            return View(cat);
        }

        public ActionResult EditCarName(int? id)
        {

            if (Session["LoggedAdminId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CarName CarName = entity.CarName.Find(id);
                if (CarName == null)
                {
                    return HttpNotFound();
                }
                return View(CarName);
            }

            return RedirectToAction("AdminLogIn", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCarName([Bind(Include = "id, carname1")] CarName carName)
        {
            if (ModelState.IsValid)
            {
                entity.Entry(carName).State = EntityState.Modified;
                entity.SaveChanges();
                return RedirectToAction("CarNameIndex", "CarName");
            }
            return View(carName);
        }

        public ActionResult DeleteCarName(int? id)
        {
            if (Session["LoggedAdminId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CarName carName = entity.CarName.Find(id);
                if (carName == null)
                {
                    return HttpNotFound();
                }
                return View(carName);
            }
            return RedirectToAction("AdminLogIn", "Admin");
        }

        [HttpPost, ActionName("DeleteCarName")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarName cat = entity.CarName.Find(id);
            entity.CarName.Remove(cat);
            entity.SaveChanges();
            return RedirectToAction("CarNameIndex", "CarName");
        }
    }
}
