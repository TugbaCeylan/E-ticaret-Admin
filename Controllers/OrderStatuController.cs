using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TwoDesign.Models;

namespace TwoDesign.Controllers
{
    public class OrderStatuController : Controller
    {
        private TDesignEntities db = new TDesignEntities();

        // GET: /OrderStatu/
        public ActionResult Index()
        {
            return View(db.OrderStatus.ToList());
        }

        // GET: /OrderStatu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderStatu orderstatu = db.OrderStatus.Find(id);
            if (orderstatu == null)
            {
                return HttpNotFound();
            }
            return View(orderstatu);
        }

        // GET: /OrderStatu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /OrderStatu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="OrderStatusID,Name,Description")] OrderStatu orderstatu)
        {
            if (ModelState.IsValid)
            {
                db.OrderStatus.Add(orderstatu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(orderstatu);
        }

        // GET: /OrderStatu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderStatu orderstatu = db.OrderStatus.Find(id);
            if (orderstatu == null)
            {
                return HttpNotFound();
            }
            return View(orderstatu);
        }

        // POST: /OrderStatu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="OrderStatusID,Name,Description")] OrderStatu orderstatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderstatu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orderstatu);
        }

        // GET: /OrderStatu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderStatu orderstatu = db.OrderStatus.Find(id);
            if (orderstatu == null)
            {
                return HttpNotFound();
            }
            return View(orderstatu);
        }

        // POST: /OrderStatu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderStatu orderstatu = db.OrderStatus.Find(id);
            db.OrderStatus.Remove(orderstatu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
