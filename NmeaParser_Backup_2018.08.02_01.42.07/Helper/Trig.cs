using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NmeaParser.Models;

namespace NmeaParser.Navigate
{
   public static class Trig
    {
        public static double GetBearing(GeoCoordinate x, GeoCoordinate y, double Localdev)
        {

            return GetBearing(x.Latitude, x.Longitude, y.Latitude, y.Longitude, Localdev);
        }

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
        public static double DegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        public static double AverageWeighted(double bearing, double lastBearing, double v)
        {
            return (double)(v * bearing + ((1 - v) * lastBearing));
        }
        public static int GetNewAverageBearing(double newBearing, ref double lastBearing, double alpha)
        {
            var diff = ((lastBearing - newBearing + 180.0 + 360) % 360) - 180.0;
            double newValue = (360 + newBearing + ((diff) * (1-alpha))) % 360;
            lastBearing = newValue;
            return (int)newValue;
        }

        internal static double CalcXTrack(double xLat, double xLong, Course.WayPoint startWayPoint, Course.WayPoint endWayPoint)
        {
            double bearing1 = GetBearing(startWayPoint.Latitude, startWayPoint.Longitude, endWayPoint.Latitude, endWayPoint.Longitude, 11);
            double bearing2 = GetBearing(startWayPoint.Latitude, startWayPoint.Longitude, xLat, xLong, 11);
            GeoCoordinate start = new GeoCoordinate(startWayPoint.Latitude, startWayPoint.Longitude);
            GeoCoordinate point = new GeoCoordinate(xLat, xLong);
            double distance = start.GetDistanceTo(point);
            double angle = (bearing1 - bearing2 + 540.0) % 360 - 180.0;
            return Math.Round(distance * Math.Sin(DegreesToRadians(angle)), 0);
        }
    }
}
