using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using navappwpf.Constants;
using navappwpf.Helper;
using navappwpf.ViewModels;
using SerialPortProcess;

namespace navappwpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SerialPort _serialPort;

        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            App.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            SetupPorts();
            MainWindowViewModel context = new MainWindowViewModel(_serialPort.NavigationDisplay);
            Window mainWindow = new MainWindow() { DataContext = context };
            //context.SetCurrentViewModel(new ReadingsViewModel());
            mainWindow.Show();

        }
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            MessageBox.Show(ex.Message, "Thread Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SetupPorts()
        {
            _serialPort = new SerialPort();
            _serialPort.SetupPorts(ApplicationSettingHelper.GetFromApplicationSetting(Enums.ApplicationSettingKey.DataPath));
        }
    }
}