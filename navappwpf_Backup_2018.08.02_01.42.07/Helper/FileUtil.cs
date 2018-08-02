using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NmeaParser.Nmea;

namespace navappwpf.Helper
{
    public static class FileUtil
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
