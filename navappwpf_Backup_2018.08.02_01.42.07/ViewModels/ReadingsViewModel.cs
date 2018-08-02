using System;
using navappwpf.Common;
using NmeaParser.Navigate;

namespace navappwpf.ViewModels
{
    internal class ReadingsViewModel : ViewModelBase, IDisposable
    {
        private NavigationDisplay _navigationDisplay;
        public NavigationDisplay NavigationDisplay { get { return _navigationDisplay; } set { SetProperty(ref _navigationDisplay, value); } }

        public ReadingsViewModel(NavigationDisplay navigationDisplay) : base(new DispatcherWrapper())
        {
            _navigationDisplay = navigationDisplay;

        }
        public override void ExecutePreviousCommand()
        {
            base.ExecutePreviousCommand();
        }
        public override void ExecuteNextCommand()
        {
            base.ExecuteNextCommand();
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
