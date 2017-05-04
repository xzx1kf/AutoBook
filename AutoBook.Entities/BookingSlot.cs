using System;

namespace AutoBook.Entities
{
	public class BookingSlot
	{
		public int Court { get; set; }
		public DateTime Date { get; set; }
		public bool Booked { get; set; }
	}
}

