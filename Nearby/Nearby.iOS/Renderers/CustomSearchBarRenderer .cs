using Nearby.Controls;
using Nearby.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]
namespace Nearby.iOS.Renderers
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var csb = (CustomSearchBar)Element;

                if (csb.BarTint != null)
                    Control.BarTintColor = csb.BarTint.GetValueOrDefault().ToUIColor();

                Control.BarStyle = (UIBarStyle)Enum.Parse(typeof(UIBarStyle), csb.BarStyle);
                Control.SearchBarStyle = (UISearchBarStyle)Enum.Parse(typeof(UISearchBarStyle), csb.BarStyle);
            }
        }
    }
}