using MvvmHelpers;
using Nearby.Interfaces;
using Nearby.Utils.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Nearby.viewModel
{
    public class PlaceDetailViewModel : NearbyBaseViewModel
    {
        private Places Place;
        PlaceInfo Details;


        public ObservableRangeCollection<PlceDetailItem> PlaceContactDetails { get; } = new ObservableRangeCollection<PlceDetailItem>();
        public ObservableRangeCollection<PlceDetailItem> PlaceOperatingHours { get; } = new ObservableRangeCollection<PlceDetailItem>();

        public bool isOpen = false;
        public bool IsOpen
        {
            get { return isOpen; }
            set { SetProperty(ref isOpen, value); }
        }

        public bool hasOperatingHours = true;
        public bool HasNoOperatingHours
        {
            get { return hasOperatingHours; }
            set { SetProperty(ref hasOperatingHours, value); }
        }

        public bool hasNoContacts = false;
        public bool HasNoContacts
        {
            get { return hasNoContacts; }
            set { SetProperty(ref hasNoContacts, value); }
        }

        public bool hasContacts = false;
        public bool HasContacts
        {
            get { return hasContacts; }
            set { SetProperty(ref hasContacts, value); }
        }

        public ImageSource favImage = ImageSource.FromFile("heart_empty.png");
        public ImageSource FavImage
        {
            get { return favImage; }
            set { SetProperty(ref favImage, value); }
        }

        public int placeRating = 0;
        public int PlaceRating
        {
            get { return placeRating; }
            set { SetProperty(ref placeRating, value); }
        }

        public string reviews = "";
        public string Reviews
        {
            get { return reviews; }
            set { SetProperty(ref reviews, value); }
        }

        public PlaceDetailViewModel(INavigation navigation, Places place) : base(navigation)
        {
            Place = place;

            Title = place.name;
            
            GetDetails();
        }

        public async Task GetDetails()
        {
            try
            {
                if (IsBusy)
                    return;

                HasContacts = false;
                HasNoOperatingHours = false;
                HasNoContacts = false;

                IsBusy = true;

                var httpClient = new HttpClient();

                var placesResult = "";
                placesResult = await httpClient.GetStringAsync(new UriBuilder("https://maps.googleapis.com/maps/api/place/details/json?placeid=" + Place.place_id + "&key=AIzaSyAg-d-wLhMl65Fo_sfyj_U9tFOoW41UcDQ").Uri.ToString());

                Details = JsonConvert.DeserializeObject<PlaceInfo>(placesResult);
                List<PlceDetailItem> contacts = new List<PlceDetailItem>();

                if (!string.IsNullOrEmpty(Details.result.formatted_phone_number))
                    contacts.Add(new PlceDetailItem { PlaceDetailLabel = "Tel", PlaceDetailValue = Details.result.formatted_phone_number });

                if (!string.IsNullOrEmpty(Details.result.website))
                    contacts.Add(new PlceDetailItem { PlaceDetailLabel = "Website", PlaceDetailValue = Details.result.website });

                PlaceContactDetails.AddRange(contacts);

                if (Details.result.opening_hours != null)
                {
                    IsOpen = (Details.result.opening_hours.open_now ? true : false);
                    HasNoOperatingHours = (Details.result.opening_hours.periods.Count() > 0 ? false : true);
                }

                if (!string.IsNullOrEmpty(Details.result.formatted_phone_number) || !string.IsNullOrEmpty(Details.result.website))
                {
                    HasContacts = true;
                    HasNoContacts = false;
                }
                else
                {
                    HasContacts = false;
                    HasNoContacts = true;
                }

                #region Get operation hours

                foreach (Period p in Details.result.opening_hours.periods)
                {
                    switch (p.open.day)
                    {
                        case 1:
                            {
                                PlaceOperatingHours.Add(new PlceDetailItem { PlaceDetailLabel = "Monday", PlaceDetailValue = p.open.time + " - " + p.close.time });
                            }
                            break;
                        case 2:
                            {
                                PlaceOperatingHours.Add(new PlceDetailItem { PlaceDetailLabel = "Tuesday", PlaceDetailValue = p.open.time + " - " + p.close.time });
                            }
                            break;
                        case 3:
                            {
                                PlaceOperatingHours.Add(new PlceDetailItem { PlaceDetailLabel = "Wednestdau", PlaceDetailValue = p.open.time + " - " + p.close.time });
                            }
                            break;
                        case 4:
                            {
                                PlaceOperatingHours.Add(new PlceDetailItem { PlaceDetailLabel = "Thursday", PlaceDetailValue = p.open.time + " - " + p.close.time });
                            }
                            break;
                        case 5:
                            {
                                PlaceOperatingHours.Add(new PlceDetailItem { PlaceDetailLabel = "Friday", PlaceDetailValue = p.open.time + " - " + p.close.time });
                            }
                            break;
                        case 6:
                            {
                                PlaceOperatingHours.Add(new PlceDetailItem { PlaceDetailLabel = "Saturday", PlaceDetailValue = p.open.time + " - " + p.close.time });
                            }
                            break;
                    }
                }

                if (PlaceOperatingHours.Count() == 0)
                    HasNoOperatingHours = true;

                #endregion

                var fav = NearbyDataContext.GetItems<FavoritePlaces>().Where(x => x.PlaceId == Place.place_id).FirstOrDefault();
                if (fav != null)
                {
                    FavImage = ImageSource.FromFile("heart_filled.png");
                }

                PlaceRating = Details.result.reviews[0].aspects[0].rating;

                Reviews = "Based on " + Details.result.reviews.Count() + " reviews.";
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

        ICommand savefavourite;
        public ICommand SaveFavourite => savefavourite ?? (savefavourite = new Command(async () => await AddToFavourites()));

        async Task AddToFavourites()
        {
            try
            {
                var fav = NearbyDataContext.GetItems<FavoritePlaces>().Where(x => x.PlaceId == Place.place_id).FirstOrDefault();
                if (fav == null)
                {
                    NearbyDataContext.SaveItem<FavoritePlaces>(new FavoritePlaces { Created = DateTime.Now, PlaceId = Place.place_id, PlaceName = Details.result.name , Latitude = Place.geometry.location.lat, Longitude = Place.geometry.location.lng, Vicinity = Place.vicinity});
                    FavImage = ImageSource.FromFile("heart_filled.png");
                    Application.Current?.MainPage?.DisplayAlert("Favourite", Details.result.name + " was successfully added to you favourites.", "Ok");
                }
                else
                    Application.Current?.MainPage?.DisplayAlert("Info", "You have already saved " + Details.result.name, "Ok");
            }
            catch (Exception ex)
            {

            }
        }





        ICommand openNavigation;
        public ICommand OpenNavigation => openNavigation ?? (openNavigation = new Command(async () => await OpenNavigationToPlace()));

        async Task OpenNavigationToPlace()
        {
            try
            {
                if (Device.OS == TargetPlatform.Android)
                    Device.OpenUri(new Uri("http://maps.google.com/?daddr="+ Place.geometry.location.lat + "," + Place.geometry.location.lng));
                else
                    Device.OpenUri(new Uri("http://maps.apple.com/?daddr=" + Place.geometry.location.lat + "," + Place.geometry.location.lng));                  
            }
            catch (Exception ex)
            {

            }
        }


        ICommand openShare;
        public ICommand OpenShare => openShare ?? (openShare = new Command(async () => await DisplayShareOptions()));

        async Task DisplayShareOptions()
        {
            try
            {
                if (Device.OS == TargetPlatform.Android)
                    Device.OpenUri(new Uri("twitter://post?message=Guess what i am doing at " + Place.name + "? Come join me."));
                else
                {
                    var service = DependencyService.Get<IAppLauncher>();
                    service.SendTweet("Guess what i am doing at " + Place.name + "? Come join me.");
                }
            }
            catch (Exception ex)
            {

            }
        }


















        #region Internal classes

        public class PlceDetailItem
        {
            public String PlaceDetailLabel { get; set; }
            public String PlaceDetailValue { get; set; }
        }

        public class AddressComponent
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public List<string> types { get; set; }
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

        public class Close
        {
            public int day { get; set; }
            public string time { get; set; }
        }

        public class Open
        {
            public int day { get; set; }
            public string time { get; set; }
        }

        public class Period
        {
            public Close close { get; set; }
            public Open open { get; set; }
        }

        public class OpeningHours
        {
            public bool open_now { get; set; }
            public List<Period> periods { get; set; }
            public List<string> weekday_text { get; set; }
        }

        public class Photo
        {
            public int height { get; set; }
            public List<string> html_attributions { get; set; }
            public string photo_reference { get; set; }
            public int width { get; set; }
        }

        public class Aspect
        {
            public int rating { get; set; }
            public string type { get; set; }
        }

        public class Review
        {
            public List<Aspect> aspects { get; set; }
            public string author_name { get; set; }
            public string author_url { get; set; }
            public string language { get; set; }
            public string profile_photo_url { get; set; }
            public int rating { get; set; }
            public string text { get; set; }
            public int time { get; set; }
        }

        public class PlaceDetail
        {
            public List<AddressComponent> address_components { get; set; }
            public string adr_address { get; set; }
            public string formatted_address { get; set; }
            public string formatted_phone_number { get; set; }
            public Geometry geometry { get; set; }
            public string icon { get; set; }
            public string id { get; set; }
            public string international_phone_number { get; set; }
            public string name { get; set; }
            public OpeningHours opening_hours { get; set; }
            public List<Photo> photos { get; set; }
            public string place_id { get; set; }
            public string reference { get; set; }
            public List<Review> reviews { get; set; }
            public string scope { get; set; }
            public List<string> types { get; set; }
            public string url { get; set; }
            public int utc_offset { get; set; }
            public string vicinity { get; set; }
            public string website { get; set; }
        }

        public class PlaceInfo
        {
            public List<object> html_attributions { get; set; }
            public PlaceDetail result { get; set; }
            public string status { get; set; }
        }


        #endregion
    }
}
