using MvvmHelpers;
using Nearby.Interfaces;
using Nearby.Models;
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

        private ObservableCollection<EventNearbyItem> _eventsNearby = new ObservableCollection<EventNearbyItem>();
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

        public ICommand SetReminder => new Command(SetReminderCommand);
        private async void SetReminderCommand(object e)
        {
            var nearbyEvent = e as EventNearbyItem;

            if (nearbyEvent != null)
            {
                var reminderService = DependencyService.Get<IReminderService>();

                reminderService.AddEvent(Convert.ToDateTime(nearbyEvent.StartTime), Convert.ToDateTime(nearbyEvent.StopTime), nearbyEvent.Title, nearbyEvent.VenueName, string.Empty, (success) =>
                {
                    Application.Current?.MainPage.DisplayAlert("Reminder", "Reminder was successfully set.", "Ok");
                }, "4242016400");

                //new Plugin.Calendars.Abstractions.CalendarEvent
                //{
                //    Description = nearbyEvent.Description,
                //    Location = nearbyEvent.VenueName,
                //    AllDay = true,
                //    Name = nearbyEvent.Title,
                //    Start = Convert.ToDateTime(nearbyEvent.StartTime),
                //    End = Convert.ToDateTime(nearbyEvent.StopTime)
                //};

                
            }
            else
                return;
        }

        #endregion


        public EventsViewModel(INavigation navigation) : base(navigation)
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

                //Get the users current location
                position = await UpdateCurrentLocation();

                //Get the weather for the users location
                GetCurrentWeather();

                var httpClient = new HttpClient();
                var eventsResults = "";

                eventsResults = await httpClient.GetStringAsync(new UriBuilder("http://api.eventful.com/json/events/search?app_key=hTRdwhLvk8LgjnFL&within=30&page_size=30&sort_order=popularity&include=categories&location=" + position.Latitude.ToString().Replace(',', '.') + "," + position.Longitude.ToString().Replace(',', '.')).Uri.ToString());

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
                        Description = re.description,
                        Latitude = re.latitude,
                        Longitude = re.longitude,
                        StartTime = re.start_time,
                        StopTime = re.stop_time,
                        VenueName = re.venue_name,
                        EventURL = re.venue_url,
                        SetReminder = SetReminder,
                        EventImage = (re.image != null ? ImageSource.FromUri(new Uri(re.image.url)) : ImageSource.FromUri(new Uri("https://raw.githubusercontent.com/Microsoft/BikeSharing360_MobileApps/master/src/CommonResources/suggestion_bronx_river.png"))),
                        Categories = String.Join(String.Empty, (re.categories != null ? re.categories.category.Select(x => x.name).ToList() : new List<string>()))
                    });
                }

                EventsNearby = new ObservableCollection<EventNearbyItem>(eventsList);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task GetCurrentWeather()
        {
            try
            {
                var httpClient = new HttpClient();
                var weatherResults = "";

                weatherResults = await httpClient.GetStringAsync(new UriBuilder("http://api.openweathermap.org/data/2.5/weather?APPID=b806514997d45be8cf0f0000935b48e6&lat=" + position.Latitude.ToString().Replace(',', '.') + "&lon=" + position.Longitude.ToString().Replace(',', '.') + "&units=metric").Uri.ToString());

                var response = JsonConvert.DeserializeObject<WatherResult>(weatherResults);

                Temp = String.Format("{0}°C",response.main.temp.ToString());
                WeatherDesc = response.weather[0].description;
                TempHiLow = String.Format("{0}° / {1}°", response.main.temp_min, response.main.temp_max);
                WeatherLocation = response.name;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
    }
}
