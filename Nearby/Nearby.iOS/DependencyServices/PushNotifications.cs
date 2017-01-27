using System;
using UIKit;
using System.Threading.Tasks;
using XamarinEvolve.iOS;
using Xamarin.Forms;
using Foundation;
using Nearby.Interfaces;

[assembly:Dependency(typeof(PushNotifications))]
namespace XamarinEvolve.iOS
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

