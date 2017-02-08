using Microsoft.Azure.Mobile.Analytics;
using Nearby.Models;
using Nearby.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
    public class EventDetailViewModel : NearbyBaseViewModel
    {
        protected EventNearbyItem _eventDetails;
        public EventNearbyItem EventDetails
        {
            get { return _eventDetails; }
            set { SetProperty(ref _eventDetails, value); }
        }

        public List<String> EventActionItems { get; } = new List<String>();

        string eventShareMessage = "";
        public string EventShareMessage
        {
            get { return eventShareMessage; }
            set { SetProperty(ref eventShareMessage, value); }
        }

        public bool ShowReminder { get; set; }

        bool isReminderSet;
        public bool IsReminderSet
        {
            get { return isReminderSet; }
            set { SetProperty(ref isReminderSet, value); }
        }

        string eventReminderText = "Set Reminder";
        public string EventReminderText
        {
            get { return eventReminderText; }
            set { SetProperty(ref eventReminderText, value); }
        }

        bool showForecast = false;
        public bool ShowForecast
        {
            get { return showForecast; }
            set { SetProperty(ref showForecast, value); }
        }

        string weatherForecastText = "Forecast Not Avalable";
        public string WeatherForecastText
        {
            get { return weatherForecastText; }
            set { SetProperty(ref weatherForecastText, value); }
        }


        Plugin.Geolocator.Abstractions.Position position;

        public EventDetailViewModel(EventNearbyItem currentEvent)
        {
            EventDetails = currentEvent;

            EventActionItems.AddRange(new string[]
            {
                "Open In Maps",
                "View In Browser"
            });

            EventShareMessage = $"Join me for {EventDetails.Title} in {EventDetails.CityName} on {DateTime.Parse(EventDetails.StartTime).ToString("ddd, MMM dd")} #NearbyPlacesEvents";

            SetWeatherForecast();

            HasReminderSetAsync();
        }

        public async Task HasReminderSetAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                IsReminderSet = await ReminderService.HasReminderAsync(EventDetails.EventId);

                if (!IsReminderSet)
                    EventReminderText = "Set Reminder";
                else
                    EventReminderText = "Remove Reminder";
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task SetWeatherForecast()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                //Only display forcast if event occurs within the next week
                if (DateTime.Parse(_eventDetails.StartTime) < DateTime.Now.AddDays(7))
                {
                    //Get the users current location
                    position = await UpdateCurrentLocation();

                    var httpClient = new HttpClient();
                    var forecastResponse = "";

                    forecastResponse = await httpClient.GetStringAsync(new UriBuilder("https://api.darksky.net/forecast/" + GlobalKeys.DarkSkyKey + "/" + position.Latitude.ToString().Replace(',', '.') + "," + position.Longitude.ToString().Replace(',', '.') + "?units=auto&exclude=" + GlobalKeys.DarkSkyExclusions).Uri.ToString());

                    var forecast = JsonConvert.DeserializeObject<WeatherForecastItem>(forecastResponse);

                    var dayForcast = forecast.daily.data.Where(x => x.time.Date == DateTime.Parse(_eventDetails.StartTime).Date).FirstOrDefault();

                    if (dayForcast != null)
                    {
                        ShowForecast = true;
                        WeatherForecastText = $"{dayForcast.summary}";
                    }
                }
            }
            catch (Exception ex) { ShowForecast = false; }
            finally { IsBusy = false; }

        }

        #region Commands

        public ICommand OpenInMapsCommand => new Command(OpenInMaps);
        private async void OpenInMaps(object e)
        {
            try
            {
                if (Device.OS == TargetPlatform.Android)
                    Device.OpenUri(new Uri("http://maps.google.com/?daddr=" + _eventDetails.Latitude + "," + _eventDetails.Longitude));
                else
                    Device.OpenUri(new Uri("http://maps.apple.com/?daddr=" + _eventDetails.Latitude.Replace(",", ".") + "," + _eventDetails.Longitude.Replace(",", ".")));

                Analytics.TrackEvent("View_Event_Map", new Dictionary<string, string> { { "Action", "User viewed event venue on the maps app." } });
            }
            catch (Exception ex)
            {

            }
        }

        public ICommand OpenInBrowserCommand => new Command(OpenEventInBrowser);
        private async void OpenEventInBrowser(object e)
        {
            try
            {
                LaunchBrowserCommand.Execute(_eventDetails.EventURL);
            }
            catch { }
        }
        
        public ICommand SetReminder => new Command(SetReminderCommand);
        private async void SetReminderCommand(object e)
        {
            if (!IsReminderSet)
            {
                if (_eventDetails != null)
                {
                    var result = await ReminderService.AddReminderAsync(_eventDetails.EventId,
                       new Plugin.Calendars.Abstractions.CalendarEvent
                       {
                           Description = _eventDetails.Description,
                           Location = _eventDetails.VenueName,
                           AllDay = true,
                           Name = _eventDetails.Title,
                           Start = Convert.ToDateTime(_eventDetails.StartTime),
                           End = Convert.ToDateTime(_eventDetails.StopTime)
                       });

                    if (!result)
                        return;

                    Analytics.TrackEvent("Track_Event", new Dictionary<string, string> { { "Action", "User added reminder for event." } });

                    HasReminderSetAsync();
                }
                else
                    return;
            }
            else
            {
                var result = await ReminderService.RemoveReminderAsync(EventDetails.EventId);
                if (!result)
                    return;

                Analytics.TrackEvent("UnTrack_Event", new Dictionary<string, string> { { "Action", "User removed reminder for event." } });
                HasReminderSetAsync();
            }
        }

        #endregion
    }
}
