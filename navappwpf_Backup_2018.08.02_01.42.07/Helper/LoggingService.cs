using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace navappwpf.Helper
{
    public class LoggingService
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void AddInfo (string message, string type="Info")
        {
            try
            {
                log.Info(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Logging error");
            }
        }
        public static void AddDebug(string message, string type = "debug")
        {
            try
            {
                log.Debug(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Logging error");
            }
        }

    }
}
