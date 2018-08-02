using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NmeaParser.Nmea
{
    /// <summary>
    /// Represents an unknown message type
    /// </summary>
    public class UnknownMessage : NmeaMessage
    {
        /// <summary>
        /// Gets the nmea value array.
        /// </summary>
        public IReadOnlyList<string> Values { get { return base.MessageParts; } }
        /// <summary>
        /// Called when the message is being loaded.
        /// </summary>
        /// <param name="message">The NMEA message values.</param>
        protected override void OnLoadMessage(string[] message)
        {
        }
    }
}
