using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using navappwpf.Common;
using navappwpf.Constants;
using navappwpf.Views.UserControls;
using NmeaParser.Helper;
using NmeaParser.Nmea.Gps;
using NmeaParser.Wind;

namespace navappwpf.ViewModels
{
    public class Page1ViewModel : ViewModelBase
    {

        private Queue<string> messages = new Queue<string>(101);
        private NmeaParser.NmeaDevice currentDevice;
        private ProcessNavigation processNavigation;
        public Gpgga gpgga = new Gpgga();
        // private GpggaControl gpggaControl = new GpggaControl();
        private string currentDeviceInfo = "";



        public Page1ViewModel(NavigationDisplay navigateDisplay) : base(new DispatcherWrapper())
        {
            Navigatedisplay = navigateDisplay;

            var availableSerialPorts = System.IO.Ports.SerialPort.GetPortNames().OrderBy(s => s);
            // Use serial portName:
            processNavigation = new ProcessNavigation()
            {
                LocalDeviation = Helper.Helper.GetFromApplicationSetting(Enums.ApplicationSettingKey.LocalMagDeviation)
            };
            processNavigation.Setup();

            if (availableSerialPorts.Count() > 0)
            {
                var comPort = availableSerialPorts.First();
                var portName = new System.IO.Ports.SerialPort(comPort, 4800);

                //Use a log file for playing back logged data

                var device = new NmeaParser.SerialPortDevice(portName);
                StartDevice(device);
            }
            else
            {
                var device = new NmeaParser.NmeaFileDevice("NmeaSampleData.txt");
                StartDevice(device);

            }
        }

        private void StartDevice(NmeaParser.NmeaDevice device)
        {
            //Clean up old device
            if (currentDevice != null)
            {
                currentDevice.MessageReceived -= device_MessageReceived;
                currentDevice.Dispose();
            }
            messages.Clear();
            // gprmcView.Message = null;
            //  gpggaControl.Message = null;
            //navigateView.Message = null;
            //  windDirectionView.Wind = null;
            //  gpgsaView.Message = null;
            //  gpgllView.Message = null;
            //  pgrmeView.Message = null;
            //  satView.GpgsvMessages = null;
            //Start new device
            currentDevice = device;
            currentDevice.MessageReceived += device_MessageReceived;
            var _ = currentDevice.OpenAsync();
            if (device is NmeaParser.NmeaFileDevice)
                currentDeviceInfo = string.Format("NmeaFileDevice( file={0} )", ((NmeaParser.NmeaFileDevice)device).FileName);
            else if (device is NmeaParser.SerialPortDevice)
            {
                currentDeviceInfo = string.Format("SerialPortDevice( port={0}, baud={1} )",
                ((NmeaParser.SerialPortDevice)device).Port.PortName,
                ((NmeaParser.SerialPortDevice)device).Port.BaudRate);
            }
        }

        private void device_MessageReceived(object sender, NmeaParser.NmeaMessageReceivedEventArgs args)
        {
            Dispatcher.BeginInvoke((Action)delegate ()
            {
                messages.Enqueue(args.Message.MessageType + ": " + args.Message.ToString());
                if (messages.Count > 100) messages.Dequeue(); //Keep message queue at 100
                                                              // output.Text = string.Join("\n", messages.ToArray());
                                                              // output.Select(output.Text.Length - 1, 0); //scroll to bottom


                if (args.Message is NmeaParser.Nmea.Gps.Gpgga)
                {

                    var NewReading = args.Message as NmeaParser.Nmea.Gps.Gpgga;
                    if (Navigatedisplay.IsGoodMessage(NewReading))
                    {
                        Navigatedisplay.ParseNmeaMessage(NewReading);
                    }
                }
                else if (args.Message is NmeaParser.Nmea.Gps.GPRMC)
                {
                    var newReading = args.Message as NmeaParser.Nmea.Gps.Gpvtg;


                }
            });
        }

        

        public override void ExecutePreviousCommand()
        {
            base.ExecutePreviousCommand();
        }

        public override void ExecuteNextCommand()
        {
            base.ExecuteNextCommand();
        }
    }
}
