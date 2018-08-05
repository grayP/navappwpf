using System;
using navappwpf.Common;
using navappwpf.Models;
using NmeaParser.Navigate;
using Telerik.Windows.Data;

namespace navappwpf.ViewModels
{
    internal class TrendViewModel : ViewModelBase, IDisposable
    {
        private delegate void UiDelegate();
        private NavigationDisplay _navigationDisplay;
        public NavigationDisplay NavigationDisplay {
            get => _navigationDisplay;
            set
            {
                if (this._navigationDisplay == value) return;
                this._navigationDisplay = value;
                this.OnPropertyChanged("NavigationDisplay");

            }
        }

  
        private int _minHeight;
        public int MinHeight
        {
            get => _minHeight;
            set
            {
                if (this._minHeight == value) return;
                this.MinHeight = value;
                this.OnPropertyChanged("MinHeight");
            }
        }
        private RadObservableCollection<NmeaParser.Models.ChartBusinessObject> _data;
        public RadObservableCollection<NmeaParser.Models.ChartBusinessObject> Data
        {
            get => _data;
            set {
                if (this._data == value) return;
                this._data = value;
                this.OnPropertyChanged("Data"); }
        }

  
        public TrendViewModel(NavigationDisplay navigationDisplay) : base(new DispatcherWrapper())
        {

            _navigationDisplay = navigationDisplay;
           _minHeight = 300;
            Data = _navigationDisplay.ChartData;
        }


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
