﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nearby.Pages;
using Xamarin.Forms;
using Nearby.DependencyServices;
using Nearby.Controls;

namespace Nearby
{
    public partial class App : Application
    {
        public static App current;

        public App()
        {
            current = this;
            InitializeComponent();

            MainPage = new NearbyNavigationPage
            (
                new Home()
            );
        }

        protected override void OnStart()
        {
            //// Handle when your app starts
            //loc = DependencyService.Get<IMyLocation>();
            //loc.locationObtained += (object sender,
            //    ILocationEventArgs e) => {
            //        var lat = e.lat;
            //        var lng = e.lng;
            //        latitude = lat;
            //        longitude = lng;
            //    };
            //loc.ObtainMyLocation();
        }

        protected override void OnSleep()
        {
            //loc = null;
        }

        protected override void OnResume()
        {
            //if(loc == null)
            //    loc = DependencyService.Get<IMyLocation>();

            //// Handle when your app starts
            //loc.locationObtained += (object sender,
            //    ILocationEventArgs e) => {
            //        var lat = e.lat;
            //        var lng = e.lng;
            //        latitude = lat;
            //        longitude = lng;
            //    };
            //loc.ObtainMyLocation();
        }
    }
}
