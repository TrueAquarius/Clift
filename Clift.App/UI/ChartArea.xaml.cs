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
            ChartCLCd.Xmin = pref.Cd_ClCdMin;
            ChartCLCd.Xmax = pref.Cd_ClCdMax;
            ChartCLCd.Ymin = pref.Cl_ClCdMin;
            ChartCLCd.Ymax = pref.Cl_ClCdMax;

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

            ChartClAlpha.Add(p.CurveClAlpha);
            ChartCdAlpha.Add(p.CurveCdAlpha);
            ChartCLCd.Add(p.CurveClCd);
            ChartClCdAlpha.Add(p.CurveClCdAlpha);
            ChartCmAlpha.Add(p.CurveCmAlpha);

            AirFoilList.Add(p);
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
