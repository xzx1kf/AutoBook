using System;
using AutoBook.Entities;
using NUnit.Framework;
using HtmlAgilityPack;

namespace AutoBook.Tests
{
	[TestFixture()]
	public class ParserTests
	{
		[SetUp()]
		public void SetUp()
		{

		}

		[Test()]
		public void ParseTimeString (
			[Values("2:05", "12:00")] string time)
		{
			var splitTime = time.Split (new char[] { ':' });

			var result = TynemouthParser.ParseTime (time);

			Assert.That (result.Hours, Is.EqualTo (int.Parse(splitTime[0])));
			Assert.That (result.Minutes, Is.EqualTo (int.Parse(splitTime[1])));
		}

		[Test()]
		public void ParseTimeWithIncorrectFormat()
		{
			var time = "this isn't a time";

			Exception ex = Assert.Throws<Exception>(
				delegate { TynemouthParser.ParseTime (time); } );
			Assert.That( ex.Message, Is.EqualTo( "Time is not in the correct format." ) );
		}

		[Test()]
		public void ParseTimeWhereTimeIsNotAllowed(
			[Values("10:00", "13:00", "11:59", "2:1", "AB:CD", "1:A", "2:3B")] string time)
		{
			Exception ex = Assert.Throws<Exception>(
				delegate { TynemouthParser.ParseTime (time); } );
			Assert.That( ex.Message, Is.EqualTo( "Time is not in the correct format." ) );
		}

		[Test()]
		public void ParseDate(
			[Values("Thursday 04 May")] string date)
		{
			var result = TynemouthParser.ParseDate (date);
			Assert.That (result.Day, Is.EqualTo (4));
			Assert.That (result.Month, Is.EqualTo (5));
			Assert.That (result.Year, Is.EqualTo (2017));
		}

		[Test()]
		public void ParseDateInvalid(
			[Values("Fooday 02 June", "Friday 32 June", "Friday 31 June")] string date)
		{
			Exception ex = Assert.Throws<Exception>(
				delegate { TynemouthParser.ParseDate (date); } );
			Assert.That( ex.Message, Is.EqualTo( "Could not parse the Date." ) );
		}

		[Test()]
		public void TestGetDateTime()
		{
			var date = new DateTime (2017, 5, 1);
			var time = new TimeSpan (13, 5, 0);

			var result = TynemouthParser.GetDateTime (date, time);

			Assert.That( date.Add (time), Is.EqualTo(result));
		}

	}
}

