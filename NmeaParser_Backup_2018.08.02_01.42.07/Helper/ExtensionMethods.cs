using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NmeaParser.Helper
{
    public static class ExtensionMethods
    {
        public static int RoundToNearest(int value, double round)
        {
            return ((int)Math.Round(value / round) * (int)round);

        }
    }
}
