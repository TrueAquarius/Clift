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
        public Model.Draw.Point Point { get; set; }

        public ChartPoint(Model.Draw.Point p)
        {
            InitializeComponent();
            Point = p;

            MouseOverText.Text = "[ " + p.X + " | " + p.Y + " ]";
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            MouseOverText.Visibility = Visibility.Visible;
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            MouseOverText.Visibility = Visibility.Hidden;
        }


    }
}
