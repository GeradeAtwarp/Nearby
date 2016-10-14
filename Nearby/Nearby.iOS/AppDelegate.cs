using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin;
using HockeyApp.iOS;
using Nearby.Utils.Entities;

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

            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;

            var manager = BITHockeyManager.SharedHockeyManager;
            manager.Configure("bc0b686325a74f8fa50134a03ce5efc9");
            manager.CrashManager.CrashManagerStatus = BITCrashManagerStatus.AutoSend;
            manager.StartManager();

            manager.Authenticator.AuthenticateInstallation();

            //Set DB path
            Database.root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
