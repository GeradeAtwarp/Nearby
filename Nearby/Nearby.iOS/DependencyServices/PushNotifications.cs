using System;
using UIKit;
using System.Threading.Tasks;
using Xamarin.Forms;
using Foundation;
using Nearby.Interfaces;
using Nearby.iOS.DependencyServices;

[assembly:Dependency(typeof(PushNotifications))]
namespace Nearby.iOS.DependencyServices
{
    public class PushNotifications : IPushNotifications
    {
        #region IPushNotifications implementation
        
        public void OpenSettings()
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
        }

        #endregion
    }
}

