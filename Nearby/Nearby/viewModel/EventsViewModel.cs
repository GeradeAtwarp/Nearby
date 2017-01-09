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

namespace Nearby.viewModel
{
    public class EventsViewModel : NearbyBaseViewModel
    {
        public ObservableRangeCollection<Event> EventsNearby { get; } = new ObservableRangeCollection<Event>();
        Plugin.Geolocator.Abstractions.Position position;

        bool hasEvents = false;
        public bool HasEvents
        {
            get { return hasEvents; }
            set { SetProperty(ref hasEvents, value); }
        }


        public EventsViewModel(INavigation navigation) : base(navigation)
        {
            LoadEventsNearby();
        }

        public ICommand RefreshCommand => new Command(RefreshEventsCommand);
        private async void RefreshEventsCommand(object obj)
        {
            await LoadEventsNearby();
        }



        async Task LoadEventsNearby()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                
                //Get the users current location
                position = await UpdateCurrentLocation();

                var httpClient = new HttpClient();
                var eventsResults = "";

                eventsResults = await httpClient.GetStringAsync(new UriBuilder("http://api.eventful.com/json/events/search?app_key=hTRdwhLvk8LgjnFL&within=30&location=" + position.Latitude.ToString().Replace(',', '.') + "," + position.Longitude.ToString().Replace(',', '.')).Uri.ToString());
                
                EventsNearby.Clear();
                EventsNearby.AddRange(JsonConvert.DeserializeObject<RootObject>(eventsResults).events.@event);
            }
            catch (Exception ex) {
                IsBusy = false;
            }
            finally{
                IsBusy = false;
            }
        }
    }













    public class Small
    {
        public string width { get; set; }
        public string url { get; set; }
        public string height { get; set; }
    }

    public class Medium
    {
        public string width { get; set; }
        public string url { get; set; }
        public string height { get; set; }
    }

    public class Thumb
    {
        public string width { get; set; }
        public string url { get; set; }
        public string height { get; set; }
    }

    public class Image
    {
        public Small small { get; set; }
        public string width { get; set; }
        public object caption { get; set; }
        public Medium medium { get; set; }
        public string url { get; set; }
        public Thumb thumb { get; set; }
        public string height { get; set; }
    }

    public class Event
    {
        public object watching_count { get; set; }
        public string olson_path { get; set; }
        public object calendar_count { get; set; }
        public object comment_count { get; set; }
        public string region_abbr { get; set; }
        public string postal_code { get; set; }
        public object going_count { get; set; }
        public string all_day { get; set; }
        public string latitude { get; set; }
        public object groups { get; set; }
        public string url { get; set; }
        public string id { get; set; }
        public string privacy { get; set; }
        public string city_name { get; set; }
        public object link_count { get; set; }
        public string longitude { get; set; }
        public string country_name { get; set; }
        public string country_abbr { get; set; }
        public string region_name { get; set; }
        public string start_time { get; set; }
        public object tz_id { get; set; }
        public string description { get; set; }
        public string modified { get; set; }
        public string venue_display { get; set; }
        public object tz_country { get; set; }
        public object performers { get; set; }
        public string title { get; set; }
        public string venue_address { get; set; }
        public string geocode_type { get; set; }
        public object tz_olson_path { get; set; }
        public string recur_string { get; set; }
        public object calendars { get; set; }
        public string owner { get; set; }
        public object going { get; set; }
        public string country_abbr2 { get; set; }
        public Image image { get; set; }
        public string created { get; set; }
        public string venue_id { get; set; }
        public object tz_city { get; set; }
        public string stop_time { get; set; }
        public string venue_name { get; set; }
        public string venue_url { get; set; }
        public string ImagePath = "https://raw.githubusercontent.com/Microsoft/BikeSharing360_MobileApps/master/src/CommonResources/suggestion_bronx_river.png";
    }

    public class Events
    {
        public List<Event> @event { get; set; }
    }

    public class RootObject
    {
        public object last_item { get; set; }
        public string total_items { get; set; }
        public object first_item { get; set; }
        public string page_number { get; set; }
        public string page_size { get; set; }
        public object page_items { get; set; }
        public string search_time { get; set; }
        public string page_count { get; set; }
        public Events events { get; set; }
    }
}
