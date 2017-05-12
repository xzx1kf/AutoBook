using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace AutoBook.Entities
{
	public class TynemouthBookingSlot : IBookingSlot
	{
		public DateTime Date {
			get;
			set;
		}
		public int Court {
			get;
			set;
		}

		public bool Booked {
			get;
			set;
		}

		public string BookingLink { 
			get; 
			set; 
		}

		public string TimeSlot {
			get;
			private set;
		}

		public string Days {
			get;
			private set;
		}

		public string StartTime {
			get;
			private set;
		}

		public TynemouthBookingSlot(DateTime date, int court, bool booked) 
		{
			this.Date = date;
			this.Court = court;
			this.Booked = booked;
		}

		public TynemouthBookingSlot(DateTime date, int court, bool booked, string bookingLink) 
		{
			this.Date = date;
			this.Court = court;
			this.Booked = booked;
			this.BookingLink = bookingLink;

			this.parseBookingLink ();
			this.StartTime = this.Date.ToString ("yyyy-MM-dd HH:mm:ss zzz");
		}

		public bool Book()
		{
			if (this.Booked) {
				throw new ApplicationException ("This slot has already been booked.");
			}

			if (string.IsNullOrEmpty (this.BookingLink)) {
				throw new ApplicationException ("This slot has no booking url.");
			}

		    if (this.Date <= DateTime.Now)
		    {
		        return false;
		    }

			//BookSlotOnWebSite ();

			this.Booked = true;
			return this.Booked;
		}

		private void parseBookingLink()
		{
			var options = this.BookingLink.Replace ("/bookings/new?", "");
			var tokens = options.Split (new string[] { "&amp;" }, StringSplitOptions.None);

			var parameterDict = new StringDictionary ();

			foreach (var parameters in tokens)
			{
				var s = parameters.Split (new char[] { '=' });
				parameterDict.Add(s[0], s[1]); 
			}

			this.TimeSlot = parameterDict ["timeSlot"];
			this.Days = parameterDict ["days"];
		}

		private void BookSlotOnWebSite ()
		{
			var cookieJar = new CookieContainer ();
			using (var client = new CookieAwareWebClient (cookieJar)) {
				// the website sets some cookie that is needed for login, and as well the 'authenticity_token' is always different
				string response = client.DownloadString ("http://tynemouth-squash.herokuapp.com" + this.BookingLink);
				// parse the 'authenticity_token' and cookie is auto handled by the cookieContainer
				string token = Regex.Match (response, "authenticity_token.+?value=\"(.+?)\"").Groups [1].Value;
				var values = new NameValueCollection ();
				values ["authenticity_token"] = token;
				values ["booking[booking_number]"] = "1";
				values ["booking[full_name]"] = "Nick Hale";
				values ["booking[membership_number]"] = "s119";
				values ["booking[start_time]"] = this.Date.ToString ("yyyy-MM-dd HH:mm:ss zzz");
				values ["booking[time_slot_id]"] = this.TimeSlot;
				values ["booking[court_time]"] = "40";
				values ["booking[court_id]"] = this.Court.ToString();
				values ["booking[days]"] = this.Days;
				values ["commit"] = "Book Court";
				values ["utf8"] = "%E2%9C%93";
				client.Encoding = System.Text.Encoding.UTF8;
				client.Headers.Add ("Content-Type", "application/x-www-form-urlencoded");
				var response2 = client.UploadValues ("http://tynemouth-squash.herokuapp.com/bookings", values);
				System.Console.WriteLine (Encoding.UTF8.GetString (response2));
			}
		}
	}

	public class CookieAwareWebClient : WebClient
	{
		public string Method;

		public CookieContainer CookieContainer { get; set; }

		public Uri Uri { get; set; }

		public CookieAwareWebClient ()
			: this(new CookieContainer())
		{
		}

		public CookieAwareWebClient (CookieContainer cookies)
		{
			this.CookieContainer = cookies;
		}

		protected override WebRequest GetWebRequest (Uri address)
		{
			WebRequest request = base.GetWebRequest (address);
			if (request is HttpWebRequest) {
				(request as HttpWebRequest).CookieContainer = this.CookieContainer;
				(request as HttpWebRequest).ServicePoint.Expect100Continue = false;
				(request as HttpWebRequest).UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0";
				(request as HttpWebRequest).Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
				(request as HttpWebRequest).Headers.Add (HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.5");
				(request as HttpWebRequest).Referer = "http://tynemouth-squash.herokuapp.com/bookings/new?court=2&days=20&hour=20&min=30&timeSlot=38";
				(request as HttpWebRequest).KeepAlive = true;
				(request as HttpWebRequest).AutomaticDecompression = DecompressionMethods.Deflate |
					DecompressionMethods.GZip;
				if (Method == "POST") {
					(request as HttpWebRequest).ContentType = "application/x-www-form-urlencoded";
				}

			}
			HttpWebRequest httpRequest = (HttpWebRequest)request;
			httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			return httpRequest;
		}

		protected override WebResponse GetWebResponse (WebRequest request)
		{
			WebResponse response = base.GetWebResponse (request);
			String setCookieHeader = response.Headers [HttpResponseHeader.SetCookie];

			if (setCookieHeader != null) {
				//do something if needed to parse out the cookie.
				try {
					if (setCookieHeader != null) {
						Cookie cookie = new Cookie (); //create cookie
						this.CookieContainer.Add (cookie);
					}
				} catch (Exception) {

				}
			}
			return response;

		}
	}
}

