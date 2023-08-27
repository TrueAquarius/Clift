using AirfoilView.Model.Airfoil;
using Clift.App.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirfoilView.UI
{
    /// <summary>
    /// Interaktionslogik für ChartPoint.xaml
    /// </summary>
    public partial class ChartPoint : UserControl
    {
        public Polar Polar { get; set; }
        private string _text;
        public ChartPoint(Polar p)
        {
            InitializeComponent();
            Polar = p;

            _text = "Alpha=" + p.Alpha 
                               + "   |   Cl=" + p.Cl 
                               + "   |   Cd=" + p.Cd 
                               + "   |   Cl/Cd=" + MathUtil.RoundToNthDecimalPlace(p.ClCd,4);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                // Now you can interact with the main window
                mainWindow.StatusBar.Text = _text;
            }

        }


        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                // Now you can interact with the main window
                mainWindow.StatusBar.Text = "";
            }
        }
    }
}
