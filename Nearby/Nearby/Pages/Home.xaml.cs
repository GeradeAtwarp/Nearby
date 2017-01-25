using FAB.Forms;
using FormsToolkit;
using Nearby.Controls;
using Nearby.DependencyServices;
using Nearby.Helpers;
using Nearby.Interfaces;
using Nearby.Utils;
using Nearby.viewModel;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Nearby.Pages
{
    public partial class Home : ContentPage
    {
        HomeViewModel vm;

        IMyLocation loc;
        public double longitude;
        public double latitude;

        public Home()
        {
            InitializeComponent();

            BindingContext = vm = new HomeViewModel();

            var tbItemNavigateEvents = new ToolbarItem() { Icon = "today" };
            var tbItemNavigateFav = new ToolbarItem() { Icon = "favorite"};
            var tbItemNavigateOptions = new ToolbarItem() { Icon = "settings_cog"};

            //Navigate to nearby page
            tbItemNavigateEvents.Command = new Command(async () =>
            {
                var nav = Application.Current?.MainPage?.Navigation;
                if (nav == null)
                    return;

                if (vm.IsBusy)
                    return;

                await Navigation.PushAsync(new NearbyEvents());
            });

            //Navigate to favs page
            tbItemNavigateFav.Command = new Command(async () =>
            {
                var nav = Application.Current?.MainPage?.Navigation;
                if (nav == null)
                    return;
                await Navigation.PushAsync(new Favourites());
            });

            //Navigate to options page
            tbItemNavigateOptions.Command = new Command(async () =>
            {
                var nav = Application.Current?.MainPage?.Navigation;
                if (nav == null)
                    return;

                if (vm.IsBusy)
                    return;

                await Navigation.PushAsync(new MainMenu());
            });

            if (Device.OS != TargetPlatform.Android)
            {
                ToolbarItems.Add(tbItemNavigateEvents);
                ToolbarItems.Add(tbItemNavigateOptions);
            }
            else
            {
                ToolbarItems.Add(tbItemNavigateEvents);
                ToolbarItems.Add(tbItemNavigateFav);
                ToolbarItems.Add(tbItemNavigateOptions);
            }
                       

            btnRemoveFilter.Clicked += async (sender, e) =>
            {
                vm.DeactivateFilter().ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
                {
                    SearchForPlacesNearby();
                }));
            };
            
            AddSearchButtons();            
            MoveToCurrentLocation();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.UpdateItems();

            MoveToCurrentLocation();

            //Notify user on statup that a filter is currently enabled
            if (!Settings.Current.DidNotifyUserOnStart && Settings.Current.IsSearchFilterEnabled)
            {
                vm.ShowToast("You currently have a search filter enabled.");
                Settings.Current.DidNotifyUserOnStart = true;
            }
        }

        async Task MoveToCurrentLocation()
        {
            try
            {
                if (RefineSearchMenu.Scale > 0)
                {
                    await RefineSearchMenu.ScaleTo(0, 250, Easing.SinOut);
                    RefineSearchMenu.IsVisible = false;
                }

                if (placesMap.Pins.Count() == 0)
                {
                    Plugin.Geolocator.Abstractions.Position position;

                    if (!Settings.Current.CustomLocationEnabled)
                    {
                        //Get the users current location
                        var locator = CrossGeolocator.Current;
                        position = await locator.GetPositionAsync(10000);

                        var pin = new Pin
                        {
                            Type = PinType.Place,
                            Label = "This is you!",
                            Position = new Position(position.Latitude, position.Longitude)
                        };
                        
                        placesMap.Pins.Add(pin);
                        placesMap.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMiles(0.5)));
                    }
                    else
                    {
                        if (Settings.Current.CustomLatitude == "" || Settings.Current.CustomLongitude == "")
                            Application.Current?.MainPage.DisplayAlert("Location", "Please set a custom location, Or turn off the custom location option on the settings page.", "Got it!");
                        else
                        {
                            position = new Plugin.Geolocator.Abstractions.Position
                            {
                                Latitude = Convert.ToDouble(Settings.Current.CustomLatitude),
                                Longitude = Convert.ToDouble(Settings.Current.CustomLongitude)
                            };

                            var pin = new Pin
                            {
                                Type = PinType.SavedPin,
                                Label = "This is you!",
                                Position = new Position(position.Latitude, position.Longitude)
                            };

                            placesMap.Pins.Add(pin);
                            placesMap.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMiles(0.5)));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async Task SearchForPlacesNearby()
        {
            if (RefineSearchMenu.Scale > 0)
            {
                await RefineSearchMenu.ScaleTo(0, 250, Easing.SinOut);
                RefineSearchMenu.IsVisible = false;
            }

            await vm.SearchNearby("");

            try
            {
                placesMap.Pins.Clear();

                foreach (var pn in vm.PlacesNearby)
                {
                    var newposition = new Xamarin.Forms.Maps.Position(pn.geometry.location.lat, pn.geometry.location.lng);

                    var pin = new Pin
                    {
                        Type = PinType.Place,
                        Position = newposition,
                        Label = pn.name,
                        Address = pn.vicinity
                    };

                    pin.Clicked += async (sender, e) =>
                    {
                        if (Settings.Current.IsConnected)
                        {
                            await Navigation.PushAsync(new PlaceDetailView(pn));
                        }
                        else
                        {
                            MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert { Title = "Offline", Message = "Oh snap! you have gone offline. Please check your internet connection.", Cancel = "Ok" });
                        }
                    };

                    placesMap.Pins.Add(pin);
                }

                MoveToCurrentLocation();
            }
            catch (Exception ex)
            {

            }
        }

        async Task ToggleRefineOptions()
        {
            if (RefineSearchMenu.Scale == 0)
            {
                //fabrefine.ColorNormal = Color.FromHex("#3F51B5");
                RefineSearchMenu.IsVisible = true;
                await RefineSearchMenu.ScaleTo(1, 250, Easing.SinIn);
            }
            else
            {
                //fabrefine.ColorNormal = Color.FromHex("#7885cb");
                await RefineSearchMenu.ScaleTo(0, 250, Easing.SinOut);
                RefineSearchMenu.IsVisible = false;
            }
        }

        async Task GetMyCurrentLocation()
        {


            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            locator.PositionChanged += (sender, e) => {
                var newposition = new Xamarin.Forms.Maps.Position(e.Position.Latitude, e.Position.Longitude);

                var pin = new Pin
                {
                    Type = PinType.SearchResult,
                    Position = newposition,
                    Label = "Me",
                    Address = "My current updated location"
                };

                placesMap.Pins.Add(pin);
                placesMap.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMiles(5.0)));

                Debug.WriteLine("Updated Position Latitude: {0}", newposition.Latitude);
                Debug.WriteLine("Updated Position Longitude: {0}", newposition.Longitude);
            };

            var position = await locator.GetPositionAsync(10000);
            var currposition = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);

            var p = new Pin
            {
                Type = PinType.Place,
                Position = currposition,
                Label = "Me",
                Address = "My current location"
            };

            placesMap.Pins.Add(p);
            placesMap.MoveToRegion(MapSpan.FromCenterAndRadius(p.Position, Distance.FromMiles(5.0)));

            Debug.WriteLine("Position Status: {0}", position.Timestamp);
            Debug.WriteLine("Position Latitude: {0}", position.Latitude);
            Debug.WriteLine("Position Longitude: {0}", position.Longitude);

            DisplayAlert("Location", position.Longitude + "---" + position.Latitude, "Ok");
        }

        async Task AddSearchButtons()
        {
            if(Device.OS == TargetPlatform.Android)
            {
                SearchButton.Children.Add(new FloatingActionButtonView
                {
                    Size = FloatingActionButtonSize.Mini,
                    ImageName = "search_small",
                    ColorNormal = Color.FromHex("#3F51B5"),
                    ColorPressed = Color.FromHex("#7885cb"),
                    ColorRipple = Color.FromHex("#2C3E50"),
                    Clicked = (sender, ea) => SearchForPlacesNearby()
                });

                RefineButton.Children.Add(new FloatingActionButtonView
                {
                    Size = FloatingActionButtonSize.Mini,
                    ImageName = "ic_more_vert_white",
                    ColorNormal = Color.FromHex("#3F51B5"),
                    ColorPressed = Color.FromHex("#7885cb"),
                    ColorRipple = Color.FromHex("#2C3E50"),
                    Clicked = (sender, e) => ToggleRefineOptions()
                });
            }
            else
            {
                //FloatingActionButton searchFab = new FloatingActionButton();
                //searchFab.Source = "search_small.png";
                //searchFab.Size = FabSize.Normal;
                //searchFab.NormalColor = Color.FromHex("#3F51B5");
                //searchFab.RippleColor = Color.FromHex("#2C3E50");

                //FloatingActionButton refineFab = new FloatingActionButton();
                //refineFab.Source = "ic_more_vert_white.png";
                //refineFab.Size = FabSize.Normal;
                //refineFab.NormalColor = Color.FromHex("#3F51B5");
                //refineFab.RippleColor = Color.FromHex("#2C3E50");


                Button btnSearch = new Button
                {
                    BorderRadius = 20,
                    BackgroundColor = Color.FromHex("#3F51B5"),
                    //BorderColor = Color.FromHex("#3F51B5"),
                    //BorderWidth = 2,
                    WidthRequest = 100,
                    TextColor = Color.White,
                    Text = "Search",
                    FontAttributes = FontAttributes.Bold,
                    HeightRequest = 40
                };

                btnSearch.Clicked += (sender, ea) => SearchForPlacesNearby();

                Button btnRefineSearch = new Button
                {
                    BorderRadius = 20,
                    BackgroundColor = Color.FromHex("#3F51B5"),
                    //BorderColor = Color.FromHex("#3F51B5"),
                    //BorderWidth = 2,
                    WidthRequest = 100,
                    TextColor = Color.White,
                    Text = "Filter",
                    FontAttributes = FontAttributes.Bold,
                    HeightRequest = 40
                };

                btnRefineSearch.Clicked += (sender, e) => ToggleRefineOptions();

                SearchButton.Children.Add(btnSearch);
                RefineButton.Children.Add(btnRefineSearch);
            }
        }

        void SetFilter(object sender, EventArgs args)
        {
            var image = (Xamarin.Forms.Image)sender;
            var source = image.Source as FileImageSource;

            if (source != null)
            {
                switch (source.File)
                {
                    case "pizza":
                        vm.SetActiveFilter.Execute("restaurant");
                        break;
                    case "apartment":
                        vm.SetActiveFilter.Execute("lodging");
                        break;
                    case "car":
                        vm.SetActiveFilter.Execute("parking");
                        break;
                    case "documentary":
                        vm.SetActiveFilter.Execute("movie_theater");
                        break;
                    case "beer_bottle":
                        vm.SetActiveFilter.Execute("liquor_store");
                        break;
                    case "coffee":
                        vm.SetActiveFilter.Execute("cafe");
                        break;
                }

                SearchForPlacesNearby();
            }
        }



        public class Locations
        {
            public double longitude { get; set; }
            public double latitude { get; set; }
            public string LocationName { get; set; }
            public string Description { get; set; }
        }
    }
}
