using NmeaParser.Navigate;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NmeaParser.Models
{
    public class Course
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public Single Speed { get; set; }
        public List<WayPoint> WayPoints { get; set; } = new List<WayPoint>();
        public string LocalDeviation { get; set; }
        public WayPoint StartWayPoint { get; set; }
        public WayPoint EndWayPoint { get; set; }
        public void LoadCourse()
        {
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead("DataFile\\Myora2018.csv"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    //0,12:28:00 PM,-27.47833333,153.3966667,12,28,0
                    var args = line.Split(',');
                    WayPoints.Add(new WayPoint()
                    {
                        Id = int.Parse(args[0]),
                        Latitude = Double.Parse(args[2]),
                        Longitude = Double.Parse(args[3]),
                        LegStartTime = new DateTime(2000, 1, 1, int.Parse(args[4]), int.Parse(args[5]), int.Parse(args[6]))
                    });
                }
            }
        }

        public GeoCoordinate WhereShouldIBe(GeoCoordinate position)
        {
            //DateTime nowTime = new DateTime(2000, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            DateTime nowTime = new DateTime(2000, 1, 1, 14, 21, 0);
            DateTime EventStart = WayPoints.First().LegStartTime;
            DateTime EventEnd = WayPoints.Last().LegStartTime;

            if (DateTime.Compare(nowTime, EventStart) < 0)
            {
                StartWayPoint = WayPoints.First();
                EndWayPoint = WayPoints.FirstOrDefault(w => w.Id == (StartWayPoint.Id + 1));
                double xLat = StartWayPoint.Latitude;
                double xLong = StartWayPoint.Longitude;
                double timeToTurn = (StartWayPoint.LegStartTime - nowTime).TotalSeconds;
                double NextLeg = Trig.GetBearing(StartWayPoint.Latitude, StartWayPoint.Longitude, EndWayPoint.Latitude, EndWayPoint.Longitude, 11.0);
                return new GeoCoordinate(xLat, xLong, timeToTurn, NextLeg, 0.0, 0.0, 0.0);
            }
            else if (DateTime.Compare(nowTime, EventEnd) > 0)
            {
                return null;
            }
            else

            {
                StartWayPoint = WayPoints.LastOrDefault(w => nowTime > w.LegStartTime);
                EndWayPoint = WayPoints.First(w => nowTime < w.LegStartTime);
                WayPoint NextWayPoint = WayPoints.FirstOrDefault(w => w.Id == (EndWayPoint.Id + 1));
                double PercentDownLeg = ((nowTime - StartWayPoint.LegStartTime).TotalSeconds * 1000) / ((EndWayPoint.LegStartTime - StartWayPoint.LegStartTime).TotalSeconds) / 1000.0;
                double xLat = StartWayPoint.Latitude + (EndWayPoint.Latitude - StartWayPoint.Latitude) * PercentDownLeg;
                double xLong = StartWayPoint.Longitude + (EndWayPoint.Longitude - StartWayPoint.Longitude) * PercentDownLeg;
                double xTrack = Trig.CalcXTrack(position.Latitude, position.Longitude, StartWayPoint, EndWayPoint);
                double timeToTurn = Math.Round((EndWayPoint.LegStartTime - nowTime).TotalSeconds, 0);
                double NextLeg = Trig.GetBearing(EndWayPoint.Latitude, EndWayPoint.Longitude, NextWayPoint.Latitude, NextWayPoint.Longitude, 11.0);

                return new GeoCoordinate(xLat, xLong, timeToTurn, NextLeg, 0.0, xTrack, 0.0);
            }
        }

        public class WayPoint
        {
            public int Id { get; set; }
            public DateTime LegStartTime { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        internal double BearintToWayPoint(GeoCoordinate lastPosition)
        {
            throw new NotImplementedException();
        }
    }
}
