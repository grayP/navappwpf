using navappwpf.Common;
using NmeaParser.Navigate;
using System;

namespace navappwpf.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        // private IDispatcher _dispatcher;
        private bool _isBusy;
        public bool IsBusy { get { return _isBusy; } set { SetProperty(ref _isBusy, value); } }
        private string _busyContent;
        public string BusyContent { get { return _busyContent; } set { SetProperty(ref _busyContent, value); } }
        public ViewModelBase CurrentViewModel { get { return _currentViewModel; } protected set { SetProperty(ref _currentViewModel, value); } }

        //private ProcessGps _processGps;
        //public ProcessGps ProcessGPS { get { return _processGps; }set { SetProperty(ref _processGps, value); } }

        public MainWindowViewModel(NavigationDisplay navigateDisplay) : base(new DispatcherWrapper())
        {
            Navigatedisplay = navigateDisplay;
            ExecuteNavCommand();
            SetInitialValues();
            //   ProcessGPS = new ProcessGps(navigateDisplay);
           // SetupPorts();

        }

        private void SetInitialValues()
        {
            Navigatedisplay.Alpha.AlphaCogNow = SetInitialValues(Navigatedisplay.Alpha.AlphaCogNow, Constants.Constants.AlphaCogNow);
            Navigatedisplay.Alpha.AlphaCogFast = SetInitialValues(Navigatedisplay.Alpha.AlphaCogFast, Constants.Constants.AlphaCogFast);
            Navigatedisplay.Alpha.AlphaCogSlow = SetInitialValues(Navigatedisplay.Alpha.AlphaCogSlow, Constants.Constants.AlphaCogSlow);
            Navigatedisplay.NavReadings.WindDirection = (int)SetInitialValues(Navigatedisplay.NavReadings.WindDirection, Constants.Constants.wind);
        }
        private double SetInitialValues(double value, string key)
        {
            if (value == 0.0)
            {
                return Convert.ToDouble(Helper.ApplicationSettingHelper.GetFromApplicationSetting(key));
            }
            return value;
        }

        public new void SetCurrentViewModel(ViewModelBase viewModel)
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
        public override void ExecuteDirSpeedCommand()
        {
            base.ExecuteDirSpeedCommand();
        }
        public override void ExecuteWindCommand()
        {
            base.ExecuteWindCommand();
        }

        public override void ExecuteNavCommand()
        {
            base.ExecuteNavCommand();
        }
        public override void ExecuteSettingsCommand()
        {
            base.ExecuteSettingsCommand();
        }

        public override void ExecuteTrendCommand( )
        {
            base.ExecuteTrendCommand();
        }

    }
}
