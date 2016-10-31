using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nearby.Controls
{
    public class NearbyButton : Button
    {
        public NearbyButton() : base()
        {
            const int _animationTime = 100;
            Clicked += async (sender, e) =>
            {
                var btn = (NearbyButton)sender;
                await btn.ScaleTo(1.2, _animationTime);
                btn.ScaleTo(1, _animationTime);
            };
        }
    }
}
