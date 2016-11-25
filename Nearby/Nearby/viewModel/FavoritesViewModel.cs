using FormsToolkit;
using MvvmHelpers;
using Nearby.Helpers;
using Nearby.Pages;
using Nearby.Utils.Entities;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Nearby.viewModel
{
    public class FavoritesViewModel : NearbyBaseViewModel
    {
        public ObservableRangeCollection<FavPlaceItem> FavPlaces { get; } = new ObservableRangeCollection<FavPlaceItem>();

        bool hasFavorites = false;
        public bool HasFavorites
        {
            get { return hasFavorites; }
            set { SetProperty(ref hasFavorites, value); }
        }

        public FavoritesViewModel(INavigation navigation) : base(navigation)
        {
            UpdateCurrentLocation();

            GetSavedFavorites();
        }
        
        Plugin.Geolocator.Abstractions.Position position;

        async Task GetSavedFavorites()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                
                FavPlaces.Clear();
                FavPlaces.AddRange((from fp in NearbyDataContext.GetItems<FavoritePlaces>()
                                    select new FavPlaceItem
                                    {
                                        ID = fp.Id,
                                        Name = fp.PlaceName,
                                        PlaceID = fp.PlaceId,
                                        SavedOn = "Saved on " + fp.Created.ToString("yyyy/MM/dd"),
                                        ViewDetailsCommand = GoToDetailsCommand,
                                        RemoveCommand = DeleteFavCommand,
                                        Latitude = fp.Latitude,
                                        Longitude = fp.Longitude,
                                        Vicinity = fp.Vicinity,
                                        DistanceFromCurrentLocation = DistanceTo(position.Latitude, position.Longitude, fp.Latitude, fp.Longitude)
                                    }).ToList());


                if (FavPlaces.Count() == 0)
                    HasFavorites = true;
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

        async Task UpdateCurrentLocation()
        {
            if (!Settings.CustomLocationEnabled)
            {
                //Get the users current location
                var locator = CrossGeolocator.Current;
                position = await locator.GetPositionAsync(10000);
            }
            else
            {
                position = new Plugin.Geolocator.Abstractions.Position
                {
                    Latitude = Convert.ToDouble(Settings.CustomLatitude),
                    Longitude = Convert.ToDouble(Settings.CustomLongitude)
                };
            }

        }


            ICommand goToDetailsCommand;
        public ICommand GoToDetailsCommand =>
            goToDetailsCommand ?? (goToDetailsCommand = new Command<FavPlaceItem>(async (place) => await ViewPlaceDetails(place)));

        async Task ViewPlaceDetails(FavPlaceItem place)
        {
            Places favPlave = new Places();

            favPlave.name = place.Name;
            favPlave.place_id = place.PlaceID;
            favPlave.geometry.location.lat = place.Latitude;
            favPlave.geometry.location.lng = place.Longitude;
            favPlave.vicinity = place.Vicinity;

            await Navigation.PushAsync(new PlaceDetailView(favPlave));
        }



        ICommand deleteFavCommand;
        public ICommand DeleteFavCommand =>
            deleteFavCommand ?? (deleteFavCommand = new Command<FavPlaceItem>(async (place) => await RemoveFavorite(place)));

        async Task RemoveFavorite(FavPlaceItem place)
        {
            try
            {
                MessagingService.Current.SendMessage<MessagingServiceQuestion>(MessageKeys.Question, new MessagingServiceQuestion
                {
                    Negative = "No",
                    Positive = "Continue",
                    Question = "Are uou sure you want remove this favourite?",
                    Title = "Remove Favourite",
                    OnCompleted = (async (result) =>
                    {
                        if (!result)
                            return;

                        FavoritePlaces favToRemove = NearbyDataContext.GetItems<FavoritePlaces>().Where(x => x.Id == place.ID).FirstOrDefault();

                        if (favToRemove != null)
                        {
                            NearbyDataContext.RemoveItem<FavoritePlaces>(favToRemove);

                            FavPlaces.Clear();
                            FavPlaces.AddRange((from fp in NearbyDataContext.GetItems<FavoritePlaces>()
                                                select new FavPlaceItem
                                                {
                                                    ID = fp.Id,
                                                    Name = fp.PlaceName,
                                                    PlaceID = fp.PlaceId,
                                                    SavedOn = "Saved on " + fp.Created.ToString("yyyy/MM/dd"),
                                                    ViewDetailsCommand = GoToDetailsCommand,
                                                    RemoveCommand = DeleteFavCommand,
                                                    Latitude = fp.Latitude,
                                                    Longitude = fp.Longitude,
                                                    Vicinity = fp.Vicinity
                                                }).ToList());


                            if (FavPlaces.Count() == 0)
                                HasFavorites = true;
                        }
                    })
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occured trying to remove fav: " + ex.Message);
            }
        }


        public double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }












        public class FavPlaceItem
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string PlaceID { get; set; }
            public string SavedOn { get; set; }
            public ICommand ViewDetailsCommand { get; set; }
            public ICommand RemoveCommand { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Vicinity { get; set; }
            public double DistanceFromCurrentLocation { get; set; }
        }
    }
}
