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
    public class ProducerController : Controller
    {
        MMEntities entity = new MMEntities();

        public ActionResult ProducerIndex()
        {
            if (Session["LoggedAdminId"] != null)
                return View(entity.Producer.ToList());
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult CreateProducer()
        {
            if (Session["LoggedAdminId"] != null)
                return View();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult CreateProducer(Producer cat)
        {
            if (ModelState.IsValid && Session["LoggedAdminId"] != null)
            {
                entity.Producer.Add(cat);
                entity.SaveChanges();
                return RedirectToAction("ProducerIndex", "Producer");
            }
            return View(cat);
        }

        public ActionResult EditProducer(int? id)
        {

            if (Session["LoggedAdminId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Producer Producer = entity.Producer.Find(id);
                if (Producer == null)
                {
                    return HttpNotFound();
                }
                return View(Producer);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProducer(Producer producer)
        {
            producer.cat_id = producer.Category.id;
            if (ModelState.IsValid)
            {
                producer.CarsProducers = entity.CarsProducers.Where(cp => cp.producerid.Equals(producer.id)).ToList();
                entity.Entry(producer).State = EntityState.Modified;
                entity.SaveChanges();
                return RedirectToAction("ProducerIndex", "Producer");
            }
            return View(producer);
        }

        public ActionResult DeleteProducer(int? id)
        {

            if (Session["LoggedAdminId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Producer Producer = entity.Producer.Find(id);
                if (Producer == null)
                {
                    return HttpNotFound();
                }
                return View(Producer);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("DeleteProducer")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producer cat = entity.Producer.Find(id);
            entity.Producer.Remove(cat);
            entity.SaveChanges();
            return RedirectToAction("ProducerIndex", "Producer");
        }
    }
}
