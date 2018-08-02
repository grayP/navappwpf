﻿using System;
using System.Globalization;

namespace NmeaParser.Nmea
{
    /// <summary>
    ///  Global Positioning System Fix Data
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gpgga")]
    public class Gga : NmeaMessage
    {
        /// <summary>
        /// Called when the message is being loaded.
        /// </summary>
        /// <param name="message">The NMEA message values.</param>
        protected override void OnLoadMessage(string[] message)
        {
            if (message == null || message.Length < 14)
                throw new ArgumentException("Invalid GPGGA", "message");
            FixTime = StringToTimeSpan(message[0]);
            Latitude = NmeaMessage.StringToLatitude(message[1], message[2]);
            Longitude = NmeaMessage.StringToLongitude(message[3], message[4]);
            Quality = (Gps.Gpgga.FixQuality)int.Parse(message[5], CultureInfo.InvariantCulture);
            NumberOfSatellites = int.Parse(message[6], CultureInfo.InvariantCulture);
            Hdop = NmeaMessage.StringToDouble(message[7]);
            Altitude = NmeaMessage.StringToDouble(message[8]);
            AltitudeUnits = message[9];
            HeightOfGeoid = NmeaMessage.StringToDouble(message[10]);
            HeightOfGeoidUnits = message[11];
            var timeInSeconds = StringToDouble(message[12]);
            if (!double.IsNaN(timeInSeconds))
                TimeSinceLastDgpsUpdate = TimeSpan.FromSeconds(timeInSeconds);
            else
                TimeSinceLastDgpsUpdate = TimeSpan.MaxValue;
            if (message[13].Length > 0)
                DgpsStationId = int.Parse(message[13], CultureInfo.InvariantCulture);
            else
                DgpsStationId = -1;
        }

        /// <summary>
        /// Time of day fix was taken
        /// </summary>
        public TimeSpan FixTime { get; private set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public double Latitude { get; private set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude { get; private set; }

        /// <summary>
        /// Fix Quality
        /// </summary>
        public Gps.Gpgga.FixQuality Quality { get; private set; }

        /// <summary>
        /// Number of satellites being tracked
        /// </summary>
        public int NumberOfSatellites { get; private set; }

        /// <summary>
        /// Horizontal Dilution of Precision
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hdop")]
        public double Hdop { get; private set; }

        /// <summary>
        /// Altitude
        /// </summary>
        public double Altitude { get; private set; }

        /// <summary>
        /// Altitude units ('M' for Meters)
        /// </summary>
        public string AltitudeUnits { get; private set; }

        /// <summary>
        /// Height of geoid (mean sea level) above WGS84
        /// </summary>
        public double HeightOfGeoid { get; private set; }

        /// <summary>
        /// Altitude units ('M' for Meters)
        /// </summary>
        public string HeightOfGeoidUnits { get; private set; }

        /// <summary>
        /// Time since last DGPS update
        /// </summary>
        public TimeSpan TimeSinceLastDgpsUpdate { get; private set; }

        /// <summary>
        /// DGPS Station ID Number
        /// </summary>
        public int DgpsStationId { get; private set; }
    }
}
