using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using AutoBook.Entities;
using HtmlAgilityPack;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoBook.Proto
{
	class MainClass
	{
	    //private static readonly HttpClient client = new HttpClient();

	    public static async void Book()
	    {
            /*
	        var values = new Dictionary<string, string>
	        {
	            { "booking[booking_number]", "1" },
	            { "booking[full_name]", "Nick Hale" },
	            { "booking[membership_number]", "s119" },
	            { "booking[start_time]", "2017-05-31 20:30:00 +01:00" },
                { "booking[time_slot_id]", "38"},
                { "booking[court_time]", "40"},
                { "booking[court_id]", "2" },
                { "booking[days]", "20" }
            };

	        var content = new FormUrlEncodedContent(values);

	        var response = await client.PostAsync("http://tynemouth-squash.herokuapp.com/bookings/new?court=2&days=20&hour=20&min=30&timeSlot=38", content);

	        var responseString = await response.Content.ReadAsStringAsync();
            */
        }

        public static void Main (string[] args)
		{
			var cookieJar = new CookieContainer();
			using (var client = new CookieAwareWebClient(cookieJar))
			{

		    // the website sets some cookie that is needed for login, and as well the 'authenticity_token' is always different
		    string response = client.DownloadString("http://tynemouth-squash.herokuapp.com/bookings/new?court=2&days=20&hour=20&min=30&timeSlot=38");

		    // parse the 'authenticity_token' and cookie is auto handled by the cookieContainer
		    string token = Regex.Match(response, "authenticity_token.+?value=\"(.+?)\"").Groups[1].Value;

            // create a new HTTP Web Client that supports cookies
            //var webClient = new WebClient();

		    //download my contact page containing the Anti CRSF Token
		    //var webClientData = webClient.DownloadData("http://tynemouth-squash.herokuapp.com/bookings/new?court=2&days=20&hour=20&min=30&timeSlot=38");

		    //parse out the Anti CRSF Token
		    //var antiCrsfToken = Regex.GetTokenString(
		     //   new Regex("__RequestVerificationToken=(?<CRSF_Token>[^;]+)")
		      //      .Match(webClientData.ResponseHeaders["Set-Cookie"]), "CRSF_Token");
                   
          
		        var values = new NameValueCollection();
		        values["authenticity_token"] = token;
		        values["booking[booking_number]"] = "1";
		        values["booking[full_name]"] = "Nick Hale";
		        values["booking[membership_number]"]= "s119";
		        values["booking[start_time]"] = "2017-05-31 20:30:00 +01:00";
		        values["booking[time_slot_id]"] = "38";
		        values["booking[court_time]"] = "40";
		        values["booking[court_id]"] = "2";
		        values["booking[days]"] = "20";
                values["commit"] = "Book Court";
		        values["utf8"] = "%E2%9C%93";

		        client.Encoding = System.Text.Encoding.UTF8;

                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                var response2 = client.UploadValues("http://tynemouth-squash.herokuapp.com/bookings", values);

		        var responseString = Encoding.Default.GetString(response2);
		    }

            



            /*
            var htmlDoc = new HtmlDocument ();
            htmlDoc.Load(@"../../../TestData/view-source_tynemouth-squash.herokuapp.com.html");

            // Booking slots
            var bookingSlots = htmlDoc.DocumentNode.SelectNodes ("//table[@class='booking']/tr/td[@class='booking']");

            // Date
            var dateStr = htmlDoc.DocumentNode.SelectSingleNode ("//div[@class='date']/span[@class='datePicker']/a[@class='day']").InnerText;
            var date = TynemouthParser.ParseDate (dateStr);

            var courtAvailability = new List<TynemouthBookingSlot> ();

            foreach (HtmlNode slot in bookingSlots) {
                // Time
                var time = new TimeSpan();
                try {
                    time = TynemouthParser.ParseTime (slot);
                }
                catch(ArgumentException) {
                    continue;
                }

                // DateTime
                var slotDateTime = TynemouthParser.GetDateTime (date, time);

                // Is the slot available?
                var booked = TynemouthParser.Booked (slot);

                // Booking link
                var bookingLink = "";
                if (!booked) {
                    bookingLink = TynemouthParser.ParseBookingLink (slot);
                }

                // Court
                var court = 0;
                if (!booked) {
                    court = TynemouthParser.GetCourt (bookingLink);
                }

                var bookingSlot = new TynemouthBookingSlot (slotDateTime, court, booked, bookingLink);
                courtAvailability.Add (bookingSlot);
            }

            foreach (TynemouthBookingSlot slot in courtAvailability) {
                Console.WriteLine (string.Format ("{0} {1} {2}", slot.Date, slot.Court, slot.BookingLink));
            }

            Console.ReadKey();
            */
        }
	}

    public class CookieAwareWebClient : WebClient
    {
        public string Method;
        public CookieContainer CookieContainer { get; set; }
        public Uri Uri { get; set; }

        public CookieAwareWebClient()
            : this(new CookieContainer())
        {
        }

        public CookieAwareWebClient(CookieContainer cookies)
        {
            this.CookieContainer = cookies;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = this.CookieContainer;
                (request as HttpWebRequest).ServicePoint.Expect100Continue = false;
                (request as HttpWebRequest).UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0";
                (request as HttpWebRequest).Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                (request as HttpWebRequest).Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.5");
                (request as HttpWebRequest).Referer = "http://tynemouth-squash.herokuapp.com/bookings/new?court=2&days=20&hour=20&min=30&timeSlot=38";
                (request as HttpWebRequest).KeepAlive = true;
                (request as HttpWebRequest).AutomaticDecompression = DecompressionMethods.Deflate |
                                                                     DecompressionMethods.GZip;
                if (Method == "POST")
                {
                    (request as HttpWebRequest).ContentType = "application/x-www-form-urlencoded";
                }

            }
            HttpWebRequest httpRequest = (HttpWebRequest)request;
            httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return httpRequest;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            String setCookieHeader = response.Headers[HttpResponseHeader.SetCookie];

            if (setCookieHeader != null)
            {
                //do something if needed to parse out the cookie.
                try
                {
                    if (setCookieHeader != null)
                    {
                        Cookie cookie = new Cookie(); //create cookie
                        this.CookieContainer.Add(cookie);
                    }
                }
                catch (Exception)
                {

                }
            }
            return response;

        }
    }
}
