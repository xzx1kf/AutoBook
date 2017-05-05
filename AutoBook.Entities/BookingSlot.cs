using System;

namespace AutoBook.Entities
{
	public class BookingSlot
	{
		public int Court { get; set; }
		public DateTime Date { get; set; }
		public bool Booked { get; set; }

		public BookingSlot(DateTime date, int court, bool booked)
		{
			this.Date = date;
			this.Court = court;
			this.Booked = booked;
	}
}

