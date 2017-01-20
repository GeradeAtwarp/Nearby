using Nearby.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Nearby.Pages
{
    public partial class NearbyEvents : ContentPage
    {
        EventsViewModel ViewModel => vm ?? (vm = BindingContext as EventsViewModel);
        EventsViewModel vm;

        public NearbyEvents()
        {
            InitializeComponent();

            BindingContext = vm = new EventsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (vm.EventsNearby.Count == 0)
                vm.RefreshCommand.Execute(null);
        }
    }
}
