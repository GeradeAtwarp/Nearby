using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Nearby.Interfaces;
using Xamarin.Forms;
using Nearby.iOS.DependencyServices;
using Foundation;

[assembly: Dependency(typeof(PhoneDialer))]

namespace Nearby.iOS.DependencyServices
{
    public class PhoneDialer : IPhoneDialer
    {
        public bool LaunchCall(string telnumber)
        {
            return UIApplication.SharedApplication.OpenUrl(new NSUrl("telprompt://" + telnumber));
        }
    }
}