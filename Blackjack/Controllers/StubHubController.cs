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
        // GET: StubHub
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StubHubSearchResult()
        {
            DateTime date = DateTime.Today;
            string dateString = "2017-06-29";// T19:30:00-0700";
            string city = "New York";

            ViewBag.Message = GetStubHubData(city, dateString);

            return View();
        }

        public ActionResult GetStubHubData(string city, string date)
        {
            string queryString = "https://api.stubhub.com/search/catalog/events/v3?status=active&start=0&rows=20&city=" + city
                + "&eventDateLocal=" + date;

            HttpWebRequest request = WebRequest.CreateHttp(queryString);

            request.UserAgent = @"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:20.0) Gecko/20100101 Firefox/20.0";

            string bearerToken = GetAcccessToken();

            request.Headers.Add("Authorization", "Bearer " + bearerToken);
            request.ContentType = "application/json";
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());

            string data = rd.ReadToEnd();

            ViewBag.Message = createEventList(data);

            return View("StubHubSearchResult");

        }

        public static IList<Event> createEventList(string data)
        {
            JObject StubHubData = JObject.Parse(data);

            // get JSON result objects into a list
            IList<JToken> eventFields = StubHubData["events"].Children().ToList();

            //eventFields.Add(StubHubData["events"][0]["categories"][1]["name"]);   //attempt to add category from JSON data

            // serialize JSON results into .NET objects
            IList<Event> searchResults = new List<Event>();

            //create Event objects with fields that match JTokens
            foreach (JToken result in eventFields)
            {
                Event searchResult = JsonConvert.DeserializeObject<Event>(result.ToString());  //result["categories"][1]["name"]
                searchResults.Add(searchResult);
            }

            return searchResults;
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
    }
}