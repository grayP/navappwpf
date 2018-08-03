using NmeaParser.Navigate;
using NmeaParser.Nmea.Gps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Threading;

namespace SerialPortProcess
{
    public class SerialPort
    {
        public NavigationDisplay NavigationDisplay { get; set; } = new NavigationDisplay();
        private static List<string> _ports;
        //internal ProcessNavigation ProcessNavigation;
        //internal Course GpsCourse;
        private NmeaParser.NmeaDevice _currentDevice;
        private bool _liveData = false;
        private string _dataTextSavePath;
        private string _currentDeviceInfo = "";
        private bool _dataFound;
        private static Timer _serialPortTimer;
        private int _portNumber;

        public bool AnyPortsFound()
        {
            return System.IO.Ports.SerialPort.GetPortNames().Any();
        }

        public void SetupPorts(string DataSavePath)
        {
            _ports = System.IO.Ports.SerialPort.GetPortNames().OrderBy(s => s).ToList();
            _portNumber = 0;
            if (_ports.Any())
            {
                SetupDataPath(DataSavePath);
                TrialSerialPort(_portNumber);
            }
            else
            {
                var device = new NmeaParser.NmeaFileDevice("20180628.txt");
                StartDevice(device);
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

        internal void SetupDataPath(string dataSavePath)
        {
            _liveData = true;
            _dataTextSavePath = dataSavePath;
            Directory.CreateDirectory(_dataTextSavePath);
        }
        internal void StopDevice()
        {
            if (_currentDevice != null)
            {
                _serialPortTimer.Stop();
                _serialPortTimer.Enabled = false;
                _serialPortTimer.Dispose();
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
            //  LoggingService.AddInfo(_currentDeviceInfo, "nmea");
        }

        private void Device_MessageReceived(object sender, NmeaParser.NmeaMessageReceivedEventArgs args)
        {
           // Dispatcher.BeginInvoke((Action)delegate ()
           // {
            _dataFound = true;
            switch (args.Message)
            {
                case Gpgga _:
                    var newReading = args.Message as Gpgga;
                    if (NavigationDisplay.IsGoodMessage(newReading))
                    {
                        if (_liveData) FileUtil.WriteToFile(args.Message, _dataTextSavePath);
                        //LoggingService.AddInfo(args.Message.ToString(), "nmea");
                        NavigationDisplay.ParseNmeaMessage(newReading);
                        // Navigatedisplay.GetCourseCorrections(GpsCourse);
                    }
                    break;
                case GPRMC _:
                    var newReadingt = args.Message as Gpvtg;
                    break;
            }
            //});
        }

    }
}
