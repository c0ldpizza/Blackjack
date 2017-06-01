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
    public class ChoicesController : Controller
    {
        private BlackjackDBEntities db = new BlackjackDBEntities();

        // GET: Choices
        public ActionResult Index()
        {
            var choices = db.Choices.Include(c => c.Excursion);
            return View(choices.ToList());
        }

        // GET: Choices/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Choice choice = db.Choices.Find(id);
            if (choice == null)
            {
                return HttpNotFound();
            }
            return View(choice);
        }

        // GET: Choices/Create
        public ActionResult Create()
        {
            ViewBag.ExcursionID = new SelectList(db.Excursions, "ExcursionID", "Location");
            return View();
        }

        // POST: Choices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Genre,URL,Image,IsFinal,ChoiceID,ExcursionID,ChoiceName,Votes")] Choice choice)
        {
            if (ModelState.IsValid)
            {
                db.Choices.Add(choice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ExcursionID = new SelectList(db.Excursions, "ExcursionID", "Location", choice.ExcursionID);
            return View(choice);
        }

        // GET: Choices/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Choice choice = db.Choices.Find(id);
            if (choice == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExcursionID = new SelectList(db.Excursions, "ExcursionID", "Location", choice.ExcursionID);
            return View(choice);
        }

        // POST: Choices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Genre,URL,Image,IsFinal,ChoiceID,ExcursionID,ChoiceName,Votes")] Choice choice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(choice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExcursionID = new SelectList(db.Excursions, "ExcursionID", "Location", choice.ExcursionID);
            return View(choice);
        }

        // GET: Choices/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Choice choice = db.Choices.Find(id);
            if (choice == null)
            {
                return HttpNotFound();
            }
            return View(choice);
        }

        // POST: Choices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Choice choice = db.Choices.Find(id);
            db.Choices.Remove(choice);
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

        public ActionResult Vote(int excID)
        {
            List<Choice> ChoiceList = db.Choices.Where(x => x.ExcursionID.Equals(excID)).ToList();
            ViewBag.ChoiceList = ChoiceList;
            return View();
        }
    }
}
