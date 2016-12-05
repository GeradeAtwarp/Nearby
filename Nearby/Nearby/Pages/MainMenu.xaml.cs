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

            ListViewAbout.ItemSelected += async (s, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                AboutMenuItem item = e.SelectedItem as AboutMenuItem;
               
                ListViewAbout.SelectedItem = null;
                Page page = null;

                switch (item.Value)
                {
                    case "terms":
                        page = new TermsAndConditions();
                        break;
                    case "about":
                        page = new AboutApp();
                        break;
                }

                if (page == null)
                    return;

                await NavigationService.PushAsync(Navigation, page);
            };
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

            var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.AboutItems.Count + 1;
            ListViewAbout.HeightRequest = (ViewModel.AboutItems.Count * ListViewAbout.RowHeight) - adjust;
        }
    }
}
