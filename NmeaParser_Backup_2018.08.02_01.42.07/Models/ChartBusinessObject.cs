using System;
using NmeaParser.Constants;

namespace NmeaParser.Models
{ 
    public class ChartBusinessObject
    {
        public Tack tack { get; set; }
        public double Value { get; set; }
        public DateTime ReadingDateTime { get; set; }
    }
}
