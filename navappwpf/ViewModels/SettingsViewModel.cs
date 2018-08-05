﻿using System;
using navappwpf.Common;
using NmeaParser.Navigate;

namespace navappwpf.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(NavigationDisplay navigateDisplay) : base(new DispatcherWrapper())
        {
            Navigatedisplay = navigateDisplay;
        }
    }
}
