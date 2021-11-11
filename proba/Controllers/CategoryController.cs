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
    public class CategoryController : Controller
    {
        //GET: /Admin/Category
        //műveletek a category adattáblával

        MMEntities entity = new MMEntities();

        public ActionResult CategoryIndex()
        {
            if (Session["LoggedAdminId"] != null)
                return View(entity.Category.ToList());
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {
            if (Session["LoggedAdminId"] != null)
                return View();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult CreateCategory(Category cat)
        {
            if (ModelState.IsValid && Session["LoggedAdminId"] != null)
            {
                entity.Category.Add(cat);
                entity.SaveChanges();
                return RedirectToAction("CategoryIndex","Category");
            }
            return View(cat);
        }

        public ActionResult EditCategory(int? id)
        {

            if (Session["LoggedAdminId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Category cat = entity.Category.Find(id);
                if (cat == null)
                {
                    return HttpNotFound();
                }
                return View(cat);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult EditCategory([Bind(Include = "id,categoryname,Product")] Category cat)
        {
            if (ModelState.IsValid)
            {
                entity.Entry(cat).State = EntityState.Modified;
                entity.SaveChanges();
                return RedirectToAction("CategoryIndex","Category");
            }
            return View(cat);
        }

        public ActionResult DeleteCategory(int? id)
        {

            if (Session["LoggedAdminId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Category cat = entity.Category.Find(id);
                if (cat == null)
                {
                    return HttpNotFound();
                }
                return View(cat);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category cat = entity.Category.Find(id);
            entity.Category.Remove(cat);
            entity.SaveChanges();
            return RedirectToAction("CategoryIndex","Category");
        }
    }
}
