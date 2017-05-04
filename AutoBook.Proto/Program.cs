using System;
using HtmlAgilityPack;
using System.Xml;
using System.Xml.XPath;

namespace AutoBook.Proto
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var htmlDoc = new HtmlDocument ();

			htmlDoc.Load(@"/home/nick/TestData/view-source_tynemouth-squash.herokuapp.com.html");

			// /html/body/div[5]/table/tbody/tr[2]

			var table = htmlDoc.DocumentNode.SelectNodes ("//table[@class='booking']/tr/td[@class='booking']");

			//var slots = table.SelectNodes ("tbody/tr/td");

			foreach (HtmlNode slot in table) {
				System.Console.WriteLine (slot);
			}
		}
	}
}
