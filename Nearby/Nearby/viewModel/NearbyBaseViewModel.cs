﻿using MvvmHelpers;
using Nearby.Helpers;
using Nearby.Interfaces;
using Nearby.Utils;
using Nearby.Utils.Entities;
using Plugin.Geolocator;
using Plugin.Share;
using Plugin.Share.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Nearby.viewModel
{
    public class NearbyBaseViewModel : BaseViewModel
    {
        protected INavigation Navigation { get; }
        protected Database NearbyDataContext { get; }
        protected TextInfo CultureTextInfo { get; }

        public NearbyBaseViewModel(INavigation navigation = null)
        {
            Navigation = navigation;
            NearbyDataContext = new Database();

            // Creates a TextInfo based on the "en-US" culture.
            CultureTextInfo = new CultureInfo("en-US").TextInfo;
        }

        public static void Init(bool mock = true)
        {
            //CognitoSyncManager syncManager = new CognitoSyncManager(AWSUtils.Credentials, new AmazonCognitoSyncConfig { RegionEndpoint = RegionEndpoint.USWest2 });\

            //dynamoClient = new AmazonDynamoDBClient(AWSUtils.Credentials, RegionEndpoint.USWest2);
            //dynamoContext = new DynamoDBContext(dynamoClient);
        }

        public Settings Settings
        {
            get { return Settings.Current; }
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



        public ICommand launchBrowserCommand;
        public ICommand LaunchBrowserCommand =>
        launchBrowserCommand ?? (launchBrowserCommand = new Command<string>(async (t) => await ExecuteLaunchBrowserAsync(t)));

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



        public ICommand launchDialerCommand;
        public ICommand LaunchDialerCommand =>
        launchDialerCommand ?? (launchDialerCommand = new Command<string>(async (t) => await ExecuteLaunchDialerAsync(t)));

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

            }
            catch (Exception ex) { }
            finally
            { IsBusy = false; }
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
    }
}
