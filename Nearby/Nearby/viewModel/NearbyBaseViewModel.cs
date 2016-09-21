using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nearby.viewModel
{
    public class NearbyBaseViewModel : BaseViewModel
    {
        protected INavigation Navigation { get; }

        public NearbyBaseViewModel(INavigation navigation = null)
        {
            Navigation = navigation;
        }
    }
}
