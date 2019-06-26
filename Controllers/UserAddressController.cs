using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TwoDesign.Models;

namespace TwoDesign.Controllers
{
    public class UserAddressController : Controller
    {
        TDesignEntities db = new TDesignEntities();

        public ActionResult Insert(int id)
        {

            return View(Tuple.Create<User, UserAddress>(db.Users.Find(id), new UserAddress()));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insert(UserAddress item, int id)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {

                    if (ModelState.IsValid)
                    {
                        item.UserID = id;
                        db.UserAddresses.Add(item);
                        bool sonuc = db.SaveChanges() > 0;
                        ts.Complete();
                        TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
                        return RedirectToAction("Index", "UserAddress");
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
            return View(db.UserAddresses.Find(id));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(UserAddress item)
        {

            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(db.UserAddresses.Find(item.UAddressID)).CurrentValues.SetValues(item);
                        bool sonuc = db.SaveChanges() > 0;
                        if (sonuc)
                        {
                            TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
                            ts.Complete();
                            return RedirectToAction("Index", "UserAddress");
                        }
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
            db.Entry(db.UserAddresses.Find(id)).CurrentValues.SetValues(db.Users.Find(id).IsDeleted = true);
            bool sonuc = db.SaveChanges() > 0;
            if (sonuc)
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
            }
            else
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
            }

            return RedirectToAction("Index", "UserAddress");
        }
    }
}