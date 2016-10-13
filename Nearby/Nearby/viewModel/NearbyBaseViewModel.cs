using MvvmHelpers;
using Nearby.Utils;
using Nearby.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Nearby.viewModel
{
    public class NearbyBaseViewModel : BaseViewModel
    {
        protected INavigation Navigation { get; }
        protected Database NearbyDataContext { get; }

        public NearbyBaseViewModel(INavigation navigation = null)
        {
            Navigation = navigation;
            NearbyDataContext = new Database();
        }

        public Settings Settings
        {
            get { return Settings.Current; }
        }
    }
}
