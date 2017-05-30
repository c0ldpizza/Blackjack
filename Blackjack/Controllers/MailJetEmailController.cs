using Mailjet.Client;
using Mailjet.Client.Resources;
using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace Blackjack.Controllers
{
    public class MailJetEmailController : Controller
    {
        /// 
        /// This calls sends a message to one recipient with a CustomID
        /// 
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult SendEmail()
        //{
        //    //ExecuteTest().Wait();
        //    Execute(123);

        //    return View("MemberResponse");
        //}
        static async Task RunAsync()
        {
           string Blackjack_Public_Key =  WebConfigurationManager.AppSettings["apiPublicMJKey"];
           string Blackjack_Private_Key = WebConfigurationManager.AppSettings["apiPrivateMJKey"];

            MailjetClient client = new MailjetClient(Blackjack_Public_Key, Blackjack_Private_Key);  //(Environment.GetEnvironmentVariable(Blackjack_Public_Key), Environment.GetEnvironmentVariable(Blackjack_Private_Key));
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource
            }
               .Property(Send.FromEmail, "jkpaskus@gmail.com")
               .Property(Send.FromName, "Jonas Paskus")
               .Property(Send.Subject, "Your company's event outing!")
               .Property(Send.TextPart, "Dear passenger, welcome to Mailjet! May the delivery force be with you!")
               .Property(Send.HtmlPart, "<h3>Dear passenger, welcome to Mailjet!</h3><br />May the delivery force be with you!")
               .Property(Send.To, new JArray {
                new JObject {
                 {"Email", "jkpaskus@gmail.com"}
                 }
                   });
            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                Console.WriteLine(response.GetData());
            }
            else
            {
                Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
                Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            }
        }
    }
}