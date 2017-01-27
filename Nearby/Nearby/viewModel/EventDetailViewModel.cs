using Nearby.Models;
using Nearby.Services;
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

        public EventDetailViewModel(EventNearbyItem currentEvent)
        {
            EventDetails = currentEvent;

            EventActionItems.AddRange(new string[]
            {
                "Open In Maps",
                "View In Browser"
            });

            EventShareMessage = $"Join me for {EventDetails.Title} in {EventDetails.CityName} on {DateTime.Parse(EventDetails.StartTime).ToString("ddd, MMM dd")}";

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

                HasReminderSetAsync();
            }
        }

        #endregion
    }
}
