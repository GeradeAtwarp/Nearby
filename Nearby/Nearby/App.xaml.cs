﻿using System;
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

namespace Nearby
{
    public partial class App : Application
    {
        public static App current;

        public App()
        {
            current = this;
            InitializeComponent();

            NearbyBaseViewModel.Init();

            var navPage = new NavigationPage(new Home());

            // set the MainPage of the app to the navPage
            MainPage = navPage;

            if (Device.OS == TargetPlatform.iOS)
            {
                MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromHex("#3F51B5"));
                MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            //loc = null;
        }

        protected override void OnResume()
        {

            MessagingService.Current.Subscribe<MessagingServiceQuestion>(MessageKeys.Question, async (m, q) =>
            {
                var task = Application.Current?.MainPage?.DisplayAlert(q.Title, q.Question, q.Positive, q.Negative);
                if (task == null)
                    return;
                var result = await task;
                q?.OnCompleted?.Invoke(result);
            });
        }
    }
}
