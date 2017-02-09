using Foundation;
using Nearby.Interfaces;
using Nearby.iOS.DependencyServices;
using Social;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly:Dependency(typeof(Sharing))]

namespace Nearby.iOS.DependencyServices
{
    public class Sharing : ISharer
    {
        public bool PostToFacebook(string PostText)
        {
            var slcomp = SLComposeViewController.FromService(SLServiceKind.Facebook);
            if (slcomp == null)
                return false;
            else
            {
                slcomp.SetInitialText(PostText);
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewControllerAsync(slcomp, true);
                return true;
            }
        }

        public bool SendTweet(string tweet)
        {
            try
            {
                var slcomp = SLComposeViewController.FromService(SLServiceKind.Twitter);
                if (slcomp == null)
                    return false;
                else
                {
                    slcomp.SetInitialText(tweet);
                    UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewControllerAsync(slcomp, true);
                    return true;
                }
            }
            catch { }

            return false;
        }

        public bool OpenUserName(string username)
        {
            try
            {
                if (UIApplication.SharedApplication.OpenUrl(NSUrl.FromString($"twitter://user?screen_name={username}")))
                    return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to launch url" + ex);
            }

            return false;
        }
    }
}