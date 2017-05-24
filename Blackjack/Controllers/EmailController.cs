using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Blackjack.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendEmail()
        {
            SendGridMessage msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("dx@example.com", "SendGrid DX Team"));

            List<EmailAddress> recipients = new List<EmailAddress>
            {
                //foreach loop for Members
                new EmailAddress("lambrechtca@gmail.com", "Charlie Lambrecht"),
                new EmailAddress("csharpwebapplications@gmail.com", "Brian Wood"),
                new EmailAddress("jkpaskus@gmail.com", "Jonas Paskus")
            };
            msg.AddTos(recipients);

            msg.SetSubject("Testing the SendGrid API for BlackJack");

            //custom link back to website
            msg.AddContent(MimeType.Text, "Hello World plain text!");
            msg.AddContent(MimeType.Html, "<p>Hello World!</p>");

            return View("MemberResponse");
        }
    }
}