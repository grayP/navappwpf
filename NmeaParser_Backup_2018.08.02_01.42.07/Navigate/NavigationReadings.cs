using System;
using System.ComponentModel;
using System.Device.Location;
using System.Runtime.CompilerServices;

namespace NmeaParser.Navigate
{
    public class NavigationReadings : INotifyPropertyChanged
    {
        private DateTime _timeOfReading;
        private double _cogNow;
        private double _cogFast;
        private double _cogSlow;
        private double _cogSlowPrevious;
        private double _sogNow;
        private double _sogFast;
        private double _sogSlow;
        private double _sogShortAgo;
        private double _sogToPoint;
        private int _windDirection = 0;

        private double _lastSogNow = 0.0;
        private double _lastSogShort = 0.0;
        private double _lastSogLong = 0.0;
        private double _lastSogShortAgo = 0.0;

        private double _lastCogNow = 0;
        private double _LastCogFast = 0;
        private double _LastCogSlow = 0;
        private double _LastCogFastAgo;
        public GeoCoordinate LastPosition { get; set; }
        private Constants.Tack _tack ;
        public event PropertyChangedEventHandler PropertyChanged;
        #region Properties

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public DateTime TimeOfReading
        {
            get { return this._timeOfReading; }
            set
            {
                if (value != this._timeOfReading)
                {
                    this._timeOfReading = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double CogNow
        {
            get { return this._cogNow; }
            set
            {
                if (value != this._cogNow)
                {
                    this._cogNow = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double CogFast
        {
            get { return this._cogFast; }
            set
            {
                if (value != this._cogFast)
                {
                    this._cogFast = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double CogSlow
        {
            get { return this._cogSlow; }
            set
            {
                if (value != this._cogSlow)
                {
                    this._cogSlow = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double CogSlowPrevious
        {
            get { return this._cogSlowPrevious; }
            set
            {
                if (value != this._cogSlowPrevious)
                {
                    this._cogSlowPrevious = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double SogNow
        {
            get { return this._sogNow; }
            set
            {
                if (value != this._sogNow)
                {
                    this._sogNow = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double SogFast
        {
            get { return this._sogFast; }
            set
            {
                if (value != this._sogFast)
                {
                    this._sogFast = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double SogSlow
        {
            get { return this._sogSlow; }
            set
            {
                if (value != this._sogSlow)
                {
                    this._sogSlow = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double SogShortAgo
        {
            get { return this._sogShortAgo; }
            set
            {
                if (value != this._sogShortAgo)
                {
                    this._sogShortAgo = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double SogToPoint
        {
            get { return this._sogToPoint; }
            set
            {
                if (value != this._sogToPoint)
                {
                    this._sogToPoint = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public Constants.Tack   Tack
        {
            get { return this._tack; }
            set
            {
                if (value != this._tack)
                {
                    this._tack = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void SetTack(double bearing)
        {
            var diff = ((bearing - WindDirection + 180.0 + 360) % 360) - 180.0;
            if (diff > 0.0 && diff < 90.0)
            {
                Tack = Constants.Tack.Port;
            }
            else if
            (diff >= 90.0 && diff < 180)
            {
                Tack = Constants.Tack.PortRun;
            }
            else if (diff >= -180.0 && diff < -90)
            {
                Tack = Constants.Tack.StarBoardRun;
            }
            else
                Tack = Constants.Tack.StarBoard;
        }

        public int WindDirection
        {
            get { return this._windDirection; }
            set
            {
                if (value != this._windDirection)
                {
                    this._windDirection = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region Methods
        internal void SetTheSogValues(double speed)
        {
            SogNow = GetNewAverage(speed, ref _lastSogNow, .9);
            SogFast = GetNewAverage(speed, ref _lastSogShort, .5);
            SogSlow = GetNewAverage(speed, ref _lastSogLong, .3);
        }
        internal void SetSpeedToPoint()
        {
        }
        private double GetNewAverage(double speed, ref double lastSogValue, double alpha)
        {
            var newValue = Trig.AverageWeighted(speed, lastSogValue * 100, alpha) / 100.0;
            lastSogValue = newValue;
            return Math.Round(newValue, 2);
        }

        internal void SetTheCogValues(double bearing, AlphaValues alpha)
        {
            CogNow = Trig.GetNewAverageBearing(bearing, ref _lastCogNow, alpha.AlphaCogNow);
            CogFast = Trig.GetNewAverageBearing(bearing, ref _LastCogFast, alpha.AlphaCogFast);
            CogSlow = Trig.GetNewAverageBearing(bearing, ref _LastCogSlow, alpha.AlphaCogSlow);
        }
        #endregion
    }
}
