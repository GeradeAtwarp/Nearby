using MapKit;
using Nearby.Interfaces;
using Nearby.iOS.DependencyServices;
using Social;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppLauncher))]

namespace Nearby.iOS.DependencyServices
{
    public class AppLauncher : IAppLauncher
    {
        public bool OpenNativeMaps(double lat, double longitude, string place)
        {
            var coordinate = new CoreLocation.CLLocationCoordinate2D(lat, longitude);
            var address = new MKPlacemarkAddress();
            var mapPlcae = new MKPlacemark(coordinate, address);
            var mapItem = new MKMapItem(mapPlcae);

            mapItem.Name = place;

            mapItem.OpenInMaps();

            return true;
        }

        public bool SendTweet(string tweet)
        {
            bool success = false;

            //Check if user has twitter
            var slComposer = SLComposeViewController.FromService(SLServiceType.Twitter);
            if (slComposer == null)
            {
                new UIAlertView("Unavailable", "Twitter is not available, please sign in on your devices settings screen.", null, "OK").Show();
            }
            else
            {
                slComposer.SetInitialText(tweet);
                success = true;
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewControllerAsync(slComposer, true);
            }

            return success;
        }

    }
}
