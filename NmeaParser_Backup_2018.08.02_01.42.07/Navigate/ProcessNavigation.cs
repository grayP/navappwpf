using NmeaParser.Nmea.Gps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NmeaParser.Navigate
{
    public class ProcessNavigation : IProcessNavigation
    {
        public string LocalDeviation { get; set; }
        private double LocalDev = 0.0;
        private int Seconds;
        private Queue<Gpgga> NavMessages = new Queue<Gpgga>(1001);
        // private Navigate navi = new Navigate();

        public void Setup()
        {
            Double.TryParse(LocalDeviation, out LocalDev);
        }
        public int ParseNmeaMessage(Gpgga message)
        {
            int CogNow = 0;
            NavigationDisplay navi = new NavigationDisplay();
            int SecondsNow = message.FixTime.Seconds;

            SecondsNow = SecondsNow == 60 ? 0 : SecondsNow;
            if (SecondsNow > Seconds)
            {
                if (NavMessages.Count > 1000) NavMessages.Dequeue();
                NavMessages.Enqueue(message);
                var ImmediateCOG = FindCOGFromQueue(0, 5);
                CogNow = ImmediateCOG;

                navi.NavReadings.CogSlow = NavMessages.Count();
            }
            Seconds = SecondsNow;
            return CogNow;
        }

        private int FindCOGFromQueue(int start, int N)
        {
            if (NavMessages.Count() < N) return 0;
            var LastN = NavMessages.Reverse().Take(N).ToList();
            var FirstPoint = LastN[start];
            var LastPoint = LastN[N - 1];
            return (int)Trig.GetBearing(FirstPoint.Latitude, FirstPoint.Longitude, LastPoint.Latitude, LastPoint.Longitude, LocalDev);
        }
        public bool IsGoodMessage(Gpgga newReading)
        {
            return newReading.NumberOfSatellites > 3 && Math.Abs(newReading.Latitude) > 0;
        }
    }
}
