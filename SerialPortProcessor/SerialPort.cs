using System;
using System.Timers;
using System.IO;


namespace SerialPortProcessor
{
    public class SerialPort
    {
        private static System.Collections.Generic.List<string> _ports;
        //internal ProcessNavigation ProcessNavigation;
        //internal Course GpsCourse;
        //private NmeaParser.NmeaDevice _currentDevice;
        private bool _liveData = false;
        private string _dataTextSavePath;
        private string _currentDeviceInfo = "";
        private bool _dataFound;
        private static Timer _serialPortTimer;
        private int _portNumber;

        public SerialPort()
        {

        }
        public void SetupPorts()
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

    }
}
