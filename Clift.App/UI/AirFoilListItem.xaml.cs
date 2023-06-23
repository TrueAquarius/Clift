using AirfoilView.Model;
using AirfoilView.Model.Airfoil;
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
    /// Interaktionslogik für AirFoilListItem.xaml
    /// </summary>
    public partial class AirFoilListItem : UserControl
    {
        public int Index { get; set; }
        public ChartArea? Owner;

        public AirFoilListItem()
        {
            InitializeComponent();
        }
        public AirFoilListItem(Polars p, int i, ChartArea? owner)
        {
            InitializeComponent();
            Index = i;
            Owner = owner;
            VisibilityCheckBox.IsChecked = Global.Instance.Visibility[i] == Visibility.Visible ? true : false;
            Description.Text = p.Name + "(Re=" + p.Reynolds + ", Ncrit=" + p.Ncrit+")";
            ColorSample.Fill = new SolidColorBrush( Model.Global.Instance.getColor(i));
        }

        private void VisibilityCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Global.Instance.Visibility[Index] = Visibility.Visible;
            this.Dispatcher.Invoke(() => {
                if (Owner != null)
                {
                    Owner.ReRender();
                }
            });
        }

        private void VisibilityCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Global.Instance.Visibility[Index] = Visibility.Hidden;
            this.Dispatcher.Invoke(() => {
                if (Owner != null)
                {
                    Owner.ReRender();
                }
            });
        }
    }
}
