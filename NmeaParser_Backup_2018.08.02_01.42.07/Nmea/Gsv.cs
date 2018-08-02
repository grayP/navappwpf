using NmeaParser.Nmea.Gps;
using System;
using System.Collections.Generic;
using System.Globalization;


namespace NmeaParser.Nmea
{
	/// <summary>
	///  GPS Satellites in view
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gsv")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public abstract class Gsv : NmeaMessage, IMultiPartMessage<SatelliteVehicle>
	{
		/// <summary>
		/// Called when the message is being loaded.
		/// </summary>
		/// <param name="message">The NMEA message values.</param>
		protected override void OnLoadMessage(string[] message)
		{
			if (message == null || message.Length < 3)
				throw new ArgumentException("Invalid GSV", "message"); 
			
			TotalMessages = int.Parse(message[0], CultureInfo.InvariantCulture);
			MessageNumber = int.Parse(message[1], CultureInfo.InvariantCulture);
			SVsInView = int.Parse(message[2], CultureInfo.InvariantCulture);

			List<SatelliteVehicle> svs = new List<SatelliteVehicle>();
			for (int i = 3; i < message.Length - 3; i += 4)
			{
				if (message[i].Length == 0)
					continue;
				else
					svs.Add(new SatelliteVehicle(message, i));
			}
			this.SVs = svs.ToArray();
		}

		/// <summary>
		/// Total number of messages of this type in this cycle
		/// </summary>
		public int TotalMessages { get; private set; }

		/// <summary>
		/// Message number
		/// </summary>
		public int MessageNumber { get; private set; }

		/// <summary>
		/// Total number of SVs in view
		/// </summary>
		public int SVsInView { get; private set; }

		/// <summary>
		/// Dilution of precision
		/// </summary>
		public IReadOnlyList<SatelliteVehicle> SVs { get; private set; }

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns> A System.Collections.Generic.IEnumerator{SatelliteVehicle} that can be used to iterate through the collection.</returns>
		public IEnumerator<SatelliteVehicle> GetEnumerator()
		{
			foreach(var sv in SVs)
				yield return sv;
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns> An System.Collections.IEnumerator object that can be used to iterate through the collection.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
