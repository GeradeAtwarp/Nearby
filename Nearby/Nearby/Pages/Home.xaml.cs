using Nearby.DependencyServices;
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

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        async Task SearchForPlacesNearby()
        {
            try
            {
                if (IsBusy)
                    return;
                
                await btnSearchPlaces.ScaleTo(1.2, 100);
                await btnSearchPlaces.ScaleTo(1, 100);

                var httpClient = new HttpClient();

                //Get all the places neaby
                var placesResult = await httpClient.GetStringAsync(new UriBuilder("https://maps.googleapis.com/maps/api/place/nearbysearch/json?key=AIzaSyDU4ZSeEmjTiTgT2CJgj7bZegShjj_rV7M&location=-25.766468999999997,28.2998734&radius=500&type=restaurant").Uri.ToString());

                var placesNearby = JsonConvert.DeserializeObject<PlaceNearby>(placesResult);
                List<Pin> ps = new List<Pin>();

                foreach (var pn in placesNearby.results.ToList())
                {
                    var newposition = new Xamarin.Forms.Maps.Position(pn.geometry.location.lat, pn.geometry.location.lng);

                    var pin = new Pin
                    {
                        Type = PinType.Place,
                        Position = newposition,
                        Label = pn.name,
                        Address = pn.vicinity
                    };

                    placesMap.Pins.Add(pin);
                    placesMap.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromKilometers(1.0)));
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
