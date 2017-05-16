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

			var courts = tynemouth.Courts.Where (c => c.Number == 4);

			foreach (var court in courts) {
				System.Console.WriteLine (court.Number);

				foreach (var slot in court.Slots) {

					Console.WriteLine ("{0} - {1}", slot.Date, slot.Booked);
				}
			}



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
