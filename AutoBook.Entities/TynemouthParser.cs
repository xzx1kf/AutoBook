using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

namespace AutoBook.Entities
{
	public class TynemouthParser
	{
		private static readonly Regex timeRegex = new Regex (@"^([0-9]| [0-9]|12|):[0-5][0-9]$");
		  
		public static bool Booked(HtmlNode slot)
		{
			if (slot.SelectSingleNode ("div[@class='booked']") != null) {
				return true;
			}
			return false;
		}

		public static int GetCourt(string bookingLink)
		{
			return int.Parse (bookingLink.Substring (20, 1));
		}

		public static DateTime GetDateTime(DateTime date, TimeSpan time)
		{
			return new DateTime (date.Year, date.Month, date.Day, time.Hours, time.Minutes, 0);
		}

		public static string ParseBookingLink(HtmlNode slot)
		{
			var bookingLink = slot.SelectSingleNode ("div[@class='book']/a[@class='booking_link']");
			return bookingLink.Attributes["href"].Value;
		}

		public static DateTime ParseDate(string date)
		{
			var newDate = new DateTime ();
			if (DateTime.TryParseExact (date, "dddd dd MMMM", CultureInfo.InvariantCulture, DateTimeStyles.None, out newDate)) {
				return new DateTime (DateTime.Today.Year, newDate.Month, newDate.Day, 12, 0, 0);
			} else {
				throw new Exception ("Could not parse the Date.");
			}
		}

		public static TimeSpan ParseTime(HtmlNode slot)
		{
			HtmlNode timeNode = null;
			timeNode = slot.SelectSingleNode ("div[@class='time']");
			if (timeNode == null) {
				timeNode = slot.SelectSingleNode ("div[@class='peakTime']");
			}

			var time = "";

			if (timeNode == null) {
				throw new ArgumentException ("The booking slot does not have a time parameter. Is it booked already?");
			} else {
				time = timeNode.InnerText;
			}

			if (!VerifyTime (time)) { 
				throw new Exception ("Time is not in the correct format.");
			}

			var timeStr = time.Split (new char[] { ':' });
			var timeInt = Array.ConvertAll(timeStr, int.Parse);

			if (timeInt [0] < 12) {
				return new TimeSpan (timeInt [0] + 12, timeInt [1], 0);
			} else {
				return new TimeSpan (timeInt [0], timeInt [1], 0);
			}
		}

		private static bool VerifyTime(string time)
		{
			return timeRegex.IsMatch (time);
		}
	}
}

