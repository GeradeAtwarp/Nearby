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
            MobileAds.Initialize(ApplicationContext, "bc0b686325a74f8fa50134a03ce5efc9");

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

