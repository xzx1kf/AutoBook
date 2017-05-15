using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;

using HtmlAgilityPack;

namespace AutoBook.Entities
{
	[Serializable]
	public class Club
	{
		private Dictionary<DateTime, string> _bookingData;

		public string Name {
			get;
			set;
		}

		public string Url {
			get;
			set;
		}

		public IList<Court> Courts {
			get;
			set;
		}

		public Club (string name, string url)
		{
			this.Name = name;
			this.Url = url;

			_bookingData = new Dictionary<DateTime, string> ();
			Courts = new List<Court> ();

			// TODO: Temp
			this.GetData (0);
			this.Parse ();
		}

		/// <summary>
		/// Gets the data.
		/// </summary>
		private void GetData(int numberOfDays)
		{
			using (var client = new WebClient()) {
				for (int day = 0; day <= numberOfDays; day++) {
					var html = client.DownloadString (this.Url + "?day=" + day);
					_bookingData.Add (DateTime.Today.AddDays (day), html);
				}
			}
		}

		private void Parse()
		{
			foreach (var bookingData in _bookingData) {
				var htmlDoc = new HtmlDocument ();
				htmlDoc.LoadHtml (bookingData.Value);

				var courts = htmlDoc.DocumentNode.SelectNodes ("//table[@class='booking']/tr[1]/th");

				foreach (var court in courts) {
					var courtIndex = court.InnerText.IndexOfAny(new char[] {'1','2','3','4','5'});
					var courtNumber = int.Parse(court.InnerText [courtIndex].ToString());
					this.Courts.Add (new Court (courtNumber));
				}

				// Booking slots
				var bookingSlots = htmlDoc.DocumentNode.SelectNodes ("//table[@class='booking']/tr/td[@class='booking']");

				// Date
				var date = bookingData.Key;

				foreach (HtmlNode slot in bookingSlots) {
					// Time
					var time = new TimeSpan ();
					try {
						time = TynemouthParser.ParseTime (slot);
					} catch (ArgumentException) {
						continue;
					}

					// DateTime
					var slotDateTime = TynemouthParser.GetDateTime (date, time);

					// Is the slot available?
					var booked = TynemouthParser.Booked (slot);

					// Booking link
					var bookingLink = "";
					if (!booked) {
						bookingLink = TynemouthParser.ParseBookingLink (slot);
					}

					// Court
					var court = 0;
					if (!booked) {
						court = TynemouthParser.GetCourt (bookingLink);
					} else {
						string bookingDetailHtml;
						var bookingDetail = slot.SelectSingleNode("div[@class='booked']/a").Attributes ["href"].Value;

						using (var client = new WebClient()) {
							bookingDetailHtml = client.DownloadString ("http://tynemouth-squash.herokuapp.com" + bookingDetail);
						}

						var bookingDetailHtmlDoc = new HtmlDocument ();
						bookingDetailHtmlDoc.LoadHtml (bookingDetailHtml);

						var htmlNode = bookingDetailHtmlDoc.DocumentNode.SelectSingleNode ("/html/body/h1");
						var courtIndex = htmlNode.InnerText.IndexOfAny(new char[] {'1','2','3','4','5'});
						court = int.Parse(htmlNode.InnerText [courtIndex].ToString());
					}

					var bookingSlot = new Slot (slotDateTime, booked, bookingLink);

					Courts.Single (c => c.Number == court).Slots.Add (bookingSlot);
				}
			}
		}
	}
}

