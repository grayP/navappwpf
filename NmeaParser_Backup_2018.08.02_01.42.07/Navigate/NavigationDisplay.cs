using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using NmeaParser.Constants;
using NmeaParser.Helper;
using NmeaParser.Models;
using NmeaParser.Nmea.Gps;
using Telerik.Windows.Data;

namespace NmeaParser.Navigate
{
    public class NavigationDisplay : INotifyPropertyChanged
    {
        private int messagecounter = 0;
        public bool DeviceStarted { get; set; }
        public string LocalDeviation { get; set; }
        private readonly double LocalDev = 0.0;
        private double LastLatitude = 0;
        private double LastLongitude = 0;
        private double LastLatitude2 = 0;
        private double LastLongitude2 = 0;
        private double LastLatitude3 = 0;
        private double LastLongitude3 = 0;
        private RadObservableCollection<ChartBusinessObject> collection = new RadObservableCollection<ChartBusinessObject>();
        private int _yAxisMaximum;
        public int YAxisMaximum
        {
            get { return _yAxisMaximum; }
            set
            {
                if (this.YAxisMaximum != value)
                {
                    this._yAxisMaximum = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private int _yAxisMinimum;
        public int YAxisMinimum
        {
            get { return _yAxisMinimum; }
            set
            {
                if (this.YAxisMinimum != value)
                {
                    this._yAxisMinimum = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private DateTime _minXaxis;
        public DateTime MinXaxis
        {
            get { return _minXaxis; }
            set
            {
                if (this._minXaxis != value)
                {
                    this._minXaxis = value;
                    NotifyPropertyChanged();
                }
            }
        }

        //  private GeoCoordinate lastCoordinate;
        private TimeSpan lastTimeSpan;
        private Queue<Gpgga> NavMessages = new Queue<Gpgga>(1001);
        public Queue<TackReading> TackReadings { get; set; } = new Queue<TackReading>();
        private NavigationReadings _navReadings = new NavigationReadings();
        private AlphaValues _alphaValues;
        private CourseReading _courseReadings;
        private int _numReadings;
        public int NumReadings
        {
            get { return this._numReadings; }
            set
            {
                if (value != this._numReadings)
                {
                    this._numReadings = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public NavigationDisplay()
        {
            NavReadings = new NavigationReadings();
            Alpha = new AlphaValues();
            CourseReadings = new CourseReading();
            // FillData(TackReadings);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #region Properties
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NavigationReadings NavReadings
        {
            get { return this._navReadings; }
            set
            {
                if (value != this._navReadings)
                {
                    this._navReadings = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public CourseReading CourseReadings
        {
            get { return this._courseReadings; }
            set
            {
                if (value != this._courseReadings)
                {
                    this._courseReadings = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public AlphaValues Alpha
        {
            get { return this._alphaValues; }
            set
            {
                if (value != this._alphaValues)
                {
                    this._alphaValues = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private RadObservableCollection<ChartBusinessObject> _chartData;
        public RadObservableCollection<ChartBusinessObject> ChartData
        {
            get
            {
                return this._chartData;
            }
            set
            {
                if (this._chartData != value)
                {
                    this._chartData = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion
        public class TackReading
        {
            public DateTime TimeOfReading { get; set; }
            public Tack CurrentTack { get; set; }
            public double ReadingNow { get; set; }
            public double ReadingShort { get; set; }
            public double ReadingLong { get; set; }
            public double ReadingShortPast { get; set; }
            public TackReading(NavigationReadings navReadings)
            {
                TimeOfReading = navReadings.TimeOfReading;
                CurrentTack = navReadings.Tack;
                ReadingNow = navReadings.CogNow;
                ReadingShort = navReadings.CogFast;
                ReadingLong = navReadings.CogSlow;
            }
        }
        public void ParseNmeaMessage(Gpgga message)
        {
            if (NavMessages.Count > 1000) NavMessages.Dequeue();
            NavMessages.Enqueue(message);
           // Queue<Gpgga> orderedQueue = new Queue<Gpgga>(NavMessages.OrderBy(z=>z.FixTime));

            if (Math.Abs(LastLatitude) > 0)
            {
                NavReadings.LastPosition = new GeoCoordinate(message.Latitude, message.Longitude);
                var bearing = Trig.GetBearing(LastLatitude3, LastLongitude3, message.Latitude, message.Longitude, LocalDev);
                NavReadings.TimeOfReading = CreateTime(message.FixTime);
                var speed = FindSogFromQueue(0, 15);
                NavReadings.SetTheCogValues(bearing, Alpha);
                NavReadings.SetTheSogValues(speed);
                NavReadings.SetTack(bearing);
                lastTimeSpan = message.FixTime;

                //only start plotting once three messages come through
                ManageQueue(NavReadings);
            }
            //Set past message values
                LastLatitude3 = LastLatitude2;
                LastLongitude3 = LastLongitude2;
                LastLatitude2 = LastLatitude;
                LastLongitude2 = LastLongitude;
                LastLatitude = message.Latitude;
                LastLongitude = message.Longitude;

        }
        public void GetCourseCorrections(Course course)
        {
            try
            {
                var xCoord = course.WhereShouldIBe(NavReadings.LastPosition);
                CourseReadings.Distance = Math.Round(xCoord.GetDistanceTo(NavReadings.LastPosition), 0);
                CourseReadings.BearingToWayPoint = Math.Round(Trig.GetBearing(NavReadings.LastPosition.Latitude, NavReadings.LastPosition.Longitude, course.EndWayPoint.Latitude, course.EndWayPoint.Longitude, double.Parse(course.LocalDeviation)), 0);
                CourseReadings.Turn = Math.Round((CourseReadings.BearingToWayPoint - NavReadings.CogNow + 180 + 360) % 360 - 180, 0);
                CourseReadings.SecondsToTurn = (int)xCoord.Altitude;
                CourseReadings.NextCourse = (int)xCoord.HorizontalAccuracy;
                CourseReadings.XTrack = xCoord.Speed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ManageQueue(NavigationReadings navReadings)
        {
            if (TackReadings.Count > 1000)
                TackReadings.Dequeue();

            navReadings.CogSlowPrevious = TackReadings.FirstOrDefault(x => x.TimeOfReading > navReadings.TimeOfReading.AddSeconds(-15) && x.CurrentTack == navReadings.Tack) == null ? 0 : TackReadings.FirstOrDefault(x => x.TimeOfReading > navReadings.TimeOfReading.AddSeconds(-15) && x.CurrentTack == navReadings.Tack).ReadingLong;

            var NewReading = new TackReading(navReadings);
            TackReadings.Enqueue(NewReading);
            AddData(NewReading);
            SetMaxMin();
        }

        private void AddData(TackReading newReading)
        {
            messagecounter += 1;
            if (collection.Count > Math.Max(NumReadings,30)) collection.RemoveAt(0);
            collection.Add(new ChartBusinessObject()
            {
                Counter=messagecounter,
                ReadingDateTime = newReading.TimeOfReading,
                ShortValue = newReading.ReadingShort,
                LongValue = newReading.ReadingLong,
                ImmediateValue= newReading.ReadingNow
            });
            this.ChartData = collection;
        }

        private void SetMaxMin()
        {
            var newMin = ExtensionMethods.RoundToNearest((int)collection.Min(x => x.ShortValue), 10.0) - 20;
            if (newMin < YAxisMinimum || newMin > YAxisMinimum + 19) YAxisMinimum = newMin;
            YAxisMaximum = YAxisMinimum + 20;
            var newMax = ExtensionMethods.RoundToNearest((int)collection.Max(x => x.ShortValue), 20.0) + 20;
            if (newMax > YAxisMaximum || newMax < YAxisMaximum - 19) YAxisMaximum = newMax;
            MinXaxis = collection.OrderBy(x => x.ReadingDateTime).FirstOrDefault().ReadingDateTime;
        }

        public static DateTime CreateTime(TimeSpan fixTime)
        {
            var result = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, fixTime.Hours, fixTime.Minutes, fixTime.Seconds).ToLocalTime();
            return result.Day > DateTime.Now.Day ? result.AddDays(-1) : result;
        }
        private double FindSogFromQueue(int start, int n)
        {
            if (NavMessages.Count() < n) return 0;
            var lastN = NavMessages.Reverse().Take(n).ToList();
            var firstPoint = lastN[start];
            var lastPoint = lastN[n - 1];

            var firstCoord = new GeoCoordinate(firstPoint.Latitude, firstPoint.Longitude);
            var lastCoord = new GeoCoordinate(lastPoint.Latitude, lastPoint.Longitude);
            return Math.Round(firstCoord.GetDistanceTo(lastCoord) / (firstPoint.FixTime - lastPoint.FixTime).TotalSeconds * 3.6 / 1.852 * 100, 0);
        }

        public bool IsGoodMessage(Gpgga newReading)
        {
            return newReading.NumberOfSatellites >= 3 && Math.Abs(newReading.Latitude) > 0;
        }
    }

}
