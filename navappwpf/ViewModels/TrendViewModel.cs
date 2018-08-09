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
        Timer timer;
        private DelegateCommand _switchCommand { get; set; }

        public ICommand SwitchCommand { get { if (_switchCommand == null) { _switchCommand = new DelegateCommand(ExecuteSwitchCommand, CanExecuteSwitchCommand); } return _switchCommand; } }

        public TrendViewModel(NavigationDisplay navigationDisplay) : base(new DispatcherWrapper())
        {
            NavigationDisplay = navigationDisplay;
            _minHeight = (int)(((System.Windows.Controls.Panel)Application.Current.MainWindow.Content).ActualHeight) - 300;
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
                  
                },
                () => { SetCurrentViewModel(new TrendViewModel(Navigatedisplay)); });
        }
        public virtual bool CanExecuteSwitchCommand()
        { return true; }


        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    //base.StopDevice();
                    _disposed = true;
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
