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
    public class OrderController : Controller
    {
        private TDesignEntities db = new TDesignEntities();

        // GET: /Order/
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Address).Include(o => o.OrderStatu).Include(o => o.User);
            return View(orders.ToList());
        }

        // GET: /Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: /Order/Create
        public ActionResult Create()
        {
            ViewBag.ShipAddressID = new SelectList(db.Addresses, "AddressID", "AddressName");
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "OrderStatusID", "Name");
            ViewBag.CustomerID = new SelectList(db.Users, "UserID", "FirstName");
            return View();
        }

        // POST: /Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="OrderID,CustomerID,OrderDate,ShippedDate,ShipperID,Freight,ShipName,ShipAddressID,OrderStatusID,InBascet,CargoTrackingNo,isDeleted")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShipAddressID = new SelectList(db.Addresses, "AddressID", "AddressName", order.ShipAddressID);
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "OrderStatusID", "Name", order.OrderStatusID);
            ViewBag.CustomerID = new SelectList(db.Users, "UserID", "FirstName", order.CustomerID);
            return View(order);
        }

        // GET: /Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShipAddressID = new SelectList(db.Addresses, "AddressID", "AddressName", order.ShipAddressID);
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "OrderStatusID", "Name", order.OrderStatusID);
            ViewBag.CustomerID = new SelectList(db.Users, "UserID", "FirstName", order.CustomerID);
            return View(order);
        }

        // POST: /Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="OrderID,CustomerID,OrderDate,ShippedDate,ShipperID,Freight,ShipName,ShipAddressID,OrderStatusID,InBascet,CargoTrackingNo,isDeleted")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ShipAddressID = new SelectList(db.Addresses, "AddressID", "AddressName", order.ShipAddressID);
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "OrderStatusID", "Name", order.OrderStatusID);
            ViewBag.CustomerID = new SelectList(db.Users, "UserID", "FirstName", order.CustomerID);
            return View(order);
        }

        // GET: /Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: /Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
