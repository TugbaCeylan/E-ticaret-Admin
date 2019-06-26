using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TwoDesign.Models;

namespace TwoDesign.Controllers
{
    public class ProductFeatureController : Controller
    {
        TDesignEntities db = new TDesignEntities();

        public ActionResult Index()
        {
            return View(db.ProductFeatures.ToList());
        }

        public ActionResult Insert()
        {
            ViewBag.ProductID = new SelectList(db.Products,"ProductID", "ProductName");

            return View();
        }

        [HttpPost]
        public ActionResult Insert(ProductFeature item)
        {
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", item.ProductID);
            if (ModelState.IsValid)
            {
                item.isDeleted = false;
                db.ProductFeatures.Add(item);
                bool sonuc = db.SaveChanges() > 0;
                if (sonuc)
                {
                    TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
                    return RedirectToAction("Index", "ProductFeature");
                }
                else
                {
                    TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
                }
            }
            else
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Val);
            }
            return View(item);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", id);
            return View(db.ProductFeatures.Find(id));
        }

        [HttpPost]
        public ActionResult Edit(ProductFeature item)
        {
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", item.ProductID);
            if (ModelState.IsValid)
            {
                db.Entry(db.ProductFeatures.Find(item.ProductFeatureID)).CurrentValues.SetValues(item);
                bool sonuc = db.SaveChanges() > 0;
                if (sonuc)
                {
                    TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
                    return RedirectToAction("Index", "ProductFeature");
                }
                else
                {
                    TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
                }
            }
            else
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Val);
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            db.Entry(db.ProductFeatures.Find(id)).CurrentValues.SetValues(db.ProductFeatures.Find(id).isDeleted = true);
            bool sonuc = db.SaveChanges() > 0;
            if (sonuc)
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
            }
            else
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
            }

            return RedirectToAction("Index", "ProductFeature");

        }
    }
}