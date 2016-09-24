using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MvvmHelpers;
using System.Windows.Input;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms.Maps;
using Nearby.Pages;

namespace Nearby.viewModel
{
    public class HomeViewModel : NearbyBaseViewModel
    {
        public ObservableRangeCollection<Pin> PlacesNearby { get; } = new ObservableRangeCollection<Pin>();

        public HomeViewModel(INavigation navigation) : base(navigation)
        {

        }

        ICommand searchPlacesNearby;
        public ICommand SearchPlacesNearby => searchPlacesNearby ?? (searchPlacesNearby = new Command<string>(async (param) => await SearchNearby(param)));

        public async Task SearchNearby(string searctTerm)
        {
            try
            {
                if (IsBusy)
                    return;

                var httpClient = new HttpClient();

                //Get all the places neaby
                var placesResult = await httpClient.GetStringAsync(new UriBuilder("https://maps.googleapis.com/maps/api/place/nearbysearch/json?key=AIzaSyDU4ZSeEmjTiTgT2CJgj7bZegShjj_rV7M&location=-25.766468999999997,28.2998734&radius=500&type=restaurant").Uri.ToString());

                var placesNearby = JsonConvert.DeserializeObject<PlaceNearby>(placesResult);

                foreach (var pn in placesNearby.results.ToList())
                {
                    Pin p = new Pin();
                    p.Position = new Position(pn.geometry.location.lat, pn.geometry.location.lng);
                    p.Label = pn.name;
                    p.Address = pn.vicinity;

                    PlacesNearby.Add(p);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }


    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Viewport
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
        public Viewport viewport { get; set; }
    }

    public class OpeningHours
    {
        public bool open_now { get; set; }
        public List<object> weekday_text { get; set; }
    }

    public class Photo
    {
        public int height { get; set; }
        public List<string> html_attributions { get; set; }
        public string photo_reference { get; set; }
        public int width { get; set; }
    }

    public class Places
    {
        public Geometry geometry { get; set; }
        public string icon { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public OpeningHours opening_hours { get; set; }
        public List<Photo> photos { get; set; }
        public string place_id { get; set; }
        public string reference { get; set; }
        public string scope { get; set; }
        public List<string> types { get; set; }
        public string vicinity { get; set; }
        public int? price_level { get; set; }
    }

    public class PlaceNearby
    {
        public List<string> html_attributions { get; set; }
        public List<Places> results { get; set; }
        public string status { get; set; }
    }
}
