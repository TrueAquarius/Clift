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
using AirfoilView.Model;
using AirfoilView.Model.Airfoil;

namespace AirfoilView.UI
{
    /// <summary>
    /// Interaktionslogik für ChartArea.xaml
    /// </summary>
    public partial class ChartArea : UserControl
    {
        public ChartArea()
        {
            InitializeComponent();
            AirFoilList.Owner = this;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            //AssignCurves();
            applyPreferences();
        }

        public void applyPreferences()
        {
            Preferences pref = Global.Instance.Preferences;
            ChartCLCd.Xmin = pref.CdMin;
            ChartCLCd.Xmax = pref.CdMax;
            ChartCLCd.Ymin = pref.ClMin;
            ChartCLCd.Ymax = pref.ClMax;

            ChartCdAlpha.Xmin = pref.AlphaMin;
            ChartCdAlpha.Xmax = pref.AlphaMax;
            ChartCdAlpha.Ymin = pref.CdMin;
            ChartCdAlpha.Ymax = pref.CdMax;

            ChartClAlpha.Xmin = pref.AlphaMin;
            ChartClAlpha.Xmax = pref.AlphaMax;
            ChartClAlpha.Ymin = pref.ClMin;
            ChartClAlpha.Ymax = pref.ClMax;

            ChartCmAlpha.Xmin = pref.AlphaMin;
            ChartCmAlpha.Xmax = pref.AlphaMax;
            ChartCmAlpha.Ymin = pref.CmMin;
            ChartCmAlpha.Ymax = pref.CmMax;

            ChartClCdAlpha.Xmin = pref.AlphaMin;
            ChartClCdAlpha.Xmax = pref.AlphaMax;
            ChartClCdAlpha.Ymin = pref.ClCdMin;
            ChartClCdAlpha.Ymax = pref.ClCdMax;
        }

        public void AddPolar()
        {
            Polars p = Global.Instance.Polars[Global.Instance.Polars.Count - 1];    

            ChartClAlpha.Add(p);
            ChartCdAlpha.Add(p);
            ChartCLCd.Add(p);
            ChartClCdAlpha.Add(p);
            ChartCmAlpha.Add(p);

            AirFoilList.Add(p);

            Global.Instance.Preferences.AlphaMin = Global.Instance.Polars.Min(p => p.AlphaMin); 
            Global.Instance.Preferences.AlphaMax = Global.Instance.Polars.Max(p => p.AlphaMax); 

            Global.Instance.Preferences.ClCdMin = Global.Instance.Polars.Min(p => p.ClCdMin);
            Global.Instance.Preferences.ClCdMax = Global.Instance.Polars.Max(p => p.ClCdMax);

            Global.Instance.Preferences.ClMin = Global.Instance.Polars.Min(p => p.ClMin); 
            Global.Instance.Preferences.ClMax = Global.Instance.Polars.Max(p => p.ClMax); 

            Global.Instance.Preferences.CdMin = Global.Instance.Polars.Min(p => p.CdMin);
            Global.Instance.Preferences.CdMax = Global.Instance.Polars.Max(p => p.CdMax);

            Global.Instance.Preferences.CmMin = Global.Instance.Polars.Min(p => p.CmMin);
            Global.Instance.Preferences.CmMax = Global.Instance.Polars.Max(p => p.CmMax);

            applyPreferences();
            ReRender();

        }

        public void ReRender()
        {
            ChartClAlpha.RepositionFace();
            ChartCdAlpha.RepositionFace();
            ChartCLCd.RepositionFace();
            ChartClCdAlpha.RepositionFace();
            ChartCmAlpha.RepositionFace();
        }
    }
}
