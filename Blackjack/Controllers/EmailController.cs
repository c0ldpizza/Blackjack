using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using Blackjack.Models;
using System.Web.Configuration;

namespace Blackjack.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Index()
        {
            return View();
        }

        public static void SendEmail(int excursionID)//List<Member> memberList)
        {
            BlackjackDBEntities NE = new BlackjackDBEntities();

            List<Member> memberList = NE.Members.Where(x => x.ExcursionID.Equals(excursionID)).ToList();

            string SendGridAPIkey = WebConfigurationManager.AppSettings["SendGridKey"];

            var client = new SendGridClient(SendGridAPIkey);

            SendGridMessage msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("lambrechtca@gmail.com", "BlackJack Team"));

            List<EmailAddress> recipients = new List<EmailAddress>();
            
                foreach (var member in memberList)
                {
                    EmailAddress email = new EmailAddress(member.Email , member.FirstName);
                    recipients.Add(email);
                }

            msg.AddTos(recipients);

            msg.SetSubject("BlackJack Group Vote");

            string urlLink = "http://webappjkp.azurewebsites.net/Choices/Vote?excID=" + excursionID.ToString();
            List<Content> content = new List<Content>();
            Content urlLinkContent = new Content("text/html", urlLink);

            content.Add(urlLinkContent);

            msg.AddContents(content);
            //custom link back to website
            msg.AddContent(MimeType.Text, "Hello world");
            msg.AddContent(MimeType.Html, "<h2>Thanks for choosing BlackJack!</h2>");

            


            client.SendEmailAsync(msg);
        }




        
    }
}
    