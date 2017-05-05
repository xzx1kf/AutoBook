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
		public void SlotDate ()
		{
			var result = courtOneAtTwelve_01052017.Date;
			Assert.That (result, Is.EqualTo(dateToBook));
		}

		[Test()]
		public void SlotCourt()
		{
			var result = courtOneAtTwelve_01052017.Court;
			Assert.That (result, Is.EqualTo(court));
		}

		[Test()]
		public void SlotBooked()
		{
			var result = courtOneAtTwelve_01052017.Booked;
			Assert.That (result, Is.EqualTo(booked));
		}

		[Test()]
		public void BookSlot()
		{
			var result = courtOneAtTwelve_01052017.Book ();
			Assert.That (result, Is.True);
			Assert.That (courtOneAtTwelve_01052017.Booked, Is.True);
		}

		[Test()]
		public void BookSlotThatIsBookedAlready()
		{
			var result = courtOneAtTwelve_01052017.Book ();
			Assert.That (result, Is.True);
			result = courtOneAtTwelve_01052017.Book ();

			// Test that the court was not booked.
			Assert.That (result, Is.False);

			// However the court should still be in a booked state.
			Assert.That (courtOneAtTwelve_01052017.Booked, Is.True);
		}
	}
}

