using MvvmHelpers;
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
            set { changelocationIsEnabled = value; }
        }


    }
}
