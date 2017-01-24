using Nearby.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public EventDetailViewModel(EventNearbyItem currentEvent)
        {
            EventDetails = currentEvent;

            EventActionItems.AddRange(new string[]
            {
                "Open In Maps",
                "View In Browser"
            });

            EventShareMessage = $"Join me @ {EventDetails.Title} in {EventDetails.CityName} on {DateTime.Parse(EventDetails.StartTime).ToString("ddd, MMM dd")}";
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
                    Device.OpenUri(new Uri("http://maps.apple.com/?daddr=" + _eventDetails.Latitude.Replace(",", ".") + "," + _eventDetails.Latitude.Replace(",", ".")));
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
            if (_eventDetails != null)
            {
                new Plugin.Calendars.Abstractions.CalendarEvent
                {
                    Description = _eventDetails.Description,
                    Location = _eventDetails.VenueName,
                    AllDay = true,
                    Name = _eventDetails.Title,
                    Start = Convert.ToDateTime(_eventDetails.StartTime),
                    End = Convert.ToDateTime(_eventDetails.StopTime)
                };
            }
            else
                return;
        }

        #endregion
    }
}
