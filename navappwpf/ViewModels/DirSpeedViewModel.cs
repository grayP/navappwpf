using NmeaParser.Models;
using NmeaParser.Navigate;
using System;
using navappwpf.Common;


namespace navappwpf.ViewModels
{
    internal class DirSpeedViewModel : ViewModelBase, IDisposable
    {
        public DirSpeedViewModel(NavigationDisplay navigateDisplay) : base(new DispatcherWrapper())
        {}
        public override void ExecutePreviousCommand()
        {
            base.ExecutePreviousCommand();
        }
        public override void ExecuteNextCommand()
        {
            base.ExecuteNextCommand();
        }
        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
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