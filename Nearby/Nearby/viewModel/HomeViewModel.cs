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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Nearby.Helpers;
using FormsToolkit;
using Nearby.Interfaces;
using Acr.UserDialogs;

namespace Nearby.viewModel
{
    public class HomeViewModel : NearbyBaseViewModel
    {
        string searcBurronText = "Search for places nearby";
        public string SearchButtonText
        {
            get { return searcBurronText; }
            set {
                SetProperty(ref searcBurronText, value);
            }
        }

        bool isFilterEnabled = false;
        public bool IsFilterEnabled
        {
            get { return isFilterEnabled; }
            set
            {
                SetProperty(ref isFilterEnabled, value);
                Settings.Current.IsSearchFilterEnabled = isFilterEnabled;
            }
        }

        string enabledFilter = "";
        public string EnabledFilter
        {
            get { return enabledFilter; }
            set
            {
                SetProperty(ref enabledFilter, value);
                Settings.Current.SearchFilters = enabledFilter;
            }
        }

        public ObservableRangeCollection<Places> PlacesNearby { get; } = new ObservableRangeCollection<Places>();

        public HomeViewModel()
        {
            IsFilterEnabled = Settings.Current.IsSearchFilterEnabled;
        }

        //Search for places nearby
        ICommand searchPlacesNearby;
        public ICommand SearchPlacesNearby => searchPlacesNearby ?? (searchPlacesNearby = new Command<string>(async (param) => await SearchNearby(param)));
        public async Task SearchNearby(string searctTerm)
        {
            try
            {
                if (Settings.Current.IsConnected)
                {
                    if (IsBusy)
                        return;

                    IsBusy = true;

                    UserDialogs.Instance.ShowLoading("Loading Places...", MaskType.Clear);

                    SearchButtonText = "Please wait...";

                    Plugin.Geolocator.Abstractions.Position position;

                    if (!Settings.Current.CustomLocationEnabled)
                    {
                        //Get the users current location
                        var locator = CrossGeolocator.Current;
                        position = await locator.GetPositionAsync(10000);
                    }
                    else
                    {
                        if (Settings.Current.CustomLatitude == "" || Settings.Current.CustomLongitude == "")
                        {
                            Application.Current?.MainPage.DisplayAlert("Location", "Please set a custom location, Or turn off the custom location option on the settings page.", "Got it");
                            return;
                        }
                        else
                        {
                            position = new Plugin.Geolocator.Abstractions.Position
                            {
                                Latitude = Convert.ToDouble(Settings.Current.CustomLatitude),
                                Longitude = Convert.ToDouble(Settings.Current.CustomLongitude)
                            };
                        }
                    }

                    string filter = "";
                    filter = Settings.Current.SearchFilters.ToLower().Replace(' ', '_');

                    var httpClient = new HttpClient();
                    var placesResult = "";

                    //Get all the places neaby
                    if (Device.OS == TargetPlatform.Android)
                        placesResult = await httpClient.GetStringAsync(new UriBuilder("https://maps.googleapis.com/maps/api/place/nearbysearch/json?key=AIzaSyDU4ZSeEmjTiTgT2CJgj7bZegShjj_rV7M&location=" + position.Latitude.ToString().Replace(',', '.') + "," + position.Longitude.ToString().Replace(',', '.') + "&radius=1500&type=" + filter).Uri.ToString());
                    else
                        placesResult = await httpClient.GetStringAsync(new UriBuilder("https://maps.googleapis.com/maps/api/place/nearbysearch/json?key=AIzaSyAg-d-wLhMl65Fo_sfyj_U9tFOoW41UcDQ&location=" + position.Latitude.ToString().Replace(',', '.') + "," + position.Longitude.ToString().Replace(',', '.') + "&radius=1500&type=" + filter).Uri.ToString());

                    if(JsonConvert.DeserializeObject<PlaceNearby>(placesResult).results.Count() == 0)
                    {
                        ShowToast("0 places were found. Maybe update you search criteria and try again.");
                    }

                    PlacesNearby.Clear();
                    PlacesNearby.AddRange(JsonConvert.DeserializeObject<PlaceNearby>(placesResult).results);
                }
                else
                {
                    MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert { Title = "Offline", Message = "Oh snap! you have gone offline. Please check your internet connection.", Cancel = "Ok" });
                }              
            }
            catch (Exception ex)
            {
                IsBusy = false;
                HockeyApp.MetricsManager.TrackEvent("An error Ocured getting places: " + ex.Message);
                UserDialogs.Instance.HideLoading();
            }
            finally
            {
                IsBusy = false;
                SearchButtonText = "Search for places nearby";
                UserDialogs.Instance.HideLoading();
            }
        }
        

        //set a filter for searching places nearby
        ICommand setActiveFilter;
        public ICommand SetActiveFilter => setActiveFilter ?? (setActiveFilter = new Command<string>(async (param) => await UpdateActiveFilter(param)));
        public async Task UpdateActiveFilter(string filter)
        {
            try
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    if (Settings.Current.SearchFilters == filter)
                        MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert { Title = "Already active", Message = "Filter has already been set to " + filter, Cancel = "Ok" });
                    else
                    {
                        Settings.Current.SearchFilters = filter;
                        IsFilterEnabled = true;
                    }
                }
                else
                {
                    Settings.Current.SearchFilters = "";
                    IsFilterEnabled = false;
                }
            }
            catch (Exception ex)
            {

            }
        }


        //remove filter for searching places nearby
        ICommand removeActiveFilter;
        public ICommand RemoveActiveFilter => removeActiveFilter ?? (removeActiveFilter = new Command(async () => await DeactivateFilter()));
        public async Task DeactivateFilter()
        {
            try
            {
                IsFilterEnabled = false;
                EnabledFilter = "";
            }
            catch (Exception ex)
            {
            }
        }

        public void UpdateItems()
        {
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

        public Geometry()
        {
            location = new Location();
        }
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

        public Places()
        {
            geometry = new Geometry();
        }
    }

    public class PlaceNearby
    {
        public List<string> html_attributions { get; set; }
        public List<Places> results { get; set; }
        public string status { get; set; }
    }
}
