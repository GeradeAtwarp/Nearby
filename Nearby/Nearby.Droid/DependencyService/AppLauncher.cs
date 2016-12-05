using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Nearby.Droid.DependencyService;
using Nearby.Interfaces;

[assembly: Dependency(typeof(AppLauncher))]

namespace Nearby.Droid.DependencyService
{
    public class AppLauncher : IAppLauncher
    {
        public bool OpenNativeMaps(double lat, double longitude, string place)
        {
            try
            {
                var geoUri = Android.Net.Uri.Parse("geo:42.374260,-71.120824");
                var mapIntent = new Intent(Intent.ActionView, geoUri);
                Forms.Context.StartActivity(mapIntent);

                return true;
            }
            catch (Exception ex) { }

            return false;
        }

        public bool SendTweet(string tweet)
        {
            try
            {
                Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("twitter://post?message=" + tweet));
                Forms.Context.StartActivity(intent);
                return true;
            }
            catch (Exception ex) { }

            return false;
        }

        public bool OpenTwitterProfile(string username)
        {
            try
            {
                var intent = Forms.Context.PackageManager.GetLaunchIntentForPackage("com.twitter.android");
                Forms.Context.StartActivity(intent);
                return true;
            }
            catch (Exception ex) { }

            return false;
        }
    }
}