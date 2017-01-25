using Acr.UserDialogs;
using MvvmHelpers;
using Nearby.Interfaces;
using Nearby.Models;
using Nearby.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        #region Properties

        public ObservableCollection<EventNearbyItem> _eventsNearby = new ObservableCollection<EventNearbyItem>();
        public ObservableCollection<EventNearbyItem> EventsNearby
        {
            get
            {
                return _eventsNearby;
            }

            set
            {
                _eventsNearby = value;
                RaisePropertyChanged(() => EventsNearby);
            }
        }

        Plugin.Geolocator.Abstractions.Position position;

        bool hasEvents = false;
        public bool HasEvents
        {
            get { return hasEvents; }
            set { SetProperty(ref hasEvents, value); }
        }

        string currentDate = "";
        public string CurrentDate
        {
            get { return currentDate; }
            set { SetProperty(ref currentDate, value); }
        }

        string temp = "";
        public string Temp
        {
            get { return temp; }
            set { SetProperty(ref temp, value); }
        }

        string weatherDesc = "";
        public string WeatherDesc
        {
            get { return weatherDesc; }
            set { SetProperty(ref weatherDesc, value); }
        }

        string tempHiLow = "";
        public string TempHiLow
        {
            get { return tempHiLow; }
            set { SetProperty(ref tempHiLow, value); }
        }

        string weatherLocation = "";
        public string WeatherLocation
        {
            get { return weatherLocation; }
            set { SetProperty(ref weatherLocation, value); }
        }

        #endregion


        #region Commands

        public ICommand RefreshCommand => new Command(RefreshEventsCommand);
        private async void RefreshEventsCommand(object obj)
        {
            await LoadEventsNearby();
        }

        public ICommand NavigateToDetails => new Command<EventNearbyItem>(ShowEventAsync);



        private async void ShowEventAsync(EventNearbyItem @event)
        {
            if (@event != null)
            {
                await Navigation.PushAsync(new EventDetail(@event));
            }
        }


        #endregion


        public EventsViewModel() : base()
        {
            CurrentDate = DateTime.Now.ToString("dddd, dd MMM yyyy");
        }

        async Task LoadEventsNearby()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                UserDialogs.Instance.ShowLoading("Loading Events...", MaskType.Clear);

                //Get the users current location
                position = await UpdateCurrentLocation();

                //Get the weather for the users location
                GetCurrentWeather();

                var httpClient = new HttpClient();
                var eventsResults = "";

                eventsResults = await httpClient.GetStringAsync(new UriBuilder("http://api.eventful.com/json/events/search?app_key="+ GlobalKeys.EventfulAPIKey +"&category=" + GlobalKeys.EventSearchCriteria + "&within=30&page_size=30&sort_order=popularity&sort_direction=ascending&include=categories&location=" + position.Latitude.ToString().Replace(',', '.') + "," + position.Longitude.ToString().Replace(',', '.')).Uri.ToString());

                var response = JsonConvert.DeserializeObject<EventNearbyResult>(eventsResults).events.@event;

                List<EventNearbyItem> eventsList = new List<EventNearbyItem>();

                EventsNearby.Clear();

                foreach (var re in response)
                {
                    eventsList.Add(new EventNearbyItem
                    {
                        EventId = re.id,
                        Title = re.title,
                        CityName = re.city_name,
                        Description = (!string.IsNullOrEmpty(re.description) ? re.description : "Description Not Available"),
                        Latitude = re.latitude,
                        Longitude = re.longitude,
                        StartTime = re.start_time,
                        StopTime = re.stop_time,
                        VenueName = re.venue_name,
                        EventURL = re.venue_url,
                        Categories = String.Join(String.Empty, (re.categories != null ? re.categories.category.Select(x => x.id).ToList() : new List<string>()))
                    });
                }

                EventsNearby = new ObservableCollection<EventNearbyItem>(eventsList);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine(ex.Message);
                UserDialogs.Instance.HideLoading();
            }
            finally
            {
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
            }
        }

        async Task GetCurrentWeather()
        {
            try
            {
                var httpClient = new HttpClient();
                var weatherResults = "";

                weatherResults = await httpClient.GetStringAsync(new UriBuilder("http://api.openweathermap.org/data/2.5/weather?APPID="+ GlobalKeys.OpenWeatherAPIKey +"&lat=" + position.Latitude.ToString().Replace(',', '.') + "&lon=" + position.Longitude.ToString().Replace(',', '.') + "&units=metric").Uri.ToString());

                var response = JsonConvert.DeserializeObject<WatherResult>(weatherResults);

                Temp = String.Format("{0}° C", (int)response.main.temp);
                WeatherDesc = response.weather[0].description;
                TempHiLow = String.Format("{0}° / {1}°", (int)response.main.temp_min, (int)response.main.temp_max);
                WeatherLocation = response.name;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
    }
}
