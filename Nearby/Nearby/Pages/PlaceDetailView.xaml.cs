﻿using Nearby.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using static Nearby.viewModel.PlaceDetailViewModel;

namespace Nearby.Pages
{
    public partial class PlaceDetailView : ContentPage
    {
        PlaceDetailViewModel ViewModel => vm ?? (vm = BindingContext as PlaceDetailViewModel);
        PlaceDetailViewModel vm;

        public PlaceDetailView(Places place)
        {
            InitializeComponent();

            BindingContext = vm = new PlaceDetailViewModel(place);

            if (place != null)
            {
                var newposition = new Xamarin.Forms.Maps.Position(place.geometry.location.lat, place.geometry.location.lng);

                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = newposition,
                    Label = place.name,
                    Address = place.vicinity
                };

                placeDetailMap.Pins.Add(pin);
                placeDetailMap.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(500)));

                tbItemFav.Clicked += async (sender, e) => { vm.SaveFavourite.Execute(null); };
            }

            ListPlaceDetails.ItemSelected += async (sender, e) =>
            {
                ListPlaceDetails.SelectedItem = null;
            };

            ListPlaceContactDetails.ItemSelected += async (sender, e) =>
            {
                PlceDetailItem item = e.SelectedItem as PlceDetailItem;

                if (item == null)
                    return;

                item.Command.Execute(item.CommandParameter);

                ListPlaceContactDetails.SelectedItem = null;
            };

            placeDetailMap.IsEnabled = false;

            EaseIn();
        }

        async Task EaseIn()
        {
            await Task.Delay(300);

            await Actionbuttons.ScaleTo(1, 250, Easing.SinIn);
            await PlaceReviews.ScaleTo(1, 250, Easing.SinIn);
            await PlaceOperating.ScaleTo(1, 250, Easing.SinIn);
            await PlaceContact.ScaleTo(1, 250, Easing.SinIn);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            vm = null;

            var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.PlaceOperatingHours.Count + 1;
            ListPlaceDetails.HeightRequest = (ViewModel.PlaceOperatingHours.Count * ListPlaceDetails.RowHeight) - adjust;

            adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.PlaceContactDetails.Count + 1;
            ListPlaceContactDetails.HeightRequest = (ViewModel.PlaceContactDetails.Count * ListPlaceContactDetails.RowHeight) - adjust;
        }
    }
}
