using MvvmHelpers;
using Nearby.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                SetProperty(ref _eventsNearby, value);
            }
        }

        Plugin.Geolocator.Abstractions.Position position;

        bool hasEvents = false;
        public bool HasEvents
        {
            get { return hasEvents; }
            set { SetProperty(ref hasEvents, value); }
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
                new Plugin.Calendars.Abstractions.CalendarEvent
                {
                    Description = nearbyEvent.Description,
                    Location = nearbyEvent.VenueName,
                    AllDay = true,
                    Name = nearbyEvent.Title,
                    Start = Convert.ToDateTime(nearbyEvent.StartTime),
                    End = Convert.ToDateTime(nearbyEvent.StopTime)
                };

                Application.Current?.MainPage.DisplayAlert("Reminder", "Reminder was successfully set.", "Ok");
            }
            else
                return;
        }

        #endregion


        public EventsViewModel(INavigation navigation) : base(navigation)
        {
            LoadEventsNearby();
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

                eventsResults = await httpClient.GetStringAsync(new UriBuilder("http://api.eventful.com/json/events/search?app_key=hTRdwhLvk8LgjnFL&within=30&page_size=30&sort_order=date&include=categories&location=" + position.Latitude.ToString().Replace(',', '.') + "," + position.Longitude.ToString().Replace(',', '.')).Uri.ToString());

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
            catch (Exception ex) {
                IsBusy = false;
            }
            finally{
                IsBusy = false;
            }
        }

    }   
}
