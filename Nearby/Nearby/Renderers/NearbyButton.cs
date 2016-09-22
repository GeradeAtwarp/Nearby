using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace Nearby.Renderers
{
    public class NearbyButton : Button
    {
        public static readonly BindableProperty HasOpacity = BindableProperty.Create("Opacity", typeof(bool), typeof(NearbyButton), true);

        public double Opacity
        {
            get
            {
                return (double)GetValue(HasOpacity);
            }
            set
            {
                SetValue(HasOpacity, value);
            }
        }
    }
}
