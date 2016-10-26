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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.UpdateItems();
        }
    }
}
