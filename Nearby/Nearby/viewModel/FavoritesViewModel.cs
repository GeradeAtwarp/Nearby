using FormsToolkit;
using MvvmHelpers;
using Nearby.Helpers;
using Nearby.Pages;
using Nearby.Utils.Entities;
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
            GetSavedFavorites();
        }


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
                                        Vicinity = fp.Vicinity
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
        }
    }
}
