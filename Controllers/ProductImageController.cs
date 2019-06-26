using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwoDesign.Models;

namespace TwoDesign.Controllers
{
    public class ProductImageController : Controller
    {
        TDesignEntities db = new TDesignEntities();

        public ActionResult Index(int id)
        {
            return View(db.ProductImages.Where(pi => pi.ProductID == id).ToList());
        }

        public ActionResult Delete(int id)
        {
            db.Entry(db.ProductImages.Find(id)).CurrentValues.SetValues(db.ProductImages.Find(id).isDeleted = true);
            bool sonuc = db.SaveChanges() > 0;
            if (sonuc)
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
            }
            else
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
            }

            return RedirectToAction("Index", "ProductImage");
        }
	}
}