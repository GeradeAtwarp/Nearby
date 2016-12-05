using MvvmHelpers;
using Nearby.Helpers;
using Nearby.Interfaces;
using Nearby.Utils;
using Nearby.Utils.Entities;
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
                    service.LaunchCall(arg);

            }
            catch (Exception ex) { }
            finally
            { IsBusy = false; }
        }
    }
}
