using NmeaParser.Nmea.Gps;

namespace NmeaParser.Navigate
{
    public interface IProcessNavigation
    {
        int ParseNmeaMessage(Gpgga message);

    }
}