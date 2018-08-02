using System;


namespace NmeaParser.Models
{ 

    public class ChartBusinessObject
    {
        private double _value;
        private DateTime _category;
        private double _longValue;
        public double Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
        public double LongValue
        {
            get { return this._longValue; }
            set { this._longValue = value; }
        }

        public DateTime Category
        {
            get
            {
                return this._category;
            }
            set
            {
                this._category = value;
            }
        }
    }
}
