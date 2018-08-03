using navappwpf.Common;
using navappwpf.Constants;
using navappwpf.Helper;
using NmeaParser.Models;
using NmeaParser.Navigate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NmeaParser.Nmea.Gps;
using System.Timers;

namespace navappwpf.ViewModels
{
    public class ProcessGps : ViewModelBase
    {
        private static List<string> _ports;
        internal ProcessNavigation ProcessNavigation;
        internal Course GpsCourse;
        private NmeaParser.NmeaDevice _currentDevice;
        private bool _liveData = false;
        private string _dataTextSavePath;
        private string _currentDeviceInfo = "";
        private bool _dataFound;
        private static Timer _serialPortTimer;
        private int _portNumber;


        public ProcessGps(NavigationDisplay navigateDisplay) : base(new DispatcherWrapper())
        {
            Navigatedisplay = navigateDisplay;
            ProcessNavigation = new ProcessNavigation()
            {
                LocalDeviation = ApplicationSettingHelper.GetFromApplicationSetting(Enums.ApplicationSettingKey.LocalMagDeviation)
            };
            //GpsCourse = new Course()
            //{
            //    LocalDeviation = ApplicationSettingHelper.GetFromApplicationSetting(Enums.ApplicationSettingKey.LocalMagDeviation)
            //};
            ProcessNavigation.Setup();
            Navigatedisplay.LocalDeviation = ApplicationSettingHelper.GetFromApplicationSetting(Enums.ApplicationSettingKey.LocalMagDeviation);
            SetupPorts();
            // SetupCourse();
        }

        private void SetupPorts()
        {
            _ports = System.IO.Ports.SerialPort.GetPortNames().OrderBy(s => s).ToList();
            _portNumber = 0;
            if (_ports.Any())
            {
                SetupDataPath();
                TrialSerialPort(_portNumber);
            }
            else
            {
                if (Navigatedisplay.DeviceStarted) return;
                var device = new NmeaParser.NmeaFileDevice("20180628.txt");
                StartDevice(device);
                Navigatedisplay.DeviceStarted = true;
            }
        }

        private void TrialSerialPort(int portNumber)
        {
            if (portNumber >= _ports.Count())
            {
                _portNumber = portNumber = 0;
            }

            var portName = _ports[portNumber];
            var port = new System.IO.Ports.SerialPort(portName, 4800);
            var device = new NmeaParser.SerialPortDevice(port);
            StartDevice(device);
            _serialPortTimer = new Timer
            {
                Interval = 5000
            };
            _serialPortTimer.Elapsed += SerialPortTimer_Elapsed;
            _serialPortTimer.Start();
        }
            
        private void SerialPortTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_dataFound) return;
            StopDevice();
            _portNumber++;
            TrialSerialPort(_portNumber);
        }


        internal void SetupDataPath()
        {
            _liveData = true;
            _dataTextSavePath = ApplicationSettingHelper.GetFromApplicationSetting(Enums.ApplicationSettingKey.DataPath);
            Directory.CreateDirectory(_dataTextSavePath);
        }
        internal void StopDevice()
        {
            if (_currentDevice != null)
            {
               // _serialPortTimer.Stop();
               // _serialPortTimer.Enabled = false;
               // _serialPortTimer.Dispose();
                _currentDevice.MessageReceived -= Device_MessageReceived;
                _currentDevice.Dispose();
            }
        }

        private void StartDevice(NmeaParser.NmeaDevice device)
        {
            //Clean up old device
            if (_currentDevice != null)
            {
                _currentDevice.MessageReceived -= Device_MessageReceived;
                _currentDevice.Dispose();
            }
            _currentDevice = device;
            _currentDevice.MessageReceived += Device_MessageReceived;
            var _ = _currentDevice.OpenAsync();
            if (device is NmeaParser.NmeaFileDevice)
                _currentDeviceInfo = $"NmeaFileDevice( file={((NmeaParser.NmeaFileDevice)device).FileName} )";
            else if (device is NmeaParser.SerialPortDevice)
            {
                _currentDeviceInfo =
                    $"SerialPortDevice( port={((NmeaParser.SerialPortDevice)device).Port.PortName}, baud={((NmeaParser.SerialPortDevice)device).Port.BaudRate} )";
            }
            LoggingService.AddInfo(_currentDeviceInfo, "nmea");
        }
        private void Device_MessageReceived(object sender, NmeaParser.NmeaMessageReceivedEventArgs args)
        {
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                _dataFound = true;
               if (_liveData) FileUtil.WriteToFile(args.Message, _dataTextSavePath);

                switch (args.Message)
                {
                    case Gpgga _:
                        var newReading = args.Message as Gpgga;
                        if (Navigatedisplay.IsGoodMessage(newReading))
                        {
                            LoggingService.AddInfo(args.Message.ToString(), "nmea");
                            Navigatedisplay.ParseNmeaMessage(newReading);
                           // Navigatedisplay.GetCourseCorrections(GpsCourse);
                        }

                        break;
                    case GPRMC _:
                        var newReadingt = args.Message as Gpvtg;
                        break;
                }
            });
        }
        private void SetupCourse()
        {
            GpsCourse.LoadCourse();
        }
    }
}
