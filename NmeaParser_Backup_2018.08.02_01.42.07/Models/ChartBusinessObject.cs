using System;


namespace NmeaParser.Models
{ 

    public class ChartBusinessObject
    {
        private int _counter;
        public int Counter
        {
            get
            {
                return this._counter;
            }
            set
            {
                this._counter = value;
            }
        }
        private DateTime _readingDateTime;
        private double _immediate;
        public double ImmediateValue

        {
            get
            {
                return this._immediate;
            }
            set
            {
                this._immediate = value;
            }
        }
        private double _shortValue;
        public double ShortValue

        {
            get
            {
                return this._shortValue;
            }
            set
            {
                this._shortValue = value;
            }
        }
        private double _longValue;
        public double LongValue
        {
            get { return this._longValue; }
            set { this._longValue = value; }
        }

        public DateTime ReadingDateTime
        {
            get
            {
                return this._readingDateTime;
            }
            set
            {
                this._readingDateTime = value;
            }
        }
    }
}
