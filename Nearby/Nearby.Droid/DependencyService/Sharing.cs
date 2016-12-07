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

[assembly:Dependency(typeof(Sharing))]

namespace Nearby.Droid.DependencyService
{
    public class Sharing : ISharer
    {
        public bool PostToFacebook(string PostText)
        {
            try
            {
                var tweetIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("fb://"));
                Forms.Context.StartActivity(tweetIntent);
                return true;
            }
            catch { }

            return false;
        }

        public bool SendTweet(string tweet)
        {
            try
            {
                var tweetIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("twitter://post?message=" + tweet));
                Forms.Context.StartActivity(tweetIntent);
                return true;
            }
            catch { }

            return false;
        }
    }
}