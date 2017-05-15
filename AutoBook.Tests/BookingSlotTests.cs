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
		private Slot _courtOneAtTwelve01052017;
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
			_bookingLink = "http://tynemouth-squash.herokuapp.com/bookings/new?court=2&amp;days=20&amp;hour=20&amp;min=30&amp;timeSlot=38";
			_courtOneAtTwelve01052017 = new Slot (_dateToBook, _court, false, _bookingLink);
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
			_courtOneAtTwelve01052017.Date = _courtOneAtTwelve01052017.Date.AddYears (100);
			var result = _courtOneAtTwelve01052017.Book ();
			Assert.That (result, Is.True);
			Assert.That (_courtOneAtTwelve01052017.Booked, Is.True);
		}

		[Test()]
		public void BookSlotThatIsBookedAlready()
		{
			// Book the court.
			_courtOneAtTwelve01052017.Date = _courtOneAtTwelve01052017.Date.AddYears (100);
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

	    [Test()]
	    public void SlotIsInThePast()
	    {
	        var result = _courtOneAtTwelve01052017.Book();
	        Assert.That(result, Is.False);
	    }

		[Test()]
		public void SlotReturnsCorrectDaysProperty()
		{
			var result = _courtOneAtTwelve01052017.Days;
			Assert.That (result, Is.EqualTo ("20"));
		}

		[Test()]
		public void SlotReturnsCorrectTimeSlot()
		{
			var result = _courtOneAtTwelve01052017.TimeSlot;
			Assert.That (result, Is.EqualTo ("38"));
		}

		[Test()]
		public void SlotReturnsStartTime()
		{
			var result = _courtOneAtTwelve01052017.StartTime;
			Assert.That (result, Is.EqualTo ("2017-05-01 12:00:00 +01:00"));
		}
	}
}

