using System;
using System.Globalization;

namespace NmeaParser.Nmea
{
    /// <summary>
    ///  Global Positioning System Fix Data
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gpgga")]
    public class rmc: NmeaMessage
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
            Validity = message[1];
            Latitude = NmeaMessage.StringToLatitude(message[2], message[3]);
            Longitude = NmeaMessage.StringToLongitude(message[4], message[5]);
            Sog = Double.Parse(message[5], CultureInfo.InvariantCulture);
            Cog = Double.Parse(message[6], CultureInfo.InvariantCulture);
            DateOfFix = message[7];
        }

        /// <summary>
        /// Time of day fix was taken
        /// </summary>
        public TimeSpan FixTime { get; private set; }
        public string Validity { get; private set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public double Latitude { get; private set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude { get; private set; }
        public double Sog { get; private set; }
        public double Cog { get; private set; }
        public string DateOfFix { get; private set; }

 
    
      }
}
