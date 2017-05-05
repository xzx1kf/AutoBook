using System;
using HtmlAgilityPack;

namespace AutoBook.Entities
{
	public class TynemouthParser
	{
		public static void ParseSlot(HtmlNode slot)
		{
			throw new NotImplementedException ();
		}

		public static DateTime ParseTime(string time)
		{
			// TODO: Check that the format of time is "hh:mm".
			// TODO: Check that it is numeric.
			// TODO: Check boundary values i.e. can it be 11:05.
			var splitTime = time.Split (new char[] { ':' });
			return new DateTime (2017, 5, 1, int.Parse(splitTime[0]), int.Parse(splitTime[1]), 0);
		}
	}
}

