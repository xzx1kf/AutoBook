using System;

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

			var bookingSlots = htmlDoc.DocumentNode.SelectNodes ("//table[@class='booking']/tr/td[@class='booking']");

			foreach (HtmlNode slot in bookingSlots) {
				string[] time = slot.SelectSingleNode ("div[@class='time']").InnerText.Split(new char[]{':'});

				var bookingSlot = new TynemouthBookingSlot (DateTime.Today, 1, true);
			}
		}
	}
}
