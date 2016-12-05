using Nearby.Controls;
using Nearby.Helpers;
using Nearby.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using static Nearby.viewModel.FavoritesViewModel;

namespace Nearby.Pages
{
    public partial class Favourites : ContentPage
    {
        FavoritesViewModel ViewModel => vm ?? (vm = BindingContext as FavoritesViewModel);
        FavoritesViewModel vm;

        public Favourites()
        {
            InitializeComponent();
            BindingContext = vm = new FavoritesViewModel(Navigation);

            lstFavorites.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                FavPlaceItem fi = e.SelectedItem as FavPlaceItem;

                lstFavorites.SelectedItem = null;
                vm.GoToDetailsCommand.Execute(fi);
            };
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            vm = null;

            var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.FavPlaces.Count + 1;
            lstFavorites.HeightRequest = (ViewModel.FavPlaces.Count * lstFavorites.RowHeight) - adjust;
        }
    }
}
