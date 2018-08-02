using System.Globalization;


namespace NmeaParser.Nmea.Gps
{
    /// <summary>
    ///  GLONASS Satellites in view
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gpgsv")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [NmeaMessageType("GPGSV")]
    public sealed class Gpgsv : Gsv
    {
    }

    /// <summary>
    /// Satellite vehicle
    /// </summary>
    public sealed class SatelliteVehicle
    {
        internal SatelliteVehicle(string[] message, int startIndex)
        {
            PrnNumber = int.Parse(message[startIndex], CultureInfo.InvariantCulture);
            Elevation = double.Parse(message[startIndex + 1], CultureInfo.InvariantCulture);
            Azimuth = double.Parse(message[startIndex + 2], CultureInfo.InvariantCulture);
            int snr = -1;
            if (int.TryParse(message[startIndex + 3], out snr))
                SignalToNoiseRatio = snr;
        }
        /// <summary>
        /// SV PRN number
        /// </summary>
        public int PrnNumber { get; set; }
        /// <summary>
        /// Elevation in degrees, 90 maximum
        /// </summary>
        public double Elevation { get; private set; }
        /// <summary>
        /// Azimuth, degrees from true north, 000 to 359
        /// </summary>
        public double Azimuth { get; private set; }
        /// <summary>
        /// Signal-to-Noise ratio, 0-99 dB (-1 when not tracking) 
        /// </summary>
        public int SignalToNoiseRatio { get; private set; }

        /// <summary>
        /// Satellite system
        /// </summary>
        public SatelliteSystem System
        {
            get
            {
                if (PrnNumber >= 1 && PrnNumber <= 32)
                    return SatelliteSystem.Gps;
                if (PrnNumber >= 33 && PrnNumber <= 64)
                    return SatelliteSystem.Waas;
                if (PrnNumber >= 65 && PrnNumber <= 96)
                    return SatelliteSystem.Glonass;
                return SatelliteSystem.Unknown;
            }
        }
    }

    /// <summary>
    /// Satellite system
    /// </summary>
    public enum SatelliteSystem
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown,
        /// <summary>
        /// GPS - Global Positioning System (NAVSTAR)
        /// </summary>
        Gps,
        /// <summary>
        /// WAAS - Wide Area Augmentation System
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Waas")]
        Waas,
        /// <summary>
        /// GLONASS - Globalnaya navigatsionnaya sputnikovaya sistema
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Glonass")]
        Glonass,
        /// <summary>
        /// Galileo
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Galileo")]
        Galileo
    }
}