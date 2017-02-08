﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin;
using HockeyApp.iOS;
using Nearby.Utils.Entities;
using ImageCircle.Forms.Plugin.iOS;
using Nearby.iOS.Renderers;
using Google.MobileAds;
using Microsoft.Azure.Mobile;

namespace Nearby.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            FormsMaps.Init();

            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes
            {
                TextColor = UIColor.White
            });

            UISwitch.Appearance.OnTintColor = UIColor.FromRGB(118, 53, 235);

            var manager = BITHockeyManager.SharedHockeyManager;
            manager.Configure("bc0b686325a74f8fa50134a03ce5efc9");
            manager.StartManager();

            manager.Authenticator.AuthenticateInstallation();

            //Set DB path
            Database.root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);

            ImageCircleRenderer.Init();
            NonScrollableListViewRenderer.Initialize();

            MobileAds.Configure("pub-1631454081193991");
            FFImageLoading.Forms.Touch.CachedImageRenderer.Init();

            //Configure Mobile Center Analytics
            MobileCenter.Configure("6174be7f-3e65-4e82-afd6-f28a4e7fa13b");

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
