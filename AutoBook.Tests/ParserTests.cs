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
		public void ParseTimeString ()
		{
			var time = "13:05";
			var splitTime = time.Split (new char[] { ':' });

			var result = TynemouthParser.ParseTime (time);

			Assert.That (result.Hour, Is.EqualTo (int.Parse(splitTime[0])));
			Assert.That (result.Minute, Is.EqualTo (int.Parse(splitTime[1])));
		}

		[Test()]
		public void ParseTimeWithIncorrectFormat()
		{
			var time = "this isn't a time";

			var result = TynemouthParser.ParseTime (time);
		}
	}
}

