using Microsoft.Azure.Mobile.Analytics;
using MvvmHelpers;
using Nearby.Helpers;
using Nearby.Interfaces;
using Nearby.Utils;
using Nearby.Utils.Entities;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using Plugin.Geolocator;
using Plugin.Share;
using Plugin.Share.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Nearby.viewModel
{
    public class NearbyBaseViewModel : BaseViewModel
    {
        protected static INavigation Navigation { get; set; }
        protected Database NearbyDataContext { get; }
        protected TextInfo CultureTextInfo { get; }
        protected static IToast Toaster { get; set; }
        protected static IAppLauncher AppLauncher { get; set; }


    public NearbyBaseViewModel()
        {
            NearbyDataContext = new Database();

            // Creates a TextInfo based on the "en-US" culture.
            CultureTextInfo = new CultureInfo("en-US").TextInfo;
        }

        public static void Init(INavigation navigation = null)
        {
            Navigation = navigation;
            Toaster = DependencyService.Get<IToast>();
            AppLauncher = DependencyService.Get<IAppLauncher>();
        }

        public Settings Settings
        {
            get { return Settings.Current; }
        }
        

        /// <summary>
        /// Launch the browser
        /// </summary>
        public ICommand launchBrowserCommand;
        public ICommand LaunchBrowserCommand => launchBrowserCommand ?? (launchBrowserCommand = new Command<string>(async (t) => await ExecuteLaunchBrowserAsync(t)));
        async Task ExecuteLaunchBrowserAsync(string arg)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await CrossShare.Current.OpenBrowser(arg, new BrowserOptions
                {
                    ChromeShowTitle = true,
                    ChromeToolbarColor = new ShareColor
                    {
                        A = 255,
                        R = 118,
                        G = 53,
                        B = 235
                    },
                    UseSafairReaderMode = false,
                    UseSafariWebViewController = true
                });
            }
            catch (Exception ex)
            { }
            finally { IsBusy = false; }
        }


        /// <summary>
        /// Open the phone app to make a call
        /// </summary>
        public ICommand launchDialerCommand;
        public ICommand LaunchDialerCommand => launchDialerCommand ?? (launchDialerCommand = new Command<string>(async (t) => await ExecuteLaunchDialerAsync(t)));
        async Task ExecuteLaunchDialerAsync(string arg)
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                
                var service = DependencyService.Get<IPhoneDialer>();

                if (service != null)
                    service.LaunchCall(arg.Replace(" ", ""));

                Analytics.TrackEvent("Phone_Dialer_Place_Details", new Dictionary<string, string> { { "Action", "User initiated call to place." }});
            }
            catch (Exception ex) { }
            finally
            { IsBusy = false; }
        }


        /// <summary>
        /// Open Twitter to my profile
        /// </summary>
        public ICommand OpenSocialProfile => new Command<string>(async (t) => await OpenSocialProfileCommand(t));
        async Task OpenSocialProfileCommand(string arg)
        {
            try
            {
                //Open the app to share the message
                var shareService = DependencyService.Get<ISharer>();
                if (shareService != null)
                {
                    if (shareService.OpenUserName("Raidzen10"))
                        return;
                }

                LaunchBrowserCommand.Execute(arg);
            }
            catch { }
        }


        /// <summary>
        /// Open the share options
        /// </summary>
        ICommand shareCommand;
        public ICommand ShareCommand => shareCommand ?? (shareCommand = new Command<string>(async (m) => await ExecuteShareCommandAsync(m)));
        public async Task ExecuteShareCommandAsync(string message)
        {
            Analytics.TrackEvent("Open_Share", new Dictionary<string, string> { { "Action", "User opened share dialog." } });
            await CrossShare.Current.Share(message, "Share");
        }


        /// <summary>
        /// Open the devices respective store and direct to app revies
        /// </summary>
        public ICommand OpenReviewsCommand => new Command(async () => await OpenReviews());
        public async Task OpenReviews()
        {
            try
            {
                if (CrossDeviceInfo.Current.Platform == Platform.iOS)
                {
                    AppLauncher.OpenReviewsAppStore($"itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?id={GlobalKeys.AppStoreID}&onlyLatestVersion=true&pageNumber=0&sortOrdering=1&type=Purple+Software");
                }
                else
                {
                    ExecuteLaunchBrowserAsync($"https://play.google.com/store/apps/details?id={GlobalKeys.PlayAppStoreID}");
                }

                Analytics.TrackEvent("Open_reviews", new Dictionary<string, string> { { "Action", "User opened app reviews." } });
            }
            catch(Exception ex)
            { }
        }





        #region Global Methods
        
        public async Task ShowToast(string arg)
        {
            Toaster.SendToast(arg);
        }

        public async Task ShareToProvider(string provider, string stareText = "")
        {
            try
            {
                var successShare = false;

                //Open the app to share the message
                var shareService = DependencyService.Get<ISharer>();
                if (shareService != null)
                {
                    switch (provider.ToLower())
                    {
                        case "facebook":
                            successShare = shareService.PostToFacebook(stareText);
                            break;
                        case "twitter":
                            successShare = shareService.SendTweet(stareText);
                            break;
                    }
                }

                //If the app is not installed, open the browser
                if (!successShare)
                {
                    switch (provider.ToLower())
                    {
                        case "facebook":
                            LaunchBrowserCommand.Execute("https://www.facebook.com/");
                            break;
                        case "twitter":
                            LaunchBrowserCommand.Execute("https://twitter.com/");
                            break;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        public async Task<Plugin.Geolocator.Abstractions.Position> UpdateCurrentLocation()
        {
            Plugin.Geolocator.Abstractions.Position position = new Plugin.Geolocator.Abstractions.Position();

            try
            {
                if (!Settings.Current.CustomLocationEnabled)
                {
                    //Get the users current location
                    var locator = CrossGeolocator.Current;
                    position = await locator.GetPositionAsync(10000);

                }
                else
                {
                    position = new Plugin.Geolocator.Abstractions.Position
                    {
                        Latitude = Convert.ToDouble(Settings.Current.CustomLatitude),
                        Longitude = Convert.ToDouble(Settings.Current.CustomLongitude)
                    };
                }
            }
            catch { }

            return position;
        }

        #endregion

        #region Internals

        public void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = GetMemberInfo(property).Name;
            OnPropertyChanged(name);
        }
        private MemberInfo GetMemberInfo(Expression expression)
        {
            MemberExpression operand;
            LambdaExpression lambdaExpression = (LambdaExpression)expression;
            if (lambdaExpression.Body as UnaryExpression != null)
            {
                UnaryExpression body = (UnaryExpression)lambdaExpression.Body;
                operand = (MemberExpression)body.Operand;
            }
            else
            {
                operand = (MemberExpression)lambdaExpression.Body;
            }
            return operand.Member;
        }

        #endregion
    }
}
