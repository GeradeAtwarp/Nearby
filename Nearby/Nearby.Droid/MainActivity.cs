using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Nearby.Utils.Entities;
using Plugin.Permissions;
using Android.Gms.Ads;
using Acr.UserDialogs;
using Microsoft.Azure.Mobile;
using Xamarin.Forms;
using System.Reflection;

namespace Nearby.Droid
{
    [Activity(Label = "Nearby", Icon = "@drawable/nearby_icon", Theme = "@style/splashscreen", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            //Set DB path
            Database.root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);

            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(bundle);

            FFImageLoading.Forms.Droid.CachedImageRenderer.Init();
            MobileAds.Initialize(ApplicationContext, "pub-1631454081193991");
            UserDialogs.Init(this);

            //Configure Mobile Center Analytics
            MobileCenter.Configure("91aa7090-7188-49f2-9506-b0ba75400bd2");

            global::Xamarin.Forms.Forms.Init(this, bundle);

            typeof(Color).GetProperty("Accent", BindingFlags.Public | BindingFlags.Static).SetValue(null, Color.FromHex("#757575"));

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

