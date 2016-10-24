using Nearby.Controls;
using Nearby.Helpers;
using Nearby.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Nearby.Pages
{
    public partial class Favourites : ContentPage
    {
        FavoritesViewModel ViewModel => vm ?? (vm = BindingContext as FavoritesViewModel);
        FavoritesViewModel vm;

        public Favourites()
        {
            InitializeComponent();

            Title = "Favourites";

            BindingContext = vm = new FavoritesViewModel(Navigation);

            //tbBackToMap.Command = new Command(async () =>
            //{
            //    await Navigation.PopModalAsync();
            //});

            lstFavorites.ItemSelected += async (sender, e) =>
            {
                lstFavorites.SelectedItem = null;
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
