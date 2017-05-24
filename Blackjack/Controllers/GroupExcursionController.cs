using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blackjack.Models;
using System.Data.Linq;
using System.Data.Linq.SqlClient;


namespace Blackjack.Controllers
{
    public class GroupExcursionController : Controller
    {
        // GET: GroupExcursion
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult ListAllMembers()
        {
            BlackjackDBEntities NE = new BlackjackDBEntities();

            //select * from members.
            List<Member> MemberList = NE.Members.ToList();

            ViewBag.MemberList = MemberList;

            //ViewBag.Cities = GetAllCities();
            //ViewBag.CustomerList = CustomerList;

            return View();
        }
        public ActionResult AddGroupMember()
        {
            return View();
        }

        public ActionResult SaveNewGroupMember(Member NewMember)
        {
            BlackjackDBEntities BE = new Models.BlackjackDBEntities();

            BE.Members.Add(NewMember);

            BE.SaveChanges();

            return RedirectToAction("ListAllMembers");

        }

        //public ActionResult DeleteMember(string email)
        //{
        //    if (email == null)
        //    {
        //        ViewBag.ErrorMessages = "Member has no email!!";
        //        return View("ErrorMessages");
        //    }
        //    try
        //    {
        //        BlackjackDBEntities NE = new Black();

        //        //1. Find the customer that I need to delete!

        //        Member ToDelete = NE.Members.Find(email);
        //        if (ToDelete == null)
        //        {
        //            ViewBag.ErrorMessage = "There is no one there to delete";
        //            return View("ErrorMessages");
        //        }

        //        //2. Remove the object from the list of Customers.
        //        NE.Members.Remove(ToDelete);

        //        //3. Perform the changes on the database.
        //        NE.SaveChanges();// save changes

        //        //ViewBag.Cities = GetAllCities();

        //        //return View("ListAllCustomers");

        //       // return RedirectToAction()

        //        return RedirectToAction("ListAllMembers", "GroupExcursion");
        //    }
        //    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
        //    {
        //        //log the exception!!!!!!

        //        ViewBag.ErrorMessage = "You cannot delete a Customer with orders.";

        //        return View("ErrorMessages");
        //    }

        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorMessage = "Catastrophe happened!!!!!!!!!";

        //        return View("ErrorMessages");
        //    }

        //}
    }
}