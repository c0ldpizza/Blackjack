using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blackjack.Models;

namespace Blackjack.Controllers
{
    public class ExcursionsController : Controller
    {
        private BlackjackDBEntities db = new BlackjackDBEntities();

        // GET: Excursions
        public ActionResult Index()
        {
            var excursions = db.Excursions.Include(e => e.AspNetUser);
            return View(excursions.ToList());
        }

        // GET: Excursions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Excursion excursion = db.Excursions.Find(id);
            if (excursion == null)
            {
                return HttpNotFound();
            }
            return View(excursion);
        }

        // GET: Excursions/Create
        public ActionResult Create()
        {
            ViewBag.LeadID = new SelectList(db.AspNetUsers, "Id", "FirstName");
            return View();
        }

        // POST: Excursions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Location,Date")] Excursion excursion) //Budget,LeadID,ExcursionID
        {
            AspNetUser leader = (AspNetUser)db.AspNetUsers.Where(x => x.UserName.Equals(User.Identity.Name)); // this is garbage
            excursion.LeadID = leader.Id;

            if (ModelState.IsValid)
            {
                db.Excursions.Add(excursion);
                db.SaveChanges();
                ViewBag.ExcursionID = excursion.ExcursionID;
                return RedirectToAction("../Members/Index");
            }

            ViewBag.LeadID = new SelectList(db.AspNetUsers, "Id", "FirstName", excursion.LeadID);
            return View(excursion);
        }

        // GET: Excursions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Excursion excursion = db.Excursions.Find(id);
            if (excursion == null)
            {
                return HttpNotFound();
            }
            ViewBag.LeadID = new SelectList(db.AspNetUsers, "Id", "FirstName", excursion.LeadID);
            return View(excursion);
        }

        // POST: Excursions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Location,Date,Budget,LeadID,ExcursionID")] Excursion excursion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(excursion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LeadID = new SelectList(db.AspNetUsers, "Id", "FirstName", excursion.LeadID);
            return View(excursion);
        }

        // GET: Excursions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Excursion excursion = db.Excursions.Find(id);
            if (excursion == null)
            {
                return HttpNotFound();
            }
            return View(excursion);
        }

        // POST: Excursions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Excursion excursion = db.Excursions.Find(id);
            db.Excursions.Remove(excursion);
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
