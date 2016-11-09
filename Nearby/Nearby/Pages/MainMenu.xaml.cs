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
    public partial class MainMenu : ContentPage
    {
        MainMenuViewModel ViewModel => vm ?? (vm = BindingContext as MainMenuViewModel);
        MainMenuViewModel vm;

        public MainMenu()
        {
            InitializeComponent();

            BindingContext = new MainMenuViewModel(Navigation);

            //ListAccountProviderss.ItemSelected += (s, e) =>
            //{
            //    ListAccountProviderss.SelectedItem = null;

            //};
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.UpdateItems();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            vm = null;

            //var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.AccountItems.Count + 1;
            //ListAccountProviderss.HeightRequest = (ViewModel.AccountItems.Count * ListAccountProviderss.RowHeight) - adjust;
        }
    }
}
