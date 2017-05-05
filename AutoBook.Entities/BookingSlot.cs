using System;

namespace AutoBook.Entities
{
//	public interface BookingSlot
//	{
//		void Book (IBookingSlot slot);
//	}

	public class BookingSlot
	{
		public DateTime Date { get; set; }

		public BookingSlot(DateTime date)
		{
			this.Date = date;
		}
	}

	public class TynemouthBookingSlot : BookingSlot
	{
		public int Court {
			get;
			set;
		}

		public bool Booked {
			get;
			set;
		}

		public TynemouthBookingSlot(DateTime date, int court, bool booked) : base(date)
		{
			this.Date = date;
			this.Court = court;
			this.Booked = booked;
		}

		public bool BookCourt(BookingSlot slot)
		{
			return true;
		}
	}
}

