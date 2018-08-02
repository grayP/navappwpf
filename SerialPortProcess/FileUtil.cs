using NmeaParser.Nmea;
using System;
using System.IO;

namespace SerialPortProcess
{
    internal static class FileUtil
    {
        internal static void WriteToFile(NmeaMessage message, string path)
        {
            using (StreamWriter sw = File.AppendText(Path.Combine(path, $"{DateTime.Now.ToString("yyyyMMdd")}.txt")))
            {
                sw.WriteLine(message);
            }
        }
    }
}
