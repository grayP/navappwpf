using navappwpf.Common;
using NmeaParser.Helper;

namespace navappwpf.ViewModels
{
   public class MainWindowViewModel: ObservableObject
    {

        private ViewModelBase _currentViewModel;
       // private IDispatcher _dispatcher;
        private bool _isBusy;
        public bool IsBusy { get { return _isBusy; } set { SetProperty(ref _isBusy, value); } }
        private string _busyContent;
        public string BusyContent { get { return _busyContent; } set { SetProperty(ref _busyContent, value); } }
        public ViewModelBase CurrentViewModel { get { return _currentViewModel; } protected set { SetProperty(ref _currentViewModel, value); } }

 
        public void SetCurrentViewModel(ViewModelBase viewModel)
        {
            if (viewModel != null)
            {
                CurrentViewModel = viewModel;
            }
        }
        /// <summary>
        /// Show progress/busy indicator
        /// </summary>
        /// <param name="showBusyIndicator">Pass <code>true</code> to show indicator. <code>false</code> to hide it.</param>
        /// <param name="busyIndicatorText">Text to display on Busy Indicator. In case of <code>null</code>, <code>Constants.BUSY_CONTENT</code> is displayed.</param>
        public void SetBusyIndicator(bool showBusyIndicator, string busyIndicatorText = null)
        {
            if (string.IsNullOrEmpty(busyIndicatorText))
                BusyContent = Constants.Constants.Busy_Content;
            else
                BusyContent = busyIndicatorText;

            IsBusy = showBusyIndicator;
        }
    }
}
