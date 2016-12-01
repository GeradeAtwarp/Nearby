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
        public ObservableRangeCollection<MenuItem> FilterItems { get; } = new ObservableRangeCollection<MenuItem>();
        public ObservableRangeCollection<AccountMenuItem> AccountItems { get; } = new ObservableRangeCollection<AccountMenuItem>();

        public MainMenuViewModel(INavigation navigation) : base(navigation)
        {
            Title = "Settings";

            ChangeLocationIsEnabled = Settings.CustomLocationEnabled;

            ManualItems.Add(new MenuItem
            {
                DetailLabel = "Use custom location?",
                DetailValue = ChangeLocationIsEnabled,
                MenuItemCommand = ToggleCustomLocation
            });

            if (Settings.CustomLocationEnabled)
            {
                CustomLatitude = Settings.CustomLatitude;
                CustomLongitude = Settings.CustomLongitude;
            }

            if (Settings.CustomLocation != "")
                CustomLocation = Settings.CustomLocation;

            FilterItems.AddRange(new[]
               {
                    new MenuItem { DetailLabel = "Restaurant", DetailValue = Settings.SearchFilters.Contains("Restaurant"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Restaurants"  },
                    new MenuItem { DetailLabel = "Bar", DetailValue = Settings.SearchFilters.Contains("Bar"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Bar"},
                    new MenuItem { DetailLabel = "Accomodation", DetailValue = Settings.SearchFilters.Contains("Accomodation"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Bar"},
                    new MenuItem { DetailLabel = "Cafe", DetailValue = Settings.SearchFilters.Contains("Cafe"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Cafe"  },
                    new MenuItem { DetailLabel = "Gas station",DetailValue = Settings.SearchFilters.Contains("Gas station"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Gas"  },
                    new MenuItem { DetailLabel = "Parking", DetailValue = Settings.SearchFilters.Contains("Parking") , MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Parking"  },
                    new MenuItem { DetailLabel = "Night club",DetailValue = Settings.SearchFilters.Contains("Night club"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Night_club"   },
                    new MenuItem { DetailLabel = "Movie Theater",DetailValue = Settings.SearchFilters.Contains("Movie Theater"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Movie"   },
                    new MenuItem { DetailLabel = "Liquor store",DetailValue = Settings.SearchFilters.Contains("Liquor store"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Liquor"   }
            });

            AccountItems.Add(new AccountMenuItem
            {
                ProviderLabel = "Google",
                ProviderValue = "google",
            });
        }

        public void UpdateItems()
        {
            CustomLocation = Settings.CustomLocation;
            ChangeLocationIsEnabled = Settings.CustomLocationEnabled;

            if (Settings.CustomLocationEnabled)
            {
                CustomLatitude = Settings.CustomLatitude;
                CustomLongitude = Settings.CustomLongitude;
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
                Settings.CustomLocationEnabled = value;

                if (!value)
                {
                    Settings.CustomLocation = "";
                    Settings.CustomLatitude = "";
                    Settings.CustomLongitude = "";
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


        string customLocation = "Not set";
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
                Settings.SearchFilters = filter.Text;
            else
            {
                if (Settings.SearchFilters == filter.Text)
                    Settings.SearchFilters = "";
            }

            if (Settings.SearchFilters == "")
                Settings.IsSearchFilterEnabled = false;
            else
            {
                Settings.IsSearchFilterEnabled = true;

                foreach (var category in FilterItems.Where(x => x.DetailLabel != filter.Text))
                {
                    category.DetailValue = false;
                }

                FilterItems.ReplaceRange(new[]
                  {
                        new MenuItem { DetailLabel = "Restaurant", DetailValue = Settings.SearchFilters.Contains("Restaurant"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Restaurants"  },
                        new MenuItem { DetailLabel = "Bar", DetailValue = Settings.SearchFilters.Contains("Bar"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Bar"},
                        new MenuItem { DetailLabel = "Accomodation", DetailValue = Settings.SearchFilters.Contains("Accomodation"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Bar"},
                        new MenuItem { DetailLabel = "Cafe", DetailValue = Settings.SearchFilters.Contains("Cafe"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Cafe"  },
                        new MenuItem { DetailLabel = "Gas station",DetailValue = Settings.SearchFilters.Contains("Gas station"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Gas"  },
                        new MenuItem { DetailLabel = "Parking", DetailValue = Settings.SearchFilters.Contains("Parking") , MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Parking"  },
                        new MenuItem { DetailLabel = "Night club",DetailValue = Settings.SearchFilters.Contains("Night club"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Night_club"   },
                        new MenuItem { DetailLabel = "Movie Theater",DetailValue = Settings.SearchFilters.Contains("Movie Theater"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Movie"   },
                        new MenuItem { DetailLabel = "Liquor store",DetailValue = Settings.SearchFilters.Contains("Liquor store"), MenuItemCommand = ToggleFiltern, MenuItemCommandProperty = "Liquor"   }
                });
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
    }
}
