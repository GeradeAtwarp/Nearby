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
using Nearby.Interfaces;
using Nearby.Droid.DependencyService;
using Xamarin.Forms;
using Plugin.CurrentActivity;

[assembly:Dependency(typeof(Toasting))]

namespace Nearby.Droid.DependencyService
{
    public class Toasting : IToast
    {
        public void SendToast(string text)
        {
            var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
            Device.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(context, text, ToastLength.Long).Show();
            });
        }
    }
}