using System;

namespace AutoBook.Entities
{
	public class TynemouthBookingSlot : IBookingSlot
	{
		public DateTime Date {
			get;
			set;
		}
		public int Court {
			get;
			set;
		}

		public bool Booked {
			get;
			set;
		}

		public string BookingLink { 
			get; 
			set; 
		}

		public TynemouthBookingSlot(DateTime date, int court, bool booked) 
		{
			this.Date = date;
			this.Court = court;
			this.Booked = booked;
		}

		public bool Book()
		{
			if (this.Booked) {
				return false;
			}
			this.Booked = true;
			return this.Booked;
		}
	}
}

