using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blackjack.Models;
using Microsoft.AspNet.Identity;
namespace Blackjack.Controllers
{
    public class ExcursionsController : Controller
    {
        private BlackjackDBEntities db = new BlackjackDBEntities();

        // GET: Excursions
        [Authorize]
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId(); 
            var excursions = db.Excursions.Where(e => e.LeadID == userID );
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

        public ActionResult SearchCity(string SearchExcursion)
        {
            BlackjackDBEntities NE = new BlackjackDBEntities();

            List<Excursion> ExcursionList = NE.Excursions.Where(x => x.ExcursionID.Equals(SearchExcursion.ToUpper())).ToList();

            ViewBag.ExcursionList = ExcursionList;

            return View("Index");
        }

        // POST: Excursions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "Location,Date,Budget,LeadID,ExcursionID")] Excursion newExcursion)
        {

            if (ModelState.IsValid)
            {
                newExcursion.LeadID = User.Identity.GetUserId();

                db.Excursions.Add(newExcursion);
                db.SaveChanges();

                ViewBag.NewExcursion = newExcursion;

                return RedirectToAction("Create", "Members");

            }

            ViewBag.LeadID = new SelectList(db.AspNetUsers, "Id", "FirstName", newExcursion.LeadID);
            return View(newExcursion);
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
