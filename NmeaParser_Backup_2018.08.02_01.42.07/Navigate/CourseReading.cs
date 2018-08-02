using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NmeaParser.Navigate
{
    public class CourseReading : INotifyPropertyChanged
    {
        private double _distance;
        private double _bearingToWaypoint;
        private double _turn;
        private double _xtrack;
        private int _secondsToTurn;
        private int _nextCourse;


        public event PropertyChangedEventHandler PropertyChanged;
        #region Properties

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public double Distance
        {
            get { return this._distance; }
            set
            {
                if (value != this._distance)
                {
                    this._distance = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double BearingToWayPoint
        {
            get { return this._bearingToWaypoint; }
            set
            {
                if (value != this._bearingToWaypoint)
                {
                    this._bearingToWaypoint = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double XTrack
        {
            get { return this._xtrack; }
            set
            {
                if (value != this._xtrack)
                {
                    this._xtrack = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double Turn
        {
            get { return this._turn; }
            set
            {
                if (value != this._turn)
                {
                    this._turn = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int SecondsToTurn
        {
            get { return this._secondsToTurn; }
            set
            {
                if (value != this._secondsToTurn)
                {
                    this._secondsToTurn = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int NextCourse
        {
            get { return this._nextCourse; }
            set
            {
                if (value != this._nextCourse)
                {
                    this._nextCourse = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
#endregion
}
