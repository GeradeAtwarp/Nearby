using Nearby.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Nearby.Pages
{
    public partial class PlaceDetailView : ContentPage
    {
        PlaceDetailViewModel ViewModel => vm ?? (vm = BindingContext as PlaceDetailViewModel);
        PlaceDetailViewModel vm;

        public PlaceDetailView(Pin place)
        {
            InitializeComponent();

            BindingContext = vm = new PlaceDetailViewModel(Navigation, place);

            placeDetailMap.Pins.Add(place);
            placeDetailMap.MoveToRegion(MapSpan.FromCenterAndRadius(place.Position, Distance.FromMiles(0.5)));
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            vm = null;
            var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.PlaceDetails.Count + 1;
            ListPlaceDetails.HeightRequest = (ViewModel.PlaceDetails.Count * ListPlaceDetails.RowHeight) - adjust;
        }
    }
}
