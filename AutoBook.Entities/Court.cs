using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace AutoBook.Entities
{
	[Serializable]
    public class Court
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public IList<Slot> Slots { get; set; }

        public Court(int number)
        {
			this.Number = number;
			this.Name = "Court " + number.ToString ();
            Slots = new List<Slot>();
        }
    }
}
