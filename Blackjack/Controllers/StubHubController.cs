using Blackjack.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Blackjack.Controllers
{
    public class StubHubController : Controller
    {
        private BlackjackDBEntities db = new BlackjackDBEntities();

        // GET: StubHub
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StubHubSearchResult(int id, DateTime startDate, DateTime endDate, string city)
        {
            int excursionID = id;
            DateTime excStartDate = startDate;
            DateTime excEndDate = endDate;
            string excursionCity = city;

            IList<Event> EventList = GetStubHubData(excursionCity, excStartDate, excEndDate);

            //Add EventList to Choice Table
            foreach (var item in EventList)
            {
                db.Choices.Add(new Choice {
                    ChoiceName = item.Name,
                    URL = item.WebURI,
                    imageURL = item.ImageURL,
                    ChoiceID = item.Id,
                    ExcursionID = excursionID,
                    Votes = 0
                });
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {

                //throw;
            }
            

            //send emails
            //EmailController.SendEmail(excursionID);

            return RedirectToAction("../Choices/Vote", new { excID = excursionID });
        }

        public static IList<Event> GetStubHubData(string city, DateTime startDate, DateTime endDate)
        {
            string queryString = "https://api.stubhub.com/search/catalog/events/v3?status=active&start=0&rows=300&city=" + city;

            HttpWebRequest request = WebRequest.CreateHttp(queryString);

            request.UserAgent = @"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:20.0) Gecko/20100101 Firefox/20.0";

            string bearerToken = GetAcccessToken();

            request.Headers.Add("Authorization", "Bearer " + bearerToken);
            request.ContentType = "application/json";
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());

            string data = rd.ReadToEnd();

            IList<Event> fullResults = parseJSONData(data);

            IList<Event> results = Get9Events(fullResults, startDate, endDate);

            return results;

        }

        public static IList<Event> parseJSONData(string data)
        {
            JObject StubHubData = JObject.Parse(data);

            // get JSON result objects into a list
            IList<JToken> eventFields = StubHubData["events"].Children().ToList();

            // serialize JSON results into .NET objects
            IList<Event> eventList = new List<Event>();

            //create Event objects with fields that match JTokens
            foreach (JToken result in eventFields)
            {
                Event searchResult = JsonConvert.DeserializeObject<Event>(result.ToString());
                eventList.Add(searchResult);
            }

            return eventList;
        }

        public static string GetAcccessToken()
        {
            //access data on web server

            HttpWebRequest request = WebRequest.CreateHttp("https://api.stubhub.com/login");

            request.UserAgent = @"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:20.0) Gecko/20100101 Firefox/20.0";

            //add a key to your request, if required. Read the documentation!
            request.Headers.Add("Authorization", "Basic SFRaWWxicTkwM2dSUkc0cWU5T2Vvekk2dlFBYTo2enRYb1BHWDNQNjdyZEk0TXBRT3lnNjNUU01h");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            string str = "grant_type=password&username=lambrechtca@gmail.com&password=hagrid21";

            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(str);
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            // requestWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //access data within response
            StreamReader rd = new StreamReader(response.GetResponseStream());

            string data = rd.ReadToEnd(); //raw data

            JObject StubHubData = JObject.Parse(data);

            return StubHubData["access_token"].ToString(); //not sure if this is the correct way to return access token
        }

        public static IList<Event> Get9Events(IList<Event> fullresults, DateTime startDate, DateTime endDate)
        {
            IList<Event> result = new List<Event>();
            int count = 0;

            foreach (var item in fullresults)
            {
                if (item.EventDateLocal >= startDate && item.EventDateLocal <= endDate)
                {
                    result.Add(item);
                    count++;

                    if (count >= 9)
                        break;
                }
            }
            
            return result;
        }
    }
}