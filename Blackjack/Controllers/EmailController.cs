using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using Blackjack.Models;

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
            //ExecuteTest().Wait();
            Execute();

            return View("MemberResponse");
        }

        static void Execute()//List<Member> memberList)
        {
            BlackjackDBEntities NE = new BlackjackDBEntities();

            List<Member> memberList = NE.Members.Where(x => x.ExcursionID.Equals(123456)).ToList();

            var client = new SendGridClient("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");
            SendGridMessage msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("lambrechtca@gmail.com", "BlackJack Team"));

            List<EmailAddress> recipients = new List<EmailAddress>();
            
                foreach (var member in memberList)
                {
                    EmailAddress email = new EmailAddress(member.Email , member.FirstName);
                    recipients.Add(email);
                }
                new EmailAddress("lambrechtca@gmail.com", "Charlie Lambrecht");
                new EmailAddress("csharpwebapplications@gmail.com", "Brian Wood");
                new EmailAddress("jkpaskus@gmail.com", "Jonas Paskus");
           

            msg.AddTos(recipients);

            msg.SetSubject("Testing the SendGrid API for BlackJack");

            //custom link back to website
            msg.AddContent(MimeType.Text, "Hello World plain text!");
            msg.AddContent(MimeType.Html, "<p>Hello World!</p>");

            client.SendEmailAsync(msg);
        }

        static async Task ExecuteV3()
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("lambrechtca@gmail.com", "Charlie");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("lambrechtca@gmail.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

         // using SendGrid's C# Library
         // https://github.com/sendgrid/sendgrid-csharp


            static async Task ExecuteTest()
            {
                //var apiKey = Environment.GetEnvironmentVariable("APPSETTING_SENDGRID_APIKEY");
                var client = new SendGridClient("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");
                var from = new EmailAddress("test@example.com", "Example User");
                var subject = "Sending with SendGrid is Fun";
                var to = new EmailAddress("lambrechtca@gmail.com", "Example User");
                var plainTextContent = "and easy to do anywhere, even with C#";
                var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
        }
    }