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
    public class MembersController : Controller
    {
        private BlackjackDBEntities db = new BlackjackDBEntities();


        // GET: Members
        public ActionResult Index(int? id)
        {

            if (id.HasValue)
            { 
            Excursion temp = new Excursion();
            temp.ExcursionID = id.Value;
            List<Member> members = db.Members.Include(e => e.Excursions).ToList().Where(e => e.ExcursionID == id).ToList();
                ViewBag.excID = id.Value;

            return View(members);
            }

            return View("../Home/Main");
        }

        // GET: Members/Details/5
        public ActionResult Details(int id)
        {
            if (id.Equals(null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create(int? excID)
        {
            ViewBag.ExcursionID = excID.Value;
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Email")] Member member, int? excID) 
        {
            member.ExcursionID = excID.Value;

            ViewBag.ExcursionID = member.ExcursionID;

            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = member.ExcursionID });
            }

            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int id)
        {
            if (id.Equals(null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FirstName,LastName,Email,ExcursionID")] Member member, int id)
        {
            member.MemberID = id;
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = member.ExcursionID });
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int id)
        {
            if (id.Equals(null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = member.ExcursionID });
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
