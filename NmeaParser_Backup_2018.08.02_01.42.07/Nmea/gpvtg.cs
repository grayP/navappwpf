using System;
using System.Globalization;

namespace NmeaParser.Nmea
{
    /// <summary>
    ///  Global Positioning System Fix Data
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gpgga")]
    public class Gpvtg : NmeaMessage
    {
        /// <summary>
        /// Called when the message is being loaded.
        /// </summary>
        /// <param name="message">The NMEA message values.</param>
        protected override void OnLoadMessage(string[] message)
        {
            if (message == null || message.Length < 8)
                throw new ArgumentException("Invalid GPVTG", "message");
            HeadingTrue = double.Parse(message[1]);
            HeadingMagnetic = double.Parse(message[3]);
         }


        /// <summary>
        /// COG T
        /// </summary>
        public double HeadingTrue { get; private set; }

        /// <summary>
        /// COG M
        /// </summary>
        public double HeadingMagnetic { get; private set; }

        /// <summary>
        /// Fix SOG knots
        /// </summary>
        public Double GroundSpeedKnots { get; private set; }

        /// <summary>
        /// Number of SOG kmh
        /// </summary>
        public Double GroundSpeedkmh { get; private set; }

    }
}
