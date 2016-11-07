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
using Amazon;
using Amazon.CognitoSync;
using Amazon.CognitoSync.SyncManager;
using Nearby.Utils;

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

            NearbyBaseViewModel.Init();

            NavPage = new NavigationPage(new Home());

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

        protected override void OnSleep()
        {
            //loc = null;
        }

        protected override void OnResume()
        {
            
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
        
        public static Action SuccessfulLoginAction
        {
            get
            {
                return new Action(() => {
                    NavPage.Navigation.InsertPageBefore(new Home(), NavPage.Navigation.NavigationStack.First());
                    NavPage.Navigation.PopToRootAsync();
                    //NavPage.Navigation.PopModalAsync();

                    //if (IsLoggedIn)
                    //{
                    //    NavPage.Navigation.InsertPageBefore(new TodoListPage(), NavPage.Navigation.NavigationStack.First());
                    //    NavPage.Navigation.PopToRootAsync();
                    //}
                });
            }
        }
    }
}
