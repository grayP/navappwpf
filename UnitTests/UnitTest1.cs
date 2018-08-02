using Microsoft.VisualStudio.TestTools.UnitTesting;
using NmeaParser.Helper;
using NmeaParser.Navigate;
using System;

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

            double Average = Trig.GetNewAverageBearing(bearing2, ref bearing1, .5);
            Assert.AreEqual(0, Average);

            bearing1 = 345;
            Average = Trig.GetNewAverageBearing(45, ref bearing1, .9);
            Assert.AreEqual(39, Average);


            bearing1 = 345;
            Average = Trig.GetNewAverageBearing(45, ref bearing1, .01);
            Assert.AreEqual(345, Average);

            bearing1 = 45;
            Average = Trig.GetNewAverageBearing(345, ref bearing1, .1);
            Assert.AreEqual(39, Average);

        }

        [TestMethod]
        public void TestTheTackIsBeingSetCorrectly()
        {
            NavigationReadings navigationReadings = new NavigationReadings();
            navigationReadings.WindDirection = 0;

            navigationReadings.SetTack(45);

            Assert.AreEqual(navigationReadings.Tack, NmeaParser.Constants.Tack.Port);

            navigationReadings.SetTack(170);
            Assert.AreEqual(navigationReadings.Tack, NmeaParser.Constants.Tack.PortRun);

            navigationReadings.SetTack(345);
            Assert.AreEqual(navigationReadings.Tack, NmeaParser.Constants.Tack.StarBoard);

            navigationReadings.SetTack(200);
            Assert.AreEqual(navigationReadings.Tack, NmeaParser.Constants.Tack.StarBoardRun);

            navigationReadings.WindDirection = 230;
            navigationReadings.SetTack(280);
            Assert.AreEqual(navigationReadings.Tack, NmeaParser.Constants.Tack.Port);
            navigationReadings.SetTack(170);
            Assert.AreEqual(navigationReadings.Tack, NmeaParser.Constants.Tack.StarBoard);
        }

        [TestMethod]
        public void TestForNewMinYAxisValue()
        {
            var newMin = 22;
            var YAxisMin = ExtensionMethods.RoundToNearest(newMin, 20.0);
            Assert.IsTrue(YAxisMin == 20);

        }

        [TestMethod]
        public void TestGMTTimeBeingCorrectlyCalculated()
        {
            var TimeSpan = new TimeSpan(hours: 9, minutes: 0, seconds: 0);
            var result = NavigationDisplay.CreateTime(TimeSpan);
            Assert.IsTrue(result.Day == DateTime.Now.Day);

            TimeSpan = new TimeSpan(hours: 2, minutes: 0, seconds: 0);
            result = NavigationDisplay.CreateTime(TimeSpan);
            Assert.IsTrue(result.Day == DateTime.Now.Day);

            TimeSpan = new TimeSpan(hours: 17, minutes: 0, seconds: 0);
            result = NavigationDisplay.CreateTime(TimeSpan);
            Assert.IsTrue(result.Day == DateTime.Now.Day);

            TimeSpan = new TimeSpan(hours: 23, minutes: 0, seconds: 0);
            result = NavigationDisplay.CreateTime(TimeSpan);
            Assert.IsTrue(result.Day == DateTime.Now.Day);

        }

        [TestMethod]
        public void LocalDeviationBeingAddedCorrectly()
        {

            double lat1 = -27.5;
            double long1 = 153.0;
            double lat2 = -27.5;
            double long2 = 154.0;

            var bearing1 = Math.Round(Trig.GetBearing(lat1, long1, lat2, long2, -8.37),1);

            Assert.AreEqual(bearing1, 81.9);

            lat1 = -23.0;
            long1 = 153.00;
            lat2 = -22.5;
            long2 = 153.00;

            bearing1 = Math.Round(Trig.GetBearing(lat1, long1, lat2, long2, -8.37), 1);
            Assert.AreEqual(bearing1, 351.6);
        }
    }


}
