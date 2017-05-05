using System;
using HtmlAgilityPack;
using NUnit.Framework;

using AutoBook.Entities;

namespace AutoBook.Tests
{
	[TestFixture()]
	public class BookingSlotTests
	{
		private const string mockWebPage = @"../../../TestData/view-source_tynemouth-squash.herokuapp.com.html";
		private HtmlDocument htmlDoc;
		private DateTime dateToBook;
		private TynemouthBookingSlot courtOneAtTwelve_01052017;
		private int court;
		private bool booked;
		private string bookingLink;

		[SetUp()]
		public void SetUp()
		{
			htmlDoc = new HtmlDocument ();
			htmlDoc.Load(mockWebPage);

			dateToBook = new DateTime (2017, 5, 1, 12, 0, 0);
			court = 1;
			booked = false;
			bookingLink = "http://www.google.co.uk";
			courtOneAtTwelve_01052017 = new TynemouthBookingSlot (dateToBook, court, false, bookingLink);
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
			// Book the court.
			var result = courtOneAtTwelve_01052017.Book ();
			Assert.That (result, Is.True);
			Assert.That (courtOneAtTwelve_01052017.Booked, Is.True);

			// Try to book it again.
			Exception ex = Assert.Throws<ApplicationException>(
				delegate { courtOneAtTwelve_01052017.Book (); } );
			Assert.That( ex.Message, Is.EqualTo( "This slot has already been booked." ) );
		}

		[Test()]
		public void NoBookingLink()
		{
			courtOneAtTwelve_01052017.BookingLink = null;

			Exception ex = Assert.Throws<ApplicationException>(
				delegate { courtOneAtTwelve_01052017.Book (); } );
			Assert.That( ex.Message, Is.EqualTo( "This slot has no booking url." ) );
		}
	}
}

