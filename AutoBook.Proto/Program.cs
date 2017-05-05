using System;
using System.Collections.Generic;

using AutoBook.Entities;
using HtmlAgilityPack;


namespace AutoBook.Proto
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var htmlDoc = new HtmlDocument ();
			htmlDoc.Load(@"/home/nick/TestData/view-source_tynemouth-squash.herokuapp.com.html");

			// Booking slots
			var bookingSlots = htmlDoc.DocumentNode.SelectNodes ("//table[@class='booking']/tr/td[@class='booking']");

			// Date
			var dateStr = htmlDoc.DocumentNode.SelectSingleNode ("//div[@class='date']/span[@class='datePicker']/a[@class='day']").InnerText;
			var date = TynemouthParser.ParseDate (dateStr);

			var courtAvailability = new List<TynemouthBookingSlot> ();

			foreach (HtmlNode slot in bookingSlots) {
				// Time
				// TODO: should be in parser and should test if the 'time' class is there.
				// TODO: unit tests!!!
				var timeStr = slot.SelectSingleNode ("div[@class='time']").InnerText;
				var time = TynemouthParser.ParseTime (timeStr);

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
					court = int.Parse (bookingLink.Substring (20, 1));
				}

				var bookingSlot = new TynemouthBookingSlot (slotDateTime, court, booked, bookingLink);
				courtAvailability.Add (bookingSlot);
			}

			foreach (TynemouthBookingSlot slot in courtAvailability) {
				Console.WriteLine (string.Format ("{0} {1} {2}", slot.Date, slot.Court, slot.BookingLink));
			}
		}
	}
}
