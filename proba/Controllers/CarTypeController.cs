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
    public class CarTypeController : Controller
    {
        MMEntities entity = new MMEntities();

        public ActionResult CarTypeIndex()
        {
            if (Session["LoggedAdminId"] != null)
                return View(entity.CarType.ToList());
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult CreateCarType()
        {
            if (Session["LoggedAdminId"] != null)
                return View();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult CreateCarType(CarType cat)
        {
            if (ModelState.IsValid && Session["LoggedAdminId"] != null)
            {
                entity.CarType.Add(cat);
                entity.SaveChanges();
                return RedirectToAction("CarTypeIndex", "CarType");
            }
            return View(cat);
        }

        public ActionResult EditCarType(int? id)
        {
            if (Session["LoggedAdminId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CarType carType = entity.CarType.Find(id);
                if (carType == null)
                {
                    return HttpNotFound();
                }
                return View(carType);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCarType([Bind(Include = "id, cartype1, carnameID")] CarType carType)
        {
            if (ModelState.IsValid)
            {
                entity.Entry(carType).State = EntityState.Modified;
                entity.SaveChanges();
                return RedirectToAction("CarTypeIndex", "CarType");
            }
            return View(carType);
        }

        public ActionResult DeleteCarType(int? id)
        {

            if (Session["LoggedAdminId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CarType carType = entity.CarType.Find(id);
                if (carType == null)
                {
                    return HttpNotFound();
                }
                return View(carType);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("DeleteCarType")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarType cat = entity.CarType.Find(id);
            entity.CarType.Remove(cat);
            entity.SaveChanges();
            return RedirectToAction("CarTypeIndex", "CarType");
        }

    }
}
