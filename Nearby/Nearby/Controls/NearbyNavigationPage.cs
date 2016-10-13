using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nearby.Controls
{
    public class NearbyNavigationPage : NavigationPage
    {
        public NearbyNavigationPage(Page root) : base(root)
        {
            Init();
            Title = root.Title;
            Icon = root.Icon;
        }

        public NearbyNavigationPage()
        {
            Init();
        }

        void Init()
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                BarBackgroundColor = Color.FromHex("FAFAFA");
            }
            else
            {
               
            }
        }
    }
}
