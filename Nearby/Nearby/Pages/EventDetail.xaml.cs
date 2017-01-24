using Nearby.Models;
using Nearby.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Nearby.Pages
{
    public partial class EventDetail : ContentPage
    {
        EventDetailViewModel ViewModel => vm ?? (vm = BindingContext as EventDetailViewModel);
        EventDetailViewModel vm;

        public EventDetail(EventNearbyItem selectedEvent)
        {
            InitializeComponent();

            BindingContext = vm = new EventDetailViewModel(selectedEvent);

            listEventActions.ItemSelected += async (sender, e) =>
            {
                string item = e.SelectedItem as string;

                if (item == null)
                    return;

                if (item == "Open In Maps")
                    vm.OpenInMapsCommand.Execute(null);
                else
                    vm.OpenInBrowserCommand.Execute(null);

                listEventActions.SelectedItem = null;
            };
        }
    }
}
