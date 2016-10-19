using MvvmHelpers;
using Nearby.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                await Task.Delay(3000);

                FavPlaces.AddRange((from fp in NearbyDataContext.GetItems<FavoritePlaces>()
                                    select new FavPlaceItem
                                    {
                                        Name = fp.PlaceName,
                                        PlaceID = fp.PlaceId,
                                        SavedOn = "Saved on " + fp.Created.ToString("yyyy/MM/dd")
                                    }).ToList());

                if (FavPlaces.Count() == 0)
                    HasFavorites = true;
            }
            catch(Exception ex)
            {
                IsBusy = false;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    public class FavPlaceItem
    {
        public string Name { get; set; }
        public string PlaceID { get; set; }
        public string SavedOn { get; set; }
    }


}
