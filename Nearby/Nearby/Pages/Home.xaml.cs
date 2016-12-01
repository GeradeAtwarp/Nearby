using Nearby.Controls;
using Nearby.DependencyServices;
using Nearby.Helpers;
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

            BindingContext = vm = new HomeViewModel(Navigation);

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

            tbItemNavigateFav.Command = new Command(async () =>
            {
                var nav = Application.Current?.MainPage?.Navigation;
                if (nav == null)
                    return;

                if (vm.IsBusy)
                    return;

                await Navigation.PushAsync(new Favourites());
            });

            AddSearchButtons();
            
            MoveToCurrentLocation();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.UpdateItems();
            MoveToCurrentLocation();
        }

        async Task MoveToCurrentLocation()
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
                    Application.Current?.MainPage.DisplayAlert("Location", "Please set a custom location, Or turn off the custom location option on the settings page.", "Got it");
                else
                {
                    position = new Plugin.Geolocator.Abstractions.Position
                    {
                        Latitude = Convert.ToDouble(Settings.Current.CustomLatitude),
                        Longitude = Convert.ToDouble(Settings.Current.CustomLongitude)
                    };

                    var pin = new Pin
                    {
                        Type = PinType.Place,
                        Label = "This is you!",
                        Position = new Position(position.Latitude, position.Longitude)
                    };

                    placesMap.Pins.Add(pin);
                    placesMap.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMiles(0.5)));
                }
            }
        }

        async Task SearchForPlacesNearby()
        {
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
                        await Navigation.PushAsync(new PlaceDetailView(pn));
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
                await Task.Delay(300);
                await RefineSearchMenu.ScaleTo(1, 250, Easing.SinIn);
            }
            else
            {
                //fabrefine.ColorNormal = Color.FromHex("#7885cb");
                await Task.Delay(300);
                await RefineSearchMenu.ScaleTo(0, 250, Easing.SinOut);
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
                    ImageName = "search",
                    ColorNormal = Color.FromHex("#3F51B5"),
                    ColorPressed = Color.FromHex("#7885cb"),
                    ColorRipple = Color.FromHex("#2C3E50"),
                    Clicked = (sender, ea) => SearchForPlacesNearby()
                });

                RefineButton.Children.Add(new FloatingActionButtonView
                {
                    Size = FloatingActionButtonSize.Mini,
                    ImageName = "fab_refine",
                    ColorNormal = Color.FromHex("#3F51B5"),
                    ColorPressed = Color.FromHex("#7885cb"),
                    ColorRipple = Color.FromHex("#2C3E50"),
                    Clicked = (sender, e) => ToggleRefineOptions()
                });
            }
            else
            {
                Button btnSearch = new Button
                {
                    BorderRadius = 5,
                    BorderColor = Color.FromHex("#3F51B5"),
                    BorderWidth = 2,
                    WidthRequest = 100,
                    BackgroundColor = Color.Transparent,
                    TextColor = Color.FromHex("#3F51B5"),
                    Text = "Search",
                    FontAttributes = FontAttributes.Bold
                };

                btnSearch.Clicked += (sender, ea) => SearchForPlacesNearby();

                Button btnRefineSearch = new Button
                {
                    BorderRadius = 5,
                    BorderColor = Color.FromHex("#3F51B5"),
                    BorderWidth = 2,
                    WidthRequest = 100,
                    BackgroundColor = Color.Transparent,
                    TextColor = Color.FromHex("#3F51B5"),
                    Text = "Filter",
                    FontAttributes = FontAttributes.Bold
                };

                btnRefineSearch.Clicked += (sender, e) => ToggleRefineOptions();

                SearchButton.Children.Add(btnSearch);
                RefineButton.Children.Add(btnRefineSearch);
            }
        }

        void SetFilter(object sender, EventArgs args)
        {
            var image = (Image)sender;
            var source = image.Source as FileImageSource;

            if (source != null)
            {
                switch(source.File)
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
                    case "cafe":
                        vm.SetActiveFilter.Execute("cafe");
                        break;
                }

                ToggleRefineOptions();
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
