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
    /// Interaktionslogik für AirFoilList.xaml
    /// </summary>
    public partial class AirFoilList : UserControl
    {
        public ChartArea? Owner;

        public AirFoilList()
        {
            InitializeComponent();
        }

        public void Add(Polars p)
        {
            AirFoilListItem item = new AirFoilListItem(p, Global.Instance.Polars.Count-1, Owner);
            TheList.Children.Add(item);

        }
    }
}
