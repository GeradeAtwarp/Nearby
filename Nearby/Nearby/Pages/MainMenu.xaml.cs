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

            ListViewFeeds.ItemSelected += async (s, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var item = e.SelectedItem as viewModel.MenuItem;
                Page page = new Favourites();
                
                if (!item.isSwitch)
                {
                    await NavigationService.PushAsync(Navigation, page);
                }

                ListViewFeeds.SelectedItem = null;
            };

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

    public class MainMenuItemSelector : DataTemplateSelector
    {
        public DataTemplate SwitchCellDataTemplate { get; set; }
        public DataTemplate TextCellDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((viewModel.MenuItem)item).isSwitch ? SwitchCellDataTemplate : TextCellDataTemplate;
        }
    }
}
