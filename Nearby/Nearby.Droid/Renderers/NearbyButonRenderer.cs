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
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Nearby.Renderers;
using Nearby.Droid.Renderers;

[assembly: ExportRenderer(typeof(NearbyButton), typeof(NearbyButtonRenderer))]

namespace Nearby.Droid.Renderers
{
    public class NearbyButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            var view = (NearbyButton)Element;

            var button = (NearbyButton)Element;

            if (button != null)
            {
                SetOpacity(button);
            }
        }

        void SetOpacity(NearbyButton view)
        {
            if (view.Opacity != 0.0)
                Control.Alpha = (float)view.Opacity;
        }
    }
}