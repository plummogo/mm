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
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        MMEntities entity = new MMEntities();

        public ActionResult ProductIndex()
        {
            if (Session["LoggedAdminId"] != null)
                return View(entity.Product.ToList());
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult CreateProduct()
        {

            if (Session["LoggedAdminId"] != null)
                return View();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult CreateProduct(Product pro)
        {

            if (Session["LoggedAdminId"] != null)
            {
                if (ModelState.IsValid)
                {
                    entity.Product.Add(pro);
                    entity.SaveChanges();
                    return RedirectToAction("ProductIndex", "Product");
                }
                return View(pro);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = entity.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(Product product)
        {

            if (Session["LoggedAdminId"] != null)
            {
                product.CarsProducers.id = product.carsproducersid;
                product.CarsProducers.cartypeid = product.CarsProducers.CarType.id;
                product.CarsProducers.producerid = product.CarsProducers.Producer.id;
                product.CarsProducers.Producer.cat_id = product.CarsProducers.Producer.Category.id;
                product.CarsProducers.CarType.CarName.id = product.CarsProducers.CarType.carnameID;
                if (ModelState.IsValid)
                {
                    entity.Entry(product).State = EntityState.Modified;
                    entity.SaveChanges();
                    return RedirectToAction("ProductIndex", "Product");
                }
                return View(product);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DeleteProduct(int? id)
        {

            if (Session["LoggedAdminId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = entity.Product.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = entity.Product.Find(id);
            entity.Product.Remove(product);
            entity.SaveChanges();
            return RedirectToAction("ProductIndex", "Product");
        }
    }
}
