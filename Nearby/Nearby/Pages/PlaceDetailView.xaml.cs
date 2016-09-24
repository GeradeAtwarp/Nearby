using Nearby.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Nearby.Pages
{
    public partial class PlaceDetailView : ContentPage
    {
        PlaceDetailViewModel vm;

        public PlaceDetailView(Pin place)
        {
            InitializeComponent();

            BindingContext = vm = new PlaceDetailViewModel(Navigation, place);
        }
    }
}
