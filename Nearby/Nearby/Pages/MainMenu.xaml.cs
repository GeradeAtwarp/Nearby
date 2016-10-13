﻿using Nearby.Controls;
using Nearby.Helpers;
using Nearby.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Nearby.Pages
{
    public partial class MainMenu : ContentPage
    {
        public MainMenu()
        {
            InitializeComponent();

            BindingContext = new MainMenuViewModel(Navigation);

            tbItemNavigateMap.Command = new Command(async () =>
            {
                var nav = Application.Current?.MainPage?.Navigation;
                if (nav == null)
                    return;

                await NavigationService.PushModalAsync(nav, new NearbyNavigationPage(new Home()));
            });
        }
    }
}
