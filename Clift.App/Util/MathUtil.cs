using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clift.App.Util
{
    public class MathUtil
    {
        public static double RoundToNthDecimalPlace(double value, int decimalPlaces)
        {
            double multiplier = Math.Pow(10, decimalPlaces);
            return Math.Round(value * multiplier) / multiplier;
        }

    }
}
