using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace navappwpf.Helper
{
   public static class Trig
    {
        public static double GetBearing(double lat1, double lon1, double lat2, double lon2, double LocalDeviation)
        {
            double Lat1 = DegreesToRadians(lat1);
            double Lat2 = DegreesToRadians(lat2);
            double dLon = DegreesToRadians(lon2 - lon1);

            double y = Math.Sin(dLon) * Math.Cos(Lat2);
            double x = Math.Cos(Lat1) * Math.Sin(Lat2) - Math.Sin(Lat1) * Math.Cos(Lat2) * Math.Cos(dLon);
            double brng = Math.Atan2(y, x);

            return (RadiansToDegrees(brng) + 360 + LocalDeviation) % 360;  //Add 11 for magnetic deviation in qld
        }
        private static double DegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private static double RadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

    }
}
