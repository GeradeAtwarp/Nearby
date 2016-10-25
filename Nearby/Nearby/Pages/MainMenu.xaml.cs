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
        public MainMenu()
        {
            InitializeComponent();

            BindingContext = new MainMenuViewModel(Navigation);

            //tbItemNavigateMap.Command = new Command(async () =>
            //{
            //    await Navigation.PopModalAsync();
            //});

            btnAddCustomLocation.Clicked += BtnAddCustomLocation_Clicked;

            btnIOSsetCustomLocation.Clicked += BtnAddCustomLocation_Clicked;
        }

        private void BtnAddCustomLocation_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SearchCustomPlaces());
        }
    }
}
