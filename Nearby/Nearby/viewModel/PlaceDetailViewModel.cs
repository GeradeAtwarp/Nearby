using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Nearby.viewModel
{
    public class PlaceDetailViewModel : NearbyBaseViewModel
    {
        public ObservableRangeCollection<PlceDetailItem> PlaceDetails { get; } = new ObservableRangeCollection<PlceDetailItem>();

        public PlaceDetailViewModel(INavigation navigation, Pin place) :base(navigation)
        {
            Title = place.Label;

            PlaceDetails.Add(new PlceDetailItem { PlaceDetailLabel = "Latitude", PlaceDetailValue = place.Position.Latitude.ToString() });
            PlaceDetails.Add(new PlceDetailItem { PlaceDetailLabel = "Longitude", PlaceDetailValue = place.Position.Longitude.ToString() });
        }






        #region Internal classes

        public class PlceDetailItem
        {
            public String PlaceDetailLabel { get; set; }
            public String PlaceDetailValue { get; set; }
        }

        #endregion
    }
}
