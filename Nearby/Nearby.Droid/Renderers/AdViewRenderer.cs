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
using Nearby.Droid.Renderers;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Gms.Ads;

[assembly: ExportRenderer(typeof(Nearby.Controls.AdView), typeof(AdViewRenderer))]

namespace Nearby.Droid.Renderers
{
    public class AdViewRenderer : ViewRenderer<Nearby.Controls.AdView, AdView>
    {
        string adUnitId = string.Empty;
        AdSize adSize = AdSize.SmartBanner;
        AdView adView;
        AdView CreateNativeControl()
        {
            if (adView != null)
                return adView;

            adUnitId = ("ca-app-pub-1631454081193991/4991141465");
            adView = new AdView(Forms.Context);
            adView.AdSize = adSize;
            adView.AdUnitId = adUnitId;

            var adParams = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);

            adView.LayoutParameters = adParams;

            adView.LoadAd(new AdRequest
                            .Builder()
                            .Build());
            return adView;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Nearby.Controls.AdView> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                CreateNativeControl();
                SetNativeControl(adView);
            }
        }
    }
}