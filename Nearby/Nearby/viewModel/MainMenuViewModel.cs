using MvvmHelpers;
using Nearby.Utils;
using Nearby.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nearby.viewModel
{
    public class MainMenuViewModel : NearbyBaseViewModel
    {
        public MainMenuViewModel(INavigation navigation) : base(navigation)
        {
            Title = "Options";

            ChangeLocationIsEnabled = Settings.CustomLocationEnabled;

            if (Settings.CustomLocationEnabled)
            {
                CustomLatitude = Settings.CustomLatitude;
                CustomLongitude = Settings.CustomLongitude;
            }
        }

        string changelocationtext = "Change Location";
        public string ChangeLocationText
        {
            get { return changelocationtext; }
            set { changelocationtext = value; }
        }


        bool changelocationIsEnabled = false;
        public bool ChangeLocationIsEnabled
        {
            get { return changelocationIsEnabled; }
            set
            {
                SetProperty(ref changelocationIsEnabled, value);
                Settings.CustomLocationEnabled = value;
            }
        }

        string customLatitude = "";
        public string CustomLatitude
        {
            get { return customLatitude; }
            set
            {
                SetProperty(ref customLatitude, value);
                Settings.CustomLatitude = value;
            }
        }

        string customLongitude = "";
        public string CustomLongitude
        {
            get { return customLongitude; }
            set
            {
                SetProperty(ref customLongitude, value);
                Settings.CustomLongitude = value;
            }
        }
    }
}
