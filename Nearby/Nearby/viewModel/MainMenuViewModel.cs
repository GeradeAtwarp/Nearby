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

namespace Nearby.viewModel
{
    public class MainMenuViewModel : NearbyBaseViewModel
    {
        public List<MenuItem> ManualItems { get; } = new List<MenuItem>();
        public ObservableRangeCollection<AboutMenuItem> AboutItems { get; } = new ObservableRangeCollection<AboutMenuItem>();
        public ObservableRangeCollection<AccountMenuItem> AccountItems { get; } = new ObservableRangeCollection<AccountMenuItem>();

        public MainMenuViewModel(INavigation navigation) : base(navigation)
        {
            Title = "Settings";

            ChangeLocationIsEnabled = Settings.Current.CustomLocationEnabled;

            ManualItems.Add(new MenuItem
            {
                DetailLabel = "Use custom location?",
                DetailValue = ChangeLocationIsEnabled,
                MenuItemCommand = ToggleCustomLocation
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
                    new AboutMenuItem { Label = "Terms Of Use", Value = "terms" },
                    //new AboutMenuItem { Label = "About this app", Value = "about" },
            });

            AccountItems.Add(new AccountMenuItem
            {
                ProviderLabel = "Google",
                ProviderValue = "google",
            });
        }

        public void UpdateItems()
        {
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


        string customLocation = "Not set";
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
        }


        ICommand navigateToSearch;
        public ICommand NavigateToSearch => navigateToSearch ?? (navigateToSearch = new Command(async () => await Navigation.PushAsync(new SearchCustomPlaces())));


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
    }

    public class MenuItem
    {
        bool detailValue;

        public String DetailLabel { get; set; }
        public bool DetailValue { get; set; }
        public ICommand MenuItemCommand { get; set; }
        public String MenuItemCommandProperty { get; set; }
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
    }
}
