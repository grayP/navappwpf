using NmeaParser.Constants;
using NmeaParser.Navigate;
using System;

namespace NmeaParser.Models
{
    public class TackReading
    {
        public DateTime TimeOfReading { get; set; }
        public Tack CurrentTack { get; set; }
        public double ReadingNow { get; set; }
        public double ReadingShort { get; set; }
        public double ReadingLong { get; set; }
        public double ReadingShortPast { get; set; }
        public double ReadingSpeedShort { get; set; }
        public double ReadingSpeedLong { get; set; }
        public TackReading(NavigationReadings navReading)
        {
            TimeOfReading = navReading.TimeOfReading;
            CurrentTack = navReading.Tack;
            ReadingNow = navReading.CogNow;
            ReadingShort = navReading.CogFast;
            ReadingLong = navReading.CogSlow;
            ReadingSpeedShort = navReading.SogShort;
            ReadingSpeedLong = navReading.SogLong;
        }
    }
}
