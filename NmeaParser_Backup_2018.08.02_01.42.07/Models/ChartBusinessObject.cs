using System;
namespace NmeaParser.Models
{ 
    public class ChartBusinessObject
    {
        public int Counter { get; set; }
        public double ImmediateValue { get; set; }
        public double ShortValue { get; set; }
        public double LongValue { get; set; }
        public DateTime ReadingDateTime { get; set; }
    }
}
