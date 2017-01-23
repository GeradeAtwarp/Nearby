using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nearby.Pages;
using Xamarin.Forms;
using Nearby.DependencyServices;
using Nearby.Controls;
using Nearby.viewModel;
using FormsToolkit;
using Nearby.Helpers;
using Nearby.Utils;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Nearby
{
    public partial class App : Application
    {
        public static App current;
        static NavigationPage NavPage;

        public static string AppName { get { return "GeradeDevNearbyApp"; } }
        public static User User { get; set; }

        public App()
        {
            current = this;
            InitializeComponent();

            Settings.Current.DidNotifyUserOnStart = false;

            NavPage = new NavigationPage(new Home());

            NearbyBaseViewModel.Init(NavPage.Navigation);

            User = new User();

            // set the MainPage of the app to the navPage
            MainPage = NavPage;

            if (Device.OS == TargetPlatform.iOS)
            {
                MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromHex("#3F51B5"));
                MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);
            }
        }

        protected override void OnStart()
        {
            OnResume();
        }

        protected override void OnResume()
        {
            Settings.Current.IsConnected = CrossConnectivity.Current.IsConnected;
            CrossConnectivity.Current.ConnectivityChanged += ConnectivityChanged;

            //Start messaging service to display alert
            MessagingService.Current.Subscribe<MessagingServiceAlert>(MessageKeys.Message, async (m, info) =>
            {
                var task = Application.Current?.MainPage?.DisplayAlert(info.Title, info.Message, info.Cancel);

                if (task == null)
                    return;

                await task;
                info?.OnCompleted?.Invoke();
            });

            //Confirm alert messaging service
            MessagingService.Current.Subscribe<MessagingServiceQuestion>(MessageKeys.Question, async (m, q) =>
            {
                var task = Application.Current?.MainPage?.DisplayAlert(q.Title, q.Question, q.Positive, q.Negative);
                if (task == null)
                    return;
                var result = await task;
                q?.OnCompleted?.Invoke(result);
            });
        }

        protected override void OnSleep()
        {
            MessagingService.Current.Unsubscribe<MessagingServiceQuestion>(MessageKeys.Question);
            MessagingService.Current.Unsubscribe<MessagingServiceAlert>(MessageKeys.Message);
            
            CrossConnectivity.Current.ConnectivityChanged -= ConnectivityChanged;
        }


        protected async void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            //save current state and then set it
            var connected = Settings.Current.IsConnected;
            Settings.Current.IsConnected = e.IsConnected;
            if (connected && !e.IsConnected)
            {
                //we went offline, should alert the user and also update ui (done via settings)
                var task = Application.Current?.MainPage?.DisplayAlert("Offline", "Oh snap! you have gone offline. Please check your internet connection.", "OK");
                if (task != null)
                    await task;
            }
        }
    }
}
