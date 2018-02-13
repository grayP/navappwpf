using System.Device.Location;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NmeaParser.Helper;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestBearingsLeftandRightOfZero()
        {
            double lat1 = -27.5;
            double long1 = 153.0;
            double lat2 = -27.4;
            double long2 = 152.9;

            double lat3 = -27.4;
            double long3 = 153.1;


            double bearing1 = Trig.GetBearing(lat1, long1, lat2, long2, 0);
            double bearing2 = Trig.GetBearing(lat1, long1, lat3, long3, 0);

            bearing1 = Trig.DegreesToRadians(bearing1);
            bearing2 = Trig.DegreesToRadians(bearing2);

            double Average = Trig.RadiansToDegrees( Trig.AverageWeighted(bearing1, bearing2, .5));
        }
    }
}
