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

            //lstEvents.ItemSelected += (s, e) =>
            //{
            //    lstEvents.SelectedItem = null;
            //};
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (vm.EventsNearby.Count() == 0)
                vm.RefreshCommand.Execute(null);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            vm = null;

            //var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.EventsNearby.Count + 1;
            //lstEvents.HeightRequest = (ViewModel.EventsNearby.Count * lstEvents.RowHeight) - adjust;
        }
    }
}
