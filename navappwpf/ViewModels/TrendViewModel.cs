using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
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
        private bool _cogChartVisible;
        public bool CogChartVisible
        {
            get
            {
                return _cogChartVisible;
            }
            set
            {
                _cogChartVisible = value;
                this.OnPropertyChanged("CogChartVisible");
            }
        }
        private bool _sogChartVisible;
        public bool SogChartVisible
        {
            get
            {
                return _sogChartVisible;
            }
            set
            {
                _sogChartVisible = value;
                this.OnPropertyChanged("SogChartVisible");
            }
        }
        private DelegateCommand _switchCommand { get; set; }
        public ICommand SwitchCommand { get { if (_switchCommand == null) { _switchCommand = new DelegateCommand(ExecuteSwitchCommand, CanExecuteSwitchCommand); } return _switchCommand; } }
        private DelegateCommand _resetCommand { get; set; }
        public ICommand ResetCommand { get { if (_resetCommand == null) { _resetCommand = new DelegateCommand(ExecuteResetCommand, CanExecuteResetCommand); } return _resetCommand; } }


        public TrendViewModel(NavigationDisplay navigationDisplay) : base(new DispatcherWrapper())
        {
            NavigationDisplay = navigationDisplay;
            _minHeight = (int)(((System.Windows.Controls.Panel)Application.Current.MainWindow.Content).ActualHeight) - 150;
            SogChartVisible = false;
            CogChartVisible = true;
        }

        public NavigationDisplay NavigationDisplay
        {
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
        public virtual void ExecuteSwitchCommand()
        {
            ExecuteActionInBackground(
                () =>
                {
                    SogChartVisible = CogChartVisible;
                    CogChartVisible = !CogChartVisible;
                });
        }
        public virtual bool CanExecuteSwitchCommand()
        { return true; }

        public virtual void ExecuteResetCommand()
        {
            NavigationDisplay.Reset();
        }
        public virtual bool CanExecuteResetCommand()
        { return true; }



        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                //base.StopDevice();
                _disposed = true;
            }
        }
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
