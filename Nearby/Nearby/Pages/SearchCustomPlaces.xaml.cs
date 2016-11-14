using Nearby.Helpers;
using Nearby.Models;
using Nearby.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Nearby.Pages
{
    public partial class SearchCustomPlaces : ContentPage
    {
        SearchCustomPlaceViewModel ViewModel => vm ?? (vm = BindingContext as SearchCustomPlaceViewModel);
        SearchCustomPlaceViewModel vm;

        public SearchCustomPlaces()
        {
            InitializeComponent();

            if (Device.OS == TargetPlatform.iOS)
                NavigationPage.SetBackButtonTitle(this, "");

            BindingContext = new SearchCustomPlaceViewModel(Navigation);

            lstSearch.ItemSelected += LstSearch_ItemSelected;
        }

        private void LstSearch_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedPlace = e.SelectedItem as CustomPlaceSearch;

            if(selectedPlace != null)
            {
                ViewModel.SetPlaceAsCustomLocation(selectedPlace);
            }

            lstSearch.SelectedItem = null;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            vm = null;

            var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.SearchResults.Count + 1;
            lstSearch.HeightRequest = (ViewModel.SearchResults.Count * lstSearch.RowHeight) - adjust;
        }
    }
}
