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
            BindingContext = vm = new EventsViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel.EventsNearby.Count == 0)
                ViewModel.RefreshCommand.Execute(null);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            vm = null;
        }
    }
}
