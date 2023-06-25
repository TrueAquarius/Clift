using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirfoilView.Model.Draw;
using AirfoilView.Util;

namespace AirfoilView.Model.Airfoil
{
    public class Polars
    {
        private static string INDICATOR_REYNOLDS = "Re =";
        private static string INDICATOR_NCRIT = "Ncrit =";

        public List<Polar> polars { get; set; } = new List<Polar>();
        public double Reynolds { get; set; }
        public string? Filename { get; set; }
        public string? RawData { get; set; }
        public string? Name { get; set; }
        public double Ncrit { get; set; }
        

        public Polars()
        {

        }

        public static Polars? LoadFromXfoilFile(string filename)
        {
            if (filename == null) return null;

            if (!File.Exists(filename)) return null;

            Polars p = new Polars();

            NumberStyles styles = NumberStyles.AllowExponent | NumberStyles.Number;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            p.Filename = filename;

            StreamReader sr = System.IO.File.OpenText(filename);
            string? line;
            int lineCounter = 0;
            while ((line = sr.ReadLine()) != null)
            {
                ++lineCounter;
                p.RawData += line;

                if(lineCounter == 2)
                {
                    if (!line.Trim().StartsWith("XFOIL")) return null;
                }

                if (lineCounter == 4)
                {
                    if (!line.Trim().StartsWith("Calculated polar for:")) return null;
                    p.Name = line.Trim().Substring("Calculated polar for:".Length).Trim();
                }

                if (lineCounter == 9)
                {
                    p.Reynolds = StringUtil.ExtractDouble(line, INDICATOR_REYNOLDS, INDICATOR_NCRIT, styles, culture);
                    p.Ncrit = StringUtil.ExtractDouble(line, INDICATOR_NCRIT, null, styles, culture);
                }

                if (lineCounter == 11)
                {
                    if (!line.Trim().StartsWith("alpha    CL        CD       CDp       CM     Top_Xtr  Bot_Xtr")) return null;
                }

                if (lineCounter == 12)
                {
                    if (!line.Trim().StartsWith("------ -------- --------- --------- -------- -------- --------")) return null;
                }

                if(lineCounter >=13)
                {
                    string [] lineSplit = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (lineSplit.Length != 7) return null;
                    try
                    {
                        Polar po = new Polar();
                        po.alpha = Double.Parse(lineSplit[0], styles, culture);
                        po.Cl = Double.Parse(lineSplit[1], styles, culture);
                        po.Cd = Double.Parse(lineSplit[2], styles, culture);
                        po.Cdp = Double.Parse(lineSplit[3], styles, culture);
                        po.Cm = Double.Parse(lineSplit[4], styles, culture);
                        po.TopXtr = Double.Parse(lineSplit[5], styles, culture);
                        po.BotXtr = Double.Parse(lineSplit[6], styles, culture);
                        p.polars.Add(po);
                    }
                    catch(Exception)
                    {
                        return null;
                    }
                }

            }

            return p;
        }



        public Curve CurveClCd
        {
            get
            {
                Curve curve = new Curve();

                foreach (Polar p in polars.OrderBy(x => x.alpha))
                {
                    Point point = new Point();
                    point.X = p.Cd;
                    point.Y = p.Cl;
                    curve.points.Add(point);
                }

                return curve;
            }
        }


        public Curve CurveClCdAlpha
        {
            get
            {
                Curve curve = new Curve();

                foreach (Polar p in polars.OrderBy(x => x.alpha))
                {
                    if (p.Cd != 0)
                    {
                        Point point = new Point();
                        point.X = p.alpha;
                        point.Y = p.Cl / p.Cd;
                        curve.points.Add(point);
                    }
                }

                return curve;
            }
        }


        public Curve CurveClAlpha
        {
            get
            {
                Curve curve = new Curve();

                foreach (Polar p in polars.OrderBy(x => x.alpha))
                {
                    Point point = new Point();
                    point.X = p.alpha;
                    point.Y = p.Cl;
                    curve.points.Add(point);
                }

                return curve;
            }
        }


        public Curve CurveCdAlpha
        {
            get
            {
                Curve curve = new Curve();

                foreach (Polar p in polars.OrderBy(x => x.alpha))
                {
                    Point point = new Point();
                    point.X = p.alpha;
                    point.Y = p.Cd;
                    curve.points.Add(point);
                }

                return curve;
            }
        }

        public Curve CurveCmAlpha
        {
            get
            {
                Curve curve = new Curve();

                foreach (Polar p in polars.OrderBy(x => x.alpha))
                {
                    Point point = new Point();
                    point.X = p.alpha;
                    point.Y = p.Cm;
                    curve.points.Add(point);
                }

                return curve;
            }
        }
    }
}
