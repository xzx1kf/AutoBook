using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using AutoBook.Entities;
using HtmlAgilityPack;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

namespace AutoBook.Proto
{
	class MainClass
	{
		public static void Main (string[] args)
		{

			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream("MyFile.bin", 
			                               FileMode.Open, 
			                               FileAccess.Read, 
			                               FileShare.Read);
			Club tynemouth = (Club) formatter.Deserialize(stream);
			stream.Close();

			var courts = tynemouth.Courts.Where (c => c.Number == 1);

			foreach (var court in courts) {
				System.Console.WriteLine (court.Number);

				foreach (var slot in court.Slots) {

					Console.WriteLine ("{0} - {1}", slot.Date, slot.Booked);
				}
			}


			//var court1 = tynemouth.Courts.Where (c => c.Slots).Single (s => s.Date.Equals (new DateTime (2017, 5, 22, 21, 10, 0)));
			var court1 = tynemouth.Courts.Single (c => c.Number == 1).Slots.Single(s => s.Date.Equals (new DateTime (2017, 5, 22, 21, 10, 0)));

			Console.WriteLine (court1.Booked);
			Console.Write (court1.Court);
			Console.WriteLine (": " + court1.StartTime);



			/*

			Club tynemouth = new Club ("Tynemouth", "http://tynemouth-squash.herokuapp.com/");


			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream("MyFile.bin", 
			                               FileMode.Create, 
			                               FileAccess.Write, 
			                               FileShare.None);
			formatter.Serialize(stream, tynemouth);
			stream.Close();

            Console.ReadKey();
            */
            
		}
	}
}
