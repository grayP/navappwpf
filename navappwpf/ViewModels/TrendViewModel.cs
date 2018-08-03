using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using navappwpf.Common;
using navappwpf.Models;
using NmeaParser.Navigate;
using Telerik.Windows.Data;

namespace navappwpf.ViewModels
{
    internal class TrendViewModel : ViewModelBase, IDisposable
    {
        private NavigationDisplay _navigationDisplay;
        public NavigationDisplay NavigationDisplay { get { return _navigationDisplay; } set { SetProperty(ref _navigationDisplay, value); } }
        private DateTime _lastDate;
        //    private DateTime? alignmentDate;


        private int _minHeight;
        public int MinHeight
        {
            get { return _minHeight; }
            set
            {
                if (this._minHeight != value)
                {
                    this.MinHeight = value;
                    this.OnPropertyChanged("MinHeight");
                }
            }
        }

        private RadObservableCollection<ChartBusinessObject> _data;
        public RadObservableCollection<ChartBusinessObject> Data
        {
            get
            {
                return this._data;
            }
            set
            {
                if (this._data != value)
                {
                    this._data = value;
                    this.OnPropertyChanged("Data");
                }
            }
        }

        private void UpdateChartData()
        {
            Dispatcher.BeginInvoke((Action)delegate ()
              {
                  NavigationDisplay.FillData();
              });
        }


        public TrendViewModel(NavigationDisplay navigationDisplay) : base(new DispatcherWrapper())
        {
            _navigationDisplay = navigationDisplay;
            _minHeight = 300;
            StartTimer();
        }

        private void StartTimer()
        {
            var Timer = new Timer()
            {
                Interval = 1000,
                Enabled = true,
                AutoReset=true

            };
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
        }

        private void Timer_Elapsed(Object source, System.Timers.ElapsedEventArgs e)
        {
            UpdateChartData();
        }


        //private void FillData(Queue<NavigationDisplay.TackReading> tackReadings)
        //{
        //    RadObservableCollection<ChartBusinessObject> collection = new RadObservableCollection<ChartBusinessObject>();
        //    foreach (var tackReading in tackReadings)
        //    {
        //        this._lastDate = tackReading.TimeOfReading;
        //        collection.Add(CreateBusinessObject(tackReading.ReadingShort, tackReading.TimeOfReading));
        //    }
        //    this.Data = collection;
        //}
        //private ChartBusinessObject CreateBusinessObject(double value, DateTime dateTime)
        //{
        //    ChartBusinessObject obj = new ChartBusinessObject()
        //    {
        //        Value = value,
        //        Category = dateTime
        //    };
        //    return obj;
        //}


        private bool disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //base.StopDevice();
                    disposed = true;
                }
            }
        }
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
