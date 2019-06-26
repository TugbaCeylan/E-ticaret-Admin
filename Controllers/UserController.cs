using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TwoDesign.Models;

namespace TwoDesign.Controllers
{
    public class UserController : Controller
    {
        TDesignEntities db = new TDesignEntities();

        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult Insert()
        {
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName");
            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insert(User item)
        {
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", item.RoleID);
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    item.IsDeleted = false;
                    item.CreateDate = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.Users.Add(item);
                        bool sonuc = db.SaveChanges() > 0;
                        ts.Complete();
                        TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
                        return RedirectToAction("Index", "User");
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

            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", id);
            return View(db.Users.Find(id));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(User item)
        {
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", item.RoleID);
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(db.Users.Find(item.UserID)).CurrentValues.SetValues(item);
                        bool sonuc = db.SaveChanges() > 0;
                        if (sonuc)
                        {
                            TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
                            ts.Complete();
                            return RedirectToAction("Index", "User");
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
            db.Entry(db.Users.Find(id)).CurrentValues.SetValues(db.Users.Find(id).IsDeleted = true);
            bool sonuc = db.SaveChanges() > 0;
            if (sonuc)
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.OK);
            }
            else
            {
                TempData["Message"] = FxFonksiyon.GetInformation(MessageFormat.Err);
            }

            return RedirectToAction("Index", "User");
        }
	}
}