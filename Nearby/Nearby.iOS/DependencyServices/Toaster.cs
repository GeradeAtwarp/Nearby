using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nearby.Interfaces;
using Nearby.iOS.DependencyServices;
using Xamarin.Forms;
using ToastIOS;

[assembly:Dependency(typeof(Toaster))]

namespace Nearby.iOS.DependencyServices
{
    public class Toaster : IToast
    {
        public void SendToast(string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(text, Toast.LENGTH_LONG).SetCornerRadius(0).Show();
            });
        }
    }
}