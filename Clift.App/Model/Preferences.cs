using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirfoilView.Model
{
    public class Preferences
    {
        public double AlphaMin { get; set; }
        public double AlphaMax { get; set; }

        public double Cl_ClCdMin { get; set; }
        public double Cl_ClCdMax { get; set; }

        public double Cd_ClCdMin { get; set; }
        public double Cd_ClCdMax { get; set; }

        public double ClCdMin { get; set; }
        public double ClCdMax { get; set; }

        public double ClMin { get; set; }
        public double ClMax { get; set; }

        public double CdMin { get; set; }
        public double CdMax { get; set; }

        public double CmMin { get; set; }
        public double CmMax { get; set; }


        public void Reset()
        {
            AlphaMin = -10.0;
            AlphaMax = 15.0;

            Cl_ClCdMin = -0.6;
            Cl_ClCdMax = 1.4;

            Cd_ClCdMin = 0;
            Cd_ClCdMax = 0.2;

            ClCdMin = -10;
            ClCdMax = 30;

            ClMin = -0.6;
            ClMax = 1.4;

            CdMin = 0;
            CdMax = 0.2;

            CmMin = -0.9;
            CmMax = 0.2;
        }
    }
}
