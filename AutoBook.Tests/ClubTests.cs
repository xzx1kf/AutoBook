using NUnit.Framework;
using System;

using AutoBook.Entities;

namespace AutoBook.Tests
{
	[TestFixture()]
	public class ClubTests
	{
		private Club _tynemouth;

		[SetUp()]
		public void SetUp()
		{
			_tynemouth = new Club ("Tynemouth", "http://tynemouth-squash.herokuapp.com/");
		}

		[Test()]
		public void Has_5_Courts ()
		{
			Assert.That (_tynemouth.Courts.Count, Is.EqualTo (5));
		}

		[Test()]
		public void CourtHas_15_Slots()
		{
			Assert.That (_tynemouth.Courts [0].Slots.Count, Is.EqualTo (15));
		}
	}
}

