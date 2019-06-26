using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TwoDesign.Models;

namespace TwoDesign.Controllers
{
    public class BrandController : Controller
    {
        TDesignEntities db = new TDesignEntities();

        public ActionResult Index()
        {
            return View(db.Brands.ToList());
        }

        public ActionResult Insert()
        {

            return View(Tuple.Create<Brand, Image>(new Brand(), new Image()));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insert(Brand item1, Image item2, HttpPostedFileBase fluResim)
        {

            try
            {
                using (TransactionScope ts = new TransactionScope())
                {

                    if (ModelState.IsValid)
                    {
                        if (fluResim != null)
                        {
                            string guid = Guid.NewGuid().ToString().Replace('-', '_').ToLower() + "." + fluResim.ContentType.Split('/')[1];
                            fluResim.SaveAs(Server.MapPath(string.Format("~/Content/images/brand/{0}", guid)));
                            item2.ImagePath = guid;
                            db.Images.Add(item2);
                            bool sonuc = db.SaveChanges() > 0;
                            if (sonuc)
                            {
                                item1.ImageID = item2.ImageID;
                                db.Brands.Add(item1);
                                db.SaveChanges();
                                ts.Complete();
                                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
                                return RedirectToAction("Index", "Brand");
                            }
                            else
                            {
                                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
                            }
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

            return View(db.Brands.Find(id));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Brand item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(db.Brands.Find(item.BrandID)).CurrentValues.SetValues(item);
                bool sonuc = db.SaveChanges() > 0;
                if (sonuc)
                {
                    TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
                    return RedirectToAction("Index", "Brand");
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
            db.Entry(db.Brands.Find(id)).CurrentValues.SetValues(db.Brands.Find(id).isDeleted = true);
            bool sonuc = db.SaveChanges() > 0;
            if (sonuc)
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
            }
            else
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
            }

            return RedirectToAction("Index", "Brand");
        }
	}
}