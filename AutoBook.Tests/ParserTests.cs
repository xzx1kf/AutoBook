using System;
using System.IO;
using System.Reflection;
using AutoBook.Entities;
using NUnit.Framework;
using HtmlAgilityPack;

namespace AutoBook.Tests
{
	[TestFixture()]
	public class ParserTests
	{
		//private const string MockWebPage = @"TestData/view-source_tynemouth-squash.herokuapp.com.html";
		private HtmlDocument _htmlDoc;

	    [OneTimeSetUp()]
	    public void OneTimeSetUp()
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
        }

		[SetUp()]
		public void SetUp()
		{
		    

            

			// Booking slots
			//var bookingSlots = htmlDoc.DocumentNode.SelectNodes ("//table[@class='booking']/tr/td[@class='booking']");
		}

		[Test()]
		public void ParseTimeString (
			[Values("12:20", "1:00", "7:40")] string time)
		{
			HtmlNode node = new HtmlNode (HtmlNodeType.Text, _htmlDoc, 0);
			node.InnerHtml = "<div class='time'>"+time+"</div>";

			var result = TynemouthParser.ParseTime (node);

			var timeArr = time.Split (new char[] { ':' });
			var timeNum = Array.ConvertAll (timeArr, int.Parse);

			Assert.That (result.Hours, Is.EqualTo (timeNum[0]));
			Assert.That (result.Minutes, Is.EqualTo (timeNum[1]));
		}

		[Test()]
		public void ParseTimeWithIncorrectFormat()
		{
			HtmlNode node = new HtmlNode (HtmlNodeType.Text, _htmlDoc, 0);
			node.InnerHtml = "<div class='time'>This isn't a time</div>";

			Exception ex = Assert.Throws<Exception>(
				delegate { TynemouthParser.ParseTime (node); } );
			Assert.That( ex.Message, Is.EqualTo( "Time is not in the correct format." ) );
		}

		[Test()]
		public void ParseTimeWhereTimeIsNotAllowed(
			[Values("10:00", "13:00", "11:59", "2:1", "AB:CD", "1:A", "2:3B")] string time)
		{
			HtmlNode node = new HtmlNode (HtmlNodeType.Text, _htmlDoc, 0);
			node.InnerHtml = "<div class='time'>"+time+"</div>";

			Exception ex = Assert.Throws<Exception>(
				delegate { TynemouthParser.ParseTime (node); } );
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

