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
        public ObservableRangeCollection<EventNearbyItem> EventsNearby { get; } = new ObservableRangeCollection<EventNearbyItem>();
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

        public ICommand SetReminder => new Command(SetReminderCommand);
        private async void SetReminderCommand(object e)
        {
            var nearbyEvent = e as EventNearbyItem;

            if (nearbyEvent != null)
            {
                Application.Current?.MainPage.DisplayAlert("Reminder", "Reminder was successfully set.", "Ok");
            }
            else
                return;
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

                eventsResults = await httpClient.GetStringAsync(new UriBuilder("http://api.eventful.com/json/events/search?app_key=hTRdwhLvk8LgjnFL&within=30&page_size=30&sort_order=date&location=" + position.Latitude.ToString().Replace(',', '.') + "," + position.Longitude.ToString().Replace(',', '.')).Uri.ToString());

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
                        SetReminder = SetReminder
                    });
                }

                EventsNearby.AddRange(eventsList);
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
