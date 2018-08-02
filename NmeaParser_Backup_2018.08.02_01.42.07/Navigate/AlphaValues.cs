using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NmeaParser.Navigate
{
    public class AlphaValues : INotifyPropertyChanged
    {
        private double _alphaCogNow;
        private double _alphaCogFast;
        private double _alphaCogSlow;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

            public double AlphaCogNow
        {
            get { return this._alphaCogNow; }
            set
            {
                if (value != this._alphaCogNow)
                {
                    this._alphaCogNow = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double AlphaCogSlow
        {
            get { return this._alphaCogSlow; }
            set
            {
                if (value != this._alphaCogSlow)
                {
                    this._alphaCogSlow = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double AlphaCogFast
        {
            get { return this._alphaCogFast; }
            set
            {
                if (value != this._alphaCogFast)
                {
                    this._alphaCogFast = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}