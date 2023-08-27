using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AirfoilView.Model.Airfoil
{
    public class Polar
    {
        public double Alpha { get; set; }
        public double Cl { get; set; }
        public double Cd { get; set; }
        public double Cdp { get; set; }
        public double Cm { get; set; }
        public double TopXtr { get; set; }
        public double BotXtr { get; set; }
        public double ClCd { get { return Cl / Cd; } }
        public double GetPropertyValue(String propertyName)
        {
            Type type = typeof(Polar);
            PropertyInfo propertyInfo = type.GetProperty(propertyName);

            if (propertyInfo != null)
            {
          
                return (double)propertyInfo.GetValue(this);
               
            }
            else
            {
                return 0;
            }
        }
    }
}
