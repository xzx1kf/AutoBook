using System;
using HtmlAgilityPack;
using NUnit.Framework;

using AutoBook.Entities;

namespace AutoBook.Tests
{
	[TestFixture()]
	public class Test
	{
		private const string mockWebPage = @"/home/nick/TestData/view-source_tynemouth-squash.herokuapp.com.html";
		private HtmlDocument htmlDoc;

		[SetUp()]
		public void SetUp()
		{
			htmlDoc = new HtmlDocument ();
			htmlDoc.Load(mockWebPage);
		}

		[Test()]
		public void BookCourt_Returns_Bool ()
		{
			// Arrange
			var bookingSlot = new TynemouthBookingSlot (DateTime.Today, 1, true);

			// Act
			var result = bookingSlot.BookCourt (bookingSlot);

			// Assert
			Assert.That (result is bool);
		}
	}
}

