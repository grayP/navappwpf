using System;
using navappwpf.Common;
using NmeaParser.Navigate;

namespace navappwpf.ViewModels
{
    public class WindViewModel : ViewModelBase , IDisposable
    {
        public WindViewModel(NavigationDisplay navigateDisplay) : base(new DispatcherWrapper())
        {
            Navigatedisplay = navigateDisplay;
            SetInitialWind();
        }

        public override void ExecutePreviousCommand()
        {
            SaveWindData();
            base.ExecutePreviousCommand();
        }

        public override void ExecuteNextCommand()
        {
            SaveWindData();
            base.ExecuteNextCommand();
        }
        private void SetInitialWind()
        {
            if (Navigatedisplay.NavReadings.WindDirection == 0)
            {
                Navigatedisplay.NavReadings.WindDirection = Convert.ToInt32(Helper.ApplicationSettingHelper.GetFromApplicationSetting(Constants.Constants.wind));
            }
        }
        private void SaveWindData()
        {
            Helper.ApplicationSettingHelper.SaveApplicationSetting(Navigatedisplay.NavReadings.WindDirection.ToString(), Constants.Constants.wind);
        }
        private bool disposed;
        protected virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {
                    SaveWindData();
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
