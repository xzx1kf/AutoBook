using System;
using System.IO;
using System.Reflection;
using HtmlAgilityPack;
using NUnit.Framework;

using AutoBook.Entities;

namespace AutoBook.Tests
{
	[TestFixture()]
	public class BookingSlotTests
	{
		//private const string mockWebPage = @"../../../TestData/view-source_tynemouth-squash.herokuapp.com.html";
		private HtmlDocument _htmlDoc;
		private DateTime _dateToBook;
		private TynemouthBookingSlot _courtOneAtTwelve01052017;
		private int _court;
		private bool _booked;
		private string _bookingLink;

		[SetUp()]
		public void SetUp()
		{
		    var text = "";
		    var assembly = Assembly.GetExecutingAssembly();
		    using (var stream = assembly.GetManifestResourceStream("AutoBook.Tests.TestData.view-source_tynemouth-squash.herokuapp.com.html"))
		        if (stream != null)
		            using (var reader = new StreamReader(stream))
		            {
		                text = reader.ReadToEnd();
		            }

		    _htmlDoc = new HtmlDocument();
		    _htmlDoc.LoadHtml(text);

            _dateToBook = new DateTime (2017, 5, 1, 12, 0, 0);
			_court = 1;
			_booked = false;
			_bookingLink = "http://www.google.co.uk";
			_courtOneAtTwelve01052017 = new TynemouthBookingSlot (_dateToBook, _court, false, _bookingLink);
		}

		[Test()]
		public void SlotDate ()
		{
			var result = _courtOneAtTwelve01052017.Date;
			Assert.That (result, Is.EqualTo(_dateToBook));
		}

		[Test()]
		public void SlotCourt()
		{
			var result = _courtOneAtTwelve01052017.Court;
			Assert.That (result, Is.EqualTo(_court));
		}

		[Test()]
		public void SlotBooked()
		{
			var result = _courtOneAtTwelve01052017.Booked;
			Assert.That (result, Is.EqualTo(_booked));
		}

		[Test()]
		public void BookSlot()
		{
			var result = _courtOneAtTwelve01052017.Book ();
			Assert.That (result, Is.True);
			Assert.That (_courtOneAtTwelve01052017.Booked, Is.True);
		}

		[Test()]
		public void BookSlotThatIsBookedAlready()
		{
			// Book the court.
			var result = _courtOneAtTwelve01052017.Book ();
			Assert.That (result, Is.True);
			Assert.That (_courtOneAtTwelve01052017.Booked, Is.True);

			// Try to book it again.
			Exception ex = Assert.Throws<ApplicationException>(
				delegate { _courtOneAtTwelve01052017.Book (); } );
			Assert.That( ex.Message, Is.EqualTo( "This slot has already been booked." ) );
		}

		[Test()]
		public void NoBookingLink()
		{
			_courtOneAtTwelve01052017.BookingLink = null;

			Exception ex = Assert.Throws<ApplicationException>(
				delegate { _courtOneAtTwelve01052017.Book (); } );
			Assert.That( ex.Message, Is.EqualTo( "This slot has no booking url." ) );
		}
	}
}

