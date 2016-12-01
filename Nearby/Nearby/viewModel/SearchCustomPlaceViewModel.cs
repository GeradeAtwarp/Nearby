using MvvmHelpers;
using Nearby.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static Nearby.viewModel.PlaceDetailViewModel;

namespace Nearby.viewModel
{
    public class SearchCustomPlaceViewModel : NearbyBaseViewModel
    {
        public ObservableRangeCollection<CustomPlaceSearch> SearchResults { get; } = new ObservableRangeCollection<CustomPlaceSearch>();

        public SearchCustomPlaceViewModel(INavigation navigation) :base(navigation)
        {

        }

        string filter = string.Empty;
        public string Filter
        {
            get { return filter; }
            set { SetProperty(ref filter, value); }
        }

        bool hasNoResults = false;
        public bool HasNoResults
        {
            get { return hasNoResults; }
            set { SetProperty(ref hasNoResults, value); }
        }


        ICommand searchPlacesCommand;
        public ICommand SearchPlacesCommand => searchPlacesCommand ?? (searchPlacesCommand = new Command(async () => await SearchForCustomLocation()));

        async Task SearchForCustomLocation()
        {
            try
            {
                HasNoResults = false;

                if (Filter != String.Empty)
                {
                    if (IsBusy)
                        return;

                    IsBusy = true;

                    var httpClient = new HttpClient();

                    var placesResult = "";
                    placesResult = await httpClient.GetStringAsync(new UriBuilder("https://maps.googleapis.com/maps/api/place/textsearch/json?query=" + Filter + "&key=AIzaSyAg-d-wLhMl65Fo_sfyj_U9tFOoW41UcDQ").Uri.ToString());

                    SearchPlaces places = JsonConvert.DeserializeObject<SearchPlaces>(placesResult);

                    SearchResults.Clear();

                    foreach (SearchPlace place in places.results)
                    {
                        CustomPlaceSearch sp = new CustomPlaceSearch();

                        sp.Name = place.name;
                        sp.PlaceId = place.place_id;

                        if (place.types != null)
                            sp.Tags = place.types[0].Replace("_", " ");

                        sp.Address = place.formatted_address;

                        sp.CoOrdinatesLat = place.geometry.location.lat;
                        sp.CoOrdinatesLng = place.geometry.location.lng;

                        SearchResults.Add(sp);
                    }
                    

                    if (SearchResults.Count() == 0)
                        HasNoResults = true;
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task SetPlaceAsCustomLocation(CustomPlaceSearch place)
        {
            try
            {
                Settings.CustomLatitude = place.CoOrdinatesLat.ToString();
                Settings.CustomLongitude = place.CoOrdinatesLng.ToString();
                Settings.CustomLocation = place.Name;

                Application.Current?.MainPage?.DisplayAlert("Info", "Custom location was successfully set to " + place.Name, "Ok");
                    
            }
            catch (Exception ex)
            { }
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

        public class SearchPlace
        {
            public string formatted_address { get; set; }
            public Geometry geometry { get; set; }
            public string icon { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public OpeningHours opening_hours { get; set; }
            public List<Photo> photos { get; set; }
            public string place_id { get; set; }
            public int price_level { get; set; }
            public string reference { get; set; }
            public List<string> types { get; set; }
            public double? rating { get; set; }
        }

        public class SearchPlaces
        {
            public List<object> html_attributions { get; set; }
            public List<SearchPlace> results { get; set; }
            public string status { get; set; }
        }
    }
}
