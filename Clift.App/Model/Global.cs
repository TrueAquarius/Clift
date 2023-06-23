using AirfoilView.Model.Airfoil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AirfoilView.Model
{
    /// <summary>
    /// This is a global singleton class which holds all relevant information which needs to be accessed by several components of the application.
    /// 
    /// See https://csharpindepth.com/articles/singleton
    /// </summary>
    public sealed class Global
    {
        private static readonly Lazy<Global> lazy =
        new Lazy<Global>(() => new Global());

        public static Global Instance { get { return lazy.Value; } }


        private Global()
        {
        }

        public Preferences Preferences { get; set; } = new();

        public List<Polars> Polars { get; set; } = new();
        public List<System.Windows.Visibility> Visibility { get; set; } = new();
        public const string PipeName = "AirfoilViewPipe";

        public Color getColor(int i)
        {
            i++; // avoid black
            byte r = (byte)((i % 2) * 155 + 100);
            byte g = (byte)(((i / 2) % 2) * 155 + 100);
            byte b = (byte)(((i / 4) % 2) * 155 + 100);

            return Color.FromRgb(r, g, b);
        }
    }
}
