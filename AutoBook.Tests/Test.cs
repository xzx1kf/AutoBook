using System;
using HtmlAgilityPack;
using NUnit.Framework;

using AutoBook.Entities;

namespace AutoBook.Tests
{
	[TestFixture()]
	public class Test
	{
		private const string mockWebPage = @"../../../TestData/view-source_tynemouth-squash.herokuapp.com.html";
		private HtmlDocument htmlDoc;
		private DateTime dateToBook;
		private TynemouthBookingSlot courtOneAtTwelve_01052017;
		private int court;
		private bool booked;

		[SetUp()]
		public void SetUp()
		{
			htmlDoc = new HtmlDocument ();
			htmlDoc.Load(mockWebPage);

			dateToBook = new DateTime (2017, 5, 1, 12, 0, 0);
			court = 1;
			booked = false;
			courtOneAtTwelve_01052017 = new TynemouthBookingSlot (dateToBook, court, false);
		}

		[Test()]
		public void BookingSlot_Returns_Correct_Date ()
		{
			var result = courtOneAtTwelve_01052017.Date;
			Assert.That (result == dateToBook);
		}

		[Test()]
		public void BookingSlot_Returns_Correct_CourtNumber()
		{
			var result = courtOneAtTwelve_01052017.Court;
			Assert.That (result == court);
		}

		[Test()]
		public void BookingSlot_Returns_Correct_Booked()
		{
			var result = courtOneAtTwelve_01052017.Booked;
			Assert.That (result == booked);
		}

	}
}

