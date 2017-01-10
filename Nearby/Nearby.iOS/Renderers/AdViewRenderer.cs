using Google.MobileAds;
using Nearby.Controls;
using Nearby.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AdView), typeof(AdViewRenderer))]

namespace Nearby.iOS.Renderers
{
    public class AdViewRenderer : ViewRenderer<AdView, BannerView>
    {
        string bannerId = "";
        BannerView adview;

        BannerView CreateNativeControl()
        {
            if (adview != null)
                return adview;

            adview = new BannerView(size: AdSizeCons.SmartBannerPortrait, origin: new CoreGraphics.CGPoint(0, UIScreen.MainScreen.Bounds.Size.Height - AdSizeCons.Banner.Size.Height))
            {
                AdUnitID = bannerId,
                RootViewController = GetVisibleViewController()
            };

            // Wire AdReceived event to know when the Ad is ready to be displayed
            adview.AdReceived += (object sender, EventArgs e) =>
            {
                //ad has come in
            };


            adview.LoadRequest(GetRequest());
            return adview;
        }

        Request GetRequest()
        {
            var request = Request.GetDefaultRequest();
            // Requests test ads on devices you specify. Your test device ID is printed to the console when
            // an ad request is made. GADBannerView automatically returns test ads when running on a
            // simulator. After you get your device ID, add it here
            //request.TestDevices = new [] { Request.SimulatorId.ToString () };
            return request;
        }

        /// 
        /// Gets the visible view controller.
        /// 
        /// The visible view controller.
        UIViewController GetVisibleViewController()
        {
            var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (rootController.PresentedViewController == null)
                return rootController;

            if (rootController.PresentedViewController is UINavigationController)
            {
                return ((UINavigationController)rootController.PresentedViewController).VisibleViewController;
            }

            if (rootController.PresentedViewController is UITabBarController)
            {
                return ((UITabBarController)rootController.PresentedViewController).SelectedViewController;
            }

            return rootController.PresentedViewController;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AdView> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                CreateNativeControl();
                SetNativeControl(adview);
            }
        }
    }
}
