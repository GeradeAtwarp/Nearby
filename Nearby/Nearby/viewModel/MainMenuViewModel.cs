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

namespace Nearby.viewModel
{
    public class MainMenuViewModel : NearbyBaseViewModel
    {
        public List<MenuItem> ManualItems { get; } = new List<MenuItem>();

        public MainMenuViewModel(INavigation navigation) : base(navigation)
        {
            Title = "Options";

            ChangeLocationIsEnabled = Settings.CustomLocationEnabled;

            ManualItems.Add(new MenuItem
            {
                DetailLabel = "Manual location",
                DetailValue = ChangeLocationIsEnabled,
                MenuItemCommand = ToggleCustomLocation
            });

            if (Settings.CustomLocationEnabled)
            {
                CustomLatitude = Settings.CustomLatitude;
                CustomLongitude = Settings.CustomLongitude;
            }

            CustomLocation = Settings.CustomLocation;
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
                Settings.CustomLocationEnabled = value;
            }
        }

        string customLatitude = "";
        public string CustomLatitude
        {
            get { return customLatitude; }
            set
            {
                SetProperty(ref customLatitude, value);
                Settings.CustomLatitude = value;
            }
        }

        string customLongitude = "";
        public string CustomLongitude
        {
            get { return customLongitude; }
            set
            {
                SetProperty(ref customLongitude, value);
                Settings.CustomLongitude = value;
            }
        }


        string customLocation = "";
        public string CustomLocation
        {
            get { return customLocation; }
            set
            {
                SetProperty(ref customLocation, value);
                Settings.CustomLocation = value;
            }
        }


        ICommand toggleCustomLocation;
        public ICommand ToggleCustomLocation => toggleCustomLocation ?? (toggleCustomLocation = new Command<object>(async (e) => await UpdateCustomLocation(e)));

        async Task UpdateCustomLocation(object e)
        {
            var vals = e as ToggledEventArgs;
            ChangeLocationIsEnabled = vals.Value;
        }


        ICommand navigateToSearch;
        public ICommand NavigateToSearch => navigateToSearch ?? (navigateToSearch = new Command(async () => await Navigation.PushAsync(new SearchCustomPlaces())));












        public class MenuItem
        {
            public String DetailLabel { get; set; }
            public object DetailValue { get; set; }
            public ICommand MenuItemCommand { get; set; }
        }
    }
}
