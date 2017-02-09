using MvvmHelpers;
using Nearby.Pages;
using Nearby.Utils;
using Nearby.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Nearby.Helpers;
using Microsoft.Azure.Mobile.Analytics;

namespace Nearby.viewModel
{
    public class MainMenuViewModel : NearbyBaseViewModel
    {
        public List<MenuItem> ManualItems { get; } = new List<MenuItem>();
        public ObservableRangeCollection<AboutMenuItem> AboutItems { get; } = new ObservableRangeCollection<AboutMenuItem>();
        public ObservableRangeCollection<AccountMenuItem> AccountItems { get; } = new ObservableRangeCollection<AccountMenuItem>();
        public ObservableRangeCollection<AboutMenuItem> TermsItems { get; } = new ObservableRangeCollection<AboutMenuItem>();

        public string madeByText = $"By Gerade";
        public string MadeByText
        {
            get { return madeByText; }
            set { SetProperty(ref madeByText, value); }
        }

        public MainMenuViewModel() 
        {
            Title = "Settings";

            ChangeLocationIsEnabled = Settings.Current.CustomLocationEnabled;

            //Add menu item to navigate to favs on IOS only
            if (Device.OS == TargetPlatform.iOS)
            {
                ManualItems.Add(new MenuItem
                {
                    DetailLabel = "Favourites",
                    MenuItemCommand = NavigateToFavourites,
                    isSwitch = false
                });
            }

            ManualItems.Add(new MenuItem
            {
                DetailLabel = "Use custom location?",
                DetailValue = ChangeLocationIsEnabled,
                MenuItemCommand = ToggleCustomLocation,
                isSwitch = true
            });


            if (Settings.Current.CustomLocationEnabled)
            {
                CustomLatitude = Settings.Current.CustomLatitude;
                CustomLongitude = Settings.Current.CustomLongitude;
            }

            if (Settings.Current.CustomLocation != "")
                CustomLocation = Settings.Current.CustomLocation;

            AboutItems.AddRange(new[]
            {
                new AboutMenuItem { Label = "Created by", Value = "Gerade Geldenhuys"},
                new AboutMenuItem { Label = "Version 1.0", Value = "Copyright " + DateTime.Now.Year},
                new AboutMenuItem { Label = "Feedback", Value = "Have a say", AboutCommand = OpenReviewsCommand, Disclosure = "disclosre"},
                new AboutMenuItem { Label = "About This App", AboutCommand = NavigateToAbout, Disclosure = "disclosre"},
            });

            TermsItems.AddRange(new[]
            {
                new AboutMenuItem { Label = "Terms Of Use", Value = "terms", AboutCommand = NavigateTerms }
            });
        }

        public void UpdateItems()
        {
            if (Settings.Current.CustomLocation != "")
                CustomLocation = Settings.Current.CustomLocation;

            ChangeLocationIsEnabled = Settings.Current.CustomLocationEnabled;

            if (Settings.Current.CustomLocationEnabled)
            {
                CustomLatitude = Settings.Current.CustomLatitude;
                CustomLongitude = Settings.Current.CustomLongitude;
            }
        }

        string changelocationtext = "Change Location";
        public string ChangeLocationText
        {
            get { return changelocationtext; }
            set { changelocationtext = value; }
        }


        bool changelocationIsEnabled = false;
        public bool ChangeLocationIsEnabled
        {
            get { return changelocationIsEnabled; }
            set
            {
                SetProperty(ref changelocationIsEnabled, value);
                Settings.Current.CustomLocationEnabled = value;

                if (!value)
                {
                    Settings.Current.CustomLocation = "";
                    Settings.Current.CustomLatitude = "";
                    Settings.Current.CustomLongitude = "";
                }
            }
        }

        string customLatitude = "";
        public string CustomLatitude
        {
            get { return customLatitude; }
            set
            {
                SetProperty(ref customLatitude, value);
                Settings.Current.CustomLatitude = value;
            }
        }

        string customLongitude = "";
        public string CustomLongitude
        {
            get { return customLongitude; }
            set
            {
                SetProperty(ref customLongitude, value);
                Settings.Current.CustomLongitude = value;
            }
        }


        string customLocation = "";
        public string CustomLocation
        {
            get { return customLocation; }
            set
            {
                SetProperty(ref customLocation, value);
                Settings.Current.CustomLocation = value;
            }
        }


        ICommand toggleCustomLocation;
        public ICommand ToggleCustomLocation => toggleCustomLocation ?? (toggleCustomLocation = new Command<object>(async (e) => await UpdateCustomLocation(e)));
        async Task UpdateCustomLocation(object e)
        {
            SwitchCell custLocation = (SwitchCell)e;
            ChangeLocationIsEnabled = custLocation.On;

            //Update setting to remove custom location
            if (!custLocation.On)
            {
                CustomLocation = "";
                CustomLongitude = "";
                CustomLatitude = "";
            }
            else
            {

            }

            Analytics.TrackEvent("Custome_Location_Toggle", new Dictionary<string, string> { { "Action", "User toggled custom location setting." } });
        }


        ICommand navigateToSearch;
        public ICommand NavigateToSearch => navigateToSearch ?? (navigateToSearch = new Command(async () => {
            if (ChangeLocationIsEnabled)
                await Navigation.PushAsync(new SearchCustomPlaces());
            }));

        ICommand navigateToAbout;
        public ICommand NavigateToAbout => navigateToAbout ?? (navigateToAbout = new Command(async () => {
                await Navigation.PushAsync(new AboutApp());
        }));

        ICommand navigateTerms;
        public ICommand NavigateTerms => navigateTerms ?? (navigateTerms = new Command(async () => {
            await Navigation.PushAsync(new TermsAndConditions());
        }));


        ICommand toggleFiltern;
        public ICommand ToggleFiltern => toggleFiltern ?? (toggleFiltern = new Command<object>(async (e) => await SetFilterStatus(e)));
        async Task SetFilterStatus(object e)
        {
            SwitchCell filter = (SwitchCell)e;

            if (filter.On)
                Settings.Current.SearchFilters = filter.Text;
            else
            {
                if (Settings.Current.SearchFilters == filter.Text)
                    Settings.Current.SearchFilters = "";
            }

            if (Settings.Current.SearchFilters == "")
                Settings.Current.IsSearchFilterEnabled = false;
            else
            {
                Settings.Current.IsSearchFilterEnabled = true;
            }
        }


        public ICommand NavigateToFavourites => new Command(NavigateToFavouritesCommand);
        private async void NavigateToFavouritesCommand()
        {
            await Navigation.PushAsync(new SearchCustomPlaces());
        }
    }

    public class MenuItem
    {
        bool detailValue;

        public String DetailLabel { get; set; }
        public bool DetailValue { get; set; }
        public ICommand MenuItemCommand { get; set; }
        public String MenuItemCommandProperty { get; set; }
        public Boolean isSwitch { get; set; }
    }

    public class AccountMenuItem
    {
        public String ProviderLabel { get; set; }
        public String ProviderValue { get; set; }
        public ICommand ProviderCommand { get; set; }
        public String ProviderCommandProperty { get; set; }
    }

    public class AboutMenuItem
    {
        public String Label { get; set; }
        public String Value { get; set; }
        public ICommand AboutCommand { get; set; }
        public String AboutCommandProperty { get; set; }
        public string Disclosure { get; set; }

        public AboutMenuItem()
        {
            Disclosure = "none";
        }
    }
}
