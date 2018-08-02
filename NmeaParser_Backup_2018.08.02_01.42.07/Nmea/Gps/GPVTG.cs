using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NmeaParser.Nmea.Gps
{
    /// <summary>
    ///  Global Positioning System Fix Data
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gpgga")]
    [NmeaMessageType("GPVTG")]
    public class Gpvtg : Gga
    {
        /// <summary>
        /// Fix quality
        /// </summary>
        public enum FixQuality : int
        {
            /// <summary>Invalid</summary>
            Invalid = 0,
            /// <summary>GPS</summary>
            GpsFix = 1,
            /// <summary>Differential GPS</summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dgps")]
            DgpsFix = 2,
            /// <summary>Precise Positioning Service</summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pps")]
            PpsFix = 3,
            /// <summary>Real Time Kinematic (Fixed)</summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rtk")]
            Rtk = 4,
            /// <summary>Real Time Kinematic (Floating)</summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rtk")]
            FloatRtk = 5,
            /// <summary>Estimated</summary>
            Estimated = 6,
            /// <summary>Manual input</summary>
            ManualInput = 7,
            /// <summary>Simulation</summary>
            Simulation = 8
        }
    }
}
