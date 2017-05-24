using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blackjack.Models;
using Microsoft.AspNet.Identity;

namespace Blackjack.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ApplicationDbContext USerInfo = new ApplicationDbContext();

            ApplicationUser info =USerInfo.Users.Find(User.Identity.GetUserId());


            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Main()

        {
            return View();
        }

    }
}