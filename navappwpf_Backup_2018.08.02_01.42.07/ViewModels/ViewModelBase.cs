using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using navappwpf.Common;
using NmeaParser.Navigate;

namespace navappwpf.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        protected Window MainWindow { get { return Application.Current.MainWindow; } }
        protected MainWindowViewModel MainWindowDataContext { get { return Application.Current.MainWindow != null ? (MainWindowViewModel)Application.Current.MainWindow.DataContext : null; } }
        private NavigationDisplay _navigate;
        public NavigationDisplay Navigatedisplay { get { return _navigate; } set { SetProperty(ref _navigate, value); } }



        public IDispatcher Dispatcher { get; set; }
        public ViewModelBase(IDispatcher dispatcher)
        {
            this.Dispatcher = dispatcher;
        }
      

        private DelegateCommand _nextCommand { get; set; }
        public ICommand NextCommand { get { if (_nextCommand == null) { _nextCommand = new DelegateCommand(ExecuteNextCommand, CanExecuteNextCommand); } return _nextCommand; } }

        private DelegateCommand _previousCommand { get; set; }
        public ICommand PreviousCommand { get { if (_previousCommand == null) { _previousCommand = new DelegateCommand(ExecutePreviousCommand, CanExecutePreviousCommand); } return _previousCommand; } }
        private DelegateCommand _dirSpeedCommand { get; set; }
        public ICommand DirSpeedCommand { get { if (_dirSpeedCommand == null) { _dirSpeedCommand = new DelegateCommand(ExecuteDirSpeedCommand, CanExecuteDirSpeedCommand); } return _dirSpeedCommand; } }

        private DelegateCommand _navCommand { get; set; }
        public ICommand NavCommand { get { if (_navCommand == null) { _navCommand = new DelegateCommand(ExecuteNavCommand, CanExecuteNavCommand); } return _navCommand; } }
        private DelegateCommand _windCommand { get; set; }
        public ICommand WindCommand { get { if (_windCommand == null) { _windCommand = new DelegateCommand(ExecuteWindCommand, CanExecuteWindCommand); } return _windCommand; } }
        private DelegateCommand _settingCommand { get; set; }
        public ICommand SettingCommand { get { if (_settingCommand == null) { _settingCommand = new DelegateCommand(ExecuteSettingsCommand, CanExecuteSettingsCommand); } return _settingCommand; } }
        private DelegateCommand _trendCommand { get; set; }
        public ICommand TrendCommand { get { if (_trendCommand == null) { _trendCommand = new DelegateCommand(ExecuteTrendCommand, CanExecuteTrendCommand); } return _trendCommand; } }

        public virtual bool CanExecuteTrendCommand()
        { return true; }

        public virtual bool CanExecuteDirSpeedCommand()
        {
            return true;
        }

        public virtual bool CanExecuteWindCommand()
        {
            return true;
        }
        public virtual bool CanExecuteSettingsCommand()
        {
            return true;
        }
        public virtual bool CanExecuteNavCommand()
        {
            return true;
        }

        public virtual bool CanExecuteNextCommand()
        {
            return true;
        }
        public virtual bool CanExecutePreviousCommand()
        {
            return true;
        }


        /// <summary>
        /// Show progress/busy indicator
        /// </summary>
        /// <param name="showBusyIndicator">Pass <code>true</code> to show indicator. <code>false</code> to hide it.</param>
        protected void ShowBusyIndicator(bool showBusyIndicator)
        {
            if (Dispatcher != null)
            {
                Dispatcher.BeginInvoke((Action)(delegate
                {
                    MainWindowDataContext.SetBusyIndicator(showBusyIndicator);
                }));
            }
        }
        /// <summary>
        /// Show progress/busy indicator with text
        /// </summary>
        /// <param name="busyIndicatorText">Text to display on Busy Indicator.</param>
        protected void ShowBusyIndicator(string busyIndicatorText)
        {
            if (Dispatcher != null)
            {
                Dispatcher.BeginInvoke((Action)(delegate
                {
                    MainWindowDataContext.SetBusyIndicator(true, busyIndicatorText);
                }));
            }
        }

        protected void UpdateData()
        {
            if (Dispatcher != null)
            {
                Dispatcher.BeginInvoke((Action)(delegate
                {
                    Navigatedisplay.FillData();
                }));
            }
        }


        public virtual void ExecuteWindCommand()
        {
            ExecuteActionInBackground(
                () =>
                {
                    SaveModel();
                    Dispose();
                },
                () => { SetCurrentViewModel(new WindViewModel(Navigatedisplay)); });
        }
        public virtual void ExecuteDirSpeedCommand()
        {
            ExecuteActionInBackground(
               () =>
               {
                   SaveModel();
                   Dispose();
               },
               () =>
               {
                   SetCurrentViewModel(new DirSpeedViewModel(Navigatedisplay));
               });
        }
        public virtual void ExecuteNavCommand()
        {
            ExecuteActionInBackground(
                () =>
                {
                    SaveModel();
                    Dispose();
                },
                () =>
                {
                    SetCurrentViewModel(new ReadingsViewModel(Navigatedisplay));
                });
        }

        public virtual void ExecuteTrendCommand()
        {
            ExecuteActionInBackground(
                () =>
                {
                    SaveModel();
                    Dispose();
                },
                () => { SetCurrentViewModel(new TrendViewModel(Navigatedisplay)); });
        }

        public virtual void ExecuteSettingsCommand()
        {
            ExecuteActionInBackground(
                () =>
                {
                    SaveModel();
                    Dispose();
                },
                () => { SetCurrentViewModel(new SettingsViewModel(Navigatedisplay)); });
        }

        public virtual void ExecutePreviousCommand()
        {
            ExecuteActionInBackground(
                () =>
                {
                    SaveModel();
                    Dispose();
                },
                () => { SetCurrentViewModel(GetPreviousViewModel(this)); });
        }



        public virtual void ExecuteNextCommand()
        {
            ExecuteActionInBackground(
                () =>
                {
                    SaveModel();
                    Dispose();
                },// save model to database
                () => { SetCurrentViewModel(GetNextViewModel(this)); });// goto next view
        }

        public ViewModelBase GetNextViewModel(ViewModelBase currentViewModel)
        {
            Type type = currentViewModel.GetType();
            /***********************/
            /* 1General Navigation */
            /***********************/
            if (type == typeof(ReadingsViewModel))
            {
                return new WindViewModel(Navigatedisplay);
            }
            else if (type == typeof(WindViewModel) || type == typeof(SettingsViewModel))
            {
                return new SettingsViewModel(Navigatedisplay);
            }

            else
                return new ReadingsViewModel(Navigatedisplay);
        }


        public ViewModelBase GetPreviousViewModel(ViewModelBase currentViewModel)
        {
            Type type = currentViewModel.GetType();
            if (type == typeof(ReadingsViewModel) || type == typeof(WindViewModel))
            {
                return new ReadingsViewModel(Navigatedisplay);
            }
            else if (type == typeof(SettingsViewModel))
            {
                return new WindViewModel(Navigatedisplay);
            }
            else
                return new ReadingsViewModel(Navigatedisplay);
        }



        protected void SetCurrentViewModel(ViewModelBase viewModel)
        {
            MainWindowDataContext.SetCurrentViewModel(viewModel);
            viewModel.Dispatcher = Dispatcher;
        }

        protected void ExecuteActionInBackground(Action doWork, Action workComplete = null, string busyIndicatorMessage = "")
        {
            ExecuteActionInBackground(doWork, workComplete, busyIndicatorMessage, 100);
        }

        protected void ExecuteActionInBackground(Action doWork, Action workComplete, string busyIndicatorMessage, int startWorkAfter)
        {
            ShowBusyIndicator(busyIndicatorMessage);

            try
            {
                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += (sender, e) =>
                {
                    doWork();
                };
                worker.RunWorkerCompleted += (sender, e) =>
                {
                    string error = string.Empty;
                    if (e.Error != null)
                    {
                        error = e.Error.InnerException != null ? e.Error.InnerException.Message : e.Error.Message;
                    }
                    else if (e.Result != null && e.Result.GetType().Equals(typeof(string)))
                    {
                        error = Convert.ToString(e.Result);
                    }

                    ShowBusyIndicator(false);

                    (sender as BackgroundWorker).Dispose();

                    if (Dispatcher != null)
                    {
                        Dispatcher.BeginInvoke((Action)(delegate
                        {
                            if (!string.IsNullOrEmpty(error))
                            {
                                ShowMessage(error);
                            }

                            workComplete?.Invoke();
                            //if (workComplete != null)
                            //{
                            //    workComplete();
                            //}
                        }));
                    }
                };

                DispatcherTimer timer = new DispatcherTimer()
                {
                    Interval = TimeSpan.FromMilliseconds(startWorkAfter)
                };
                timer.Tick += (sender, e) =>
                {
                    worker.RunWorkerAsync();
                    (sender as DispatcherTimer).Stop();
                };
                timer.Start();
            }
            catch
            {
                ShowBusyIndicator(false);
            }
        }


        protected void ShowMessage(string messageBoxText)
        {
            if (Dispatcher != null)
            {
                Dispatcher.BeginInvoke((Action)(delegate
                {
                    MessageBox.Show(MainWindow, messageBoxText, Constants.Constants.Application_Title);
                }));
            }
        }


        public virtual void SaveModel()
        {
            SaveSetting(Navigatedisplay.NavReadings.WindDirection, Constants.Constants.wind);
            SaveSetting(Navigatedisplay.Alpha.AlphaCogNow, Constants.Constants.AlphaCogNow);
            SaveSetting(Navigatedisplay.Alpha.AlphaCogFast, Constants.Constants.AlphaCogFast);
            SaveSetting(Navigatedisplay.Alpha.AlphaCogSlow, Constants.Constants.AlphaCogSlow);


        }

        private void SaveSetting(double value, string key)
        {
            if (value > 0.0)
                Helper.ApplicationSettingHelper.SaveApplicationSetting(value.ToString(), key);
        }

        #region Dispose
        /// <summary>
        /// This fucntion gets called when window is closing.
        /// Inheriting classes should overwrite it in case they would like to do something, for e.g. clearing memory.
        /// </summary>
        public virtual void Dispose()
        {

        }
        #endregion

    }
}

