using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nearby.Controls
{
    public class TransparentConatiner : Frame
    {
        public TransparentConatiner()
        {
            Padding = 0;

            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.iOS)
            {
                HasShadow = false;
                BackgroundColor = Color.Transparent;
            }

            OutlineColor = Color.FromHex("#00cc00");
        }
    }
}
