using Nearby.iOS.Renderers;
using Nearby.Renderers;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NearbyButton), typeof(NearbyButtonRenderer))]

namespace Nearby.iOS.Renderers
{
    public class NearbyButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            var button = (NearbyButton)Element;

            if(button != null)
            {
                SetOpacity(button);
            }            
        }

        void SetOpacity(NearbyButton view)
        {
            if (view.Opacity != 0.0)
                Control.Alpha = (nfloat)view.Opacity;
        }
    }
}
