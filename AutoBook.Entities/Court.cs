using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace AutoBook.Entities
{
    public class Court
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public IList<TynemouthBookingSlot> Slots { get; set; }

        public Court()
        {
            Slots = new List<TynemouthBookingSlot>();
        }

        public void LoadData()
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(@"../../../TestData/view-source_tynemouth-squash.herokuapp.com.html");

            // Booking slots
            var bookingSlots = htmlDoc.DocumentNode.SelectNodes("//table[@class='booking']/tr/td[@class='booking']");

            // Date
            var dateStr = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='date']/span[@class='datePicker']/a[@class='day']").InnerText;
            var date = TynemouthParser.ParseDate(dateStr);

            foreach (HtmlNode slot in bookingSlots)
            {
                // Time
                TimeSpan time;
                try
                {
                    time = TynemouthParser.ParseTime(slot);
                }
                catch (ArgumentException)
                {
                    continue;
                }

                // DateTime
                var slotDateTime = TynemouthParser.GetDateTime(date, time);

                // Is the slot available?
                var booked = TynemouthParser.Booked(slot);

                // Booking link
                var bookingLink = "";
                if (!booked)
                {
                    bookingLink = TynemouthParser.ParseBookingLink(slot);
                }

                // Court
                var court = 0;
                if (!booked)
                {
                    court = TynemouthParser.GetCourt(bookingLink);
                }

                var bookingSlot = new TynemouthBookingSlot(slotDateTime, court, booked, bookingLink);
                this.Slots.Add(bookingSlot);
            }
        }
    }
}
