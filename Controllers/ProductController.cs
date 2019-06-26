using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TwoDesign.Models;

namespace TwoDesign.Controllers
{
    public class ProductController : Controller
    {
        TDesignEntities db = new TDesignEntities();

        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }


        public ActionResult Insert()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insert(Product item, List<HttpPostedFileBase> fluResim)
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", item.CategoryID);
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", item.BrandID);

            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                   
                    item.CreateDate = DateTime.Now;
                    item.isDeleted = false;

                    if (ModelState.IsValid)
                    {
                        db.Products.Add(item);
                        bool sonuc = db.SaveChanges() > 0;
                        if (sonuc)
                        {
                            foreach (HttpPostedFileBase img in fluResim)
                            {
                                ProductImage pi = new ProductImage();
                                pi.ProductID = item.ProductID;
                                bool yuklemeSonucu;
                                string path = FxFonksiyon.ImageUpload(img, "products", out yuklemeSonucu);
                                if (yuklemeSonucu)
                                {
                                    pi.ImagePath = path;
                                }
                                db.ProductImages.Add(pi);
                                db.SaveChanges();
                            }
                            ts.Complete();
                            TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
                            return RedirectToAction("Index", "Product");
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
                }
            }
            catch (Exception)
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", id);
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", id);
            return View(db.Products.Find(id));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Product item, List<HttpPostedFileBase> fluResim)
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", item.CategoryID);
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", item.BrandID);
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(db.Products.Find(item.ProductID)).CurrentValues.SetValues(item);
                        bool sonuc = db.SaveChanges() > 0;
                        if (sonuc)
                        {
                            if (fluResim.Count > 0)
                            {
                                foreach (HttpPostedFileBase img in fluResim)
                                {
                                    ProductImage pi = new ProductImage();
                                    pi.ProductID = item.ProductID;
                                    bool yuklemeSonucu;
                                    string path = FxFonksiyon.ImageUpload(img, "products", out yuklemeSonucu);
                                    if (yuklemeSonucu)
                                    {
                                        pi.ImagePath = path;
                                    }
                                    db.ProductImages.Add(pi);
                                    db.SaveChanges();
                                }
                            }
                        }

                        TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
                        ts.Complete();
                        return RedirectToAction("Index", "Product");
                    }

                    else
                    {
                        TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Val);
                    }
                }
            }
            catch
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            db.Entry(db.Products.Find(id)).CurrentValues.SetValues(db.Products.Find(id).isDeleted = true);
            bool sonuc = db.SaveChanges() > 0;
            if (sonuc)
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
            }
            else
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
            }

            return RedirectToAction("Index", "Product");
        }

	}
}