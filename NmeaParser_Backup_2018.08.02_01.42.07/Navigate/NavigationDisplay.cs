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
        private int _messagecounter = 0;
        public bool DeviceStarted { get; set; }
        public string LocalDeviation { get; set; }
        private readonly double LocalDev = 0.0;
        private double _lastLatitude = 0;
        private double _lastLongitude = 0;
        private double _lastLatitude2 = 0;
        private double _lastLongitude2 = 0;
        private double _lastLatitude3 = 0;
        private double _lastLongitude3 = 0;

        public ChartDisplay SogChart { get; set; } = new ChartDisplay();
        public ChartDisplay CogChart { get; set; } = new ChartDisplay();

        private GeoCoordinate DroppedPoint = new GeoCoordinate();
        private DateTime DroppedPointTime;

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
            get => this._numReadings;
            set
            {
                if (value == this._numReadings) return;
                this._numReadings = value;
                NotifyPropertyChanged();
            }
        }

        public NavigationDisplay()
        {
            NavReadings = new NavigationReadings();
            Alpha = new AlphaValues();
            CourseReadings = new CourseReading();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #region Properties
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
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
            get => _alphaValues;
            set
            {
                if (value == this._alphaValues) return;
                this._alphaValues = value;
                NotifyPropertyChanged();
            }
        }

        public void Reset()
        {
            DroppedPoint = NavReadings.LastPosition;
            DroppedPointTime = NavReadings.TimeOfReading;
        }
        #endregion
        public bool IsGoodMessage(Gpgga newReading)
        {
            return newReading.NumberOfSatellites >= 3 && Math.Abs(newReading.Latitude) > 0;
        }
        public void ParseNmeaMessage(Gpgga message)
        {
            if (NavMessages.Count > 1000) NavMessages.Dequeue();
            NavMessages.Enqueue(message);
            // Queue<Gpgga> orderedQueue = new Queue<Gpgga>(NavMessages.OrderBy(z=>z.FixTime));

            if (Math.Abs(_lastLatitude) > 0)
            {
                NavReadings.LastPosition = new GeoCoordinate(message.Latitude, message.Longitude);
                var bearing = Trig.GetBearing(_lastLatitude3, _lastLongitude3, message.Latitude, message.Longitude, LocalDev);
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
            _lastLatitude3 = _lastLatitude2;
            _lastLongitude3 = _lastLongitude2;
            _lastLatitude2 = _lastLatitude;
            _lastLongitude2 = _lastLongitude;
            _lastLatitude = message.Latitude;
            _lastLongitude = message.Longitude;
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
            try
            {
                if (TackReadings.Count > 1000)
                    TackReadings.Dequeue();
                //navReadings.CogSlowPrevious = TackReadings.FirstOrDefault(x => x.TimeOfReading > navReadings.TimeOfReading.AddSeconds(-120) && x.CurrentTack == navReadings.Tack) == null ? 0 : TackReadings.FirstOrDefault(x => x.TimeOfReading > navReadings.TimeOfReading.AddSeconds(-120) && x.CurrentTack == navReadings.Tack).ReadingLong;
                var newReading = new TackReading(navReadings);
                TackReadings.Enqueue(newReading);
                AddData(newReading);
                CogChart.SetmaxMin(10);
                SogChart.SetmaxMin(1);
                SetSogToDroppedPoint(navReadings.LastPosition, navReadings.TimeOfReading);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void SetSogToDroppedPoint(GeoCoordinate lastposition, DateTime timeOfLastReading)
        {
            if (double.IsNaN(DroppedPoint.Latitude)) return;
            if (timeOfLastReading == DroppedPointTime) return;
            NavReadings.SogToPoint = Math.Round(DroppedPoint.GetDistanceTo(lastposition) / (timeOfLastReading - DroppedPointTime).TotalSeconds * .5144, 2);
        }

        private void AddData(TackReading newReading)
        {
            try
            {
                CogChart.ChartDataFast = UpdateCollection(CogChart.FastCollection, newReading.TimeOfReading, newReading.ReadingShort, newReading.CurrentTack);
                CogChart.ChartDataSlow = UpdateCollection(CogChart.SlowCollection, newReading.TimeOfReading, newReading.ReadingLong, newReading.CurrentTack);
                SogChart.ChartDataSlow = UpdateCollection(SogChart.SlowCollection, newReading.TimeOfReading, newReading.ReadingSpeedLong, newReading.CurrentTack);
                SogChart.ChartDataFast = UpdateCollection(SogChart.FastCollection, newReading.TimeOfReading, newReading.ReadingSpeedShort, newReading.CurrentTack);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private RadObservableCollection<ChartBusinessObject> UpdateCollection(RadObservableCollection<ChartBusinessObject> collection, DateTime dateTime, double value, Tack Tack)
        {
            try
            {
                if (collection.Count > Math.Max(NumReadings, 30)) collection.Take(Math.Max(NumReadings - 1, 30));
                collection.Add(new ChartBusinessObject()
                {
                    ReadingDateTime = dateTime,
                    Value = value,
                    tack = Tack
                });
                return collection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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

    }

}
