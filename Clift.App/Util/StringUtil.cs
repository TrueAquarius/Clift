using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilView.Util
{
    public class StringUtil
    {
        public static string ExtractString(string line, string? startIndicator, string? endIndicator)
        {
            int start = 0;
            int end = line.Length - 1;

            if (startIndicator != null && startIndicator.Length > 0)
            {
                start = line.IndexOf(startIndicator) + startIndicator.Length;
            }
            if (endIndicator != null && endIndicator.Length > 0)
            {
                end = line.IndexOf(endIndicator);
            }

            string s = line.Substring(start, end - start);

            return s;
        }
        public static double ExtractDouble(string line, string? startIndicator, string? endIndicator, NumberStyles styles, CultureInfo culture)
        {
            string s = RemoveWhitespaces( ExtractString(line, startIndicator, endIndicator) );

            double d = Double.Parse(s, styles, culture);

            return d;

        }

        public static string RemoveWhitespaces(string input)
        {
            string result = string.Empty;

            foreach (char c in input)
            {
                if (!char.IsWhiteSpace(c))
                {
                    result += c;
                }
            }

            return result;
        }

    }
}
