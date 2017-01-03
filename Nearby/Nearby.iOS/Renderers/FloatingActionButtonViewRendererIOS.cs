using AnimatedButtons;
using Nearby.Controls;
using Nearby.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(FloatingActionButtonViewIOS), typeof(FloatingActionButtonViewRendererIOS))]

namespace Nearby.iOS.Renderers
{
    public class FloatingActionButtonViewRendererIOS : ViewRenderer
    {
        private readonly LiquidFloatingActionButton fab;

        public FloatingActionButtonViewRendererIOS()
        {
            fab = new LiquidFloatingActionButton();

            fab.BackgroundColor = UIKit.UIColor.Red;

            SetNativeControl(fab);
        }
    }
}