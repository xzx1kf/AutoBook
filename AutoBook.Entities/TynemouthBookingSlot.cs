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

		public TynemouthBookingSlot(DateTime date, int court, bool booked, string bookingLink) 
		{
			this.Date = date;
			this.Court = court;
			this.Booked = booked;
			this.BookingLink = bookingLink;
		}

		public bool Book()
		{
			if (this.Booked) {
				throw new ApplicationException ("This slot has already been booked.");
			}

			if (string.IsNullOrEmpty (this.BookingLink)) {
				throw new ApplicationException ("This slot has no booking url.");
			}

			this.Booked = true;
			return this.Booked;
		}
	}
}

