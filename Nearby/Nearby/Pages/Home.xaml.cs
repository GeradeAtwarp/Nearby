using Nearby.DependencyServices;
using Nearby.Helpers;
using Nearby.viewModel;
using Newtonsoft.Json;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

            btnSearchPlaces.Clicked += (sender, ea) => SearchForPlacesNearby();

            MoveToCurrentLocation();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        async Task MoveToCurrentLocation()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(10000);

            var pin = new Pin
            {
                Type = PinType.Place,
                Label = "This is you!",
                Position = new Position (position.Latitude, position.Longitude )
            };

            placesMap.Pins.Add(pin);
            placesMap.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMiles(1)));
        }

        async Task SearchForPlacesNearby()
        {
            await vm.SearchNearby("");

            try
            {
                foreach (var pn in vm.PlacesNearby)
                {
                    var newposition = new Xamarin.Forms.Maps.Position(pn.Position.Latitude, pn.Position.Longitude);

                    var pin = new Pin
                    {
                        Type = PinType.Place,
                        Position = newposition,
                        Label = pn.Label,
                        Address = pn.Address
                    };

                    pin.Clicked += async (sender, e) =>
                    {
                        var nav = Application.Current?.MainPage?.Navigation;
                        if (nav == null)
                            return;

                        await NavigationService.PushModalAsync(nav, new NavigationPage(new PlaceDetailView(pn)));
                    };

                    placesMap.Pins.Add(pin);
                    placesMap.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromKilometers(0.5)));
                }
            }
            catch (Exception ex)
            {

            }
        }

        async Task PopSearchButton()
        {
            
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

        public class Locations
        {
            public double longitude { get; set; }
            public double latitude { get; set; }
            public string LocationName { get; set; }
            public string Description { get; set; }
        }
    }
}
