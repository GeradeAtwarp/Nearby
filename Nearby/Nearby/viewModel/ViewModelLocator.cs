using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearby.viewModel
{
    public static class ViewModelLocator
    {
        private static EventsViewModel _myEventsViewModel = new EventsViewModel();
        public static EventsViewModel MainventsViewModel
        {
            get
            {
                return _myEventsViewModel;
            }
        }


    }
}
