using Nearby.Controls;
using Nearby.Helpers;
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
        public Favourites()
        {
            InitializeComponent();

            Title = "Favourites";

            btnAddPlacesToFav.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new Home());
            };
        }
    }
}
