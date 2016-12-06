using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Nearby.viewModel
{
    public class AboutAppViewModel : NearbyBaseViewModel
    {
        public ObservableRangeCollection<FollowItem> FollowItems { get; } = new ObservableRangeCollection<FollowItem>();
        
        public string madeByText = $"Made with <3 by Gerade";
        public string MadeByText
        {
            get { return madeByText; }
            set { SetProperty(ref madeByText, value); }
        }
        
        public ImageSource appLogo = ImageSource.FromFile("nearby-logo.png");
        public ImageSource AppLogo
        {
            get { return appLogo; }
            set { SetProperty(ref appLogo, value); }
        }

        FollowItem selectedFollowItem;
        public FollowItem SelectedFollowItem
        {
            get { return selectedFollowItem; }
            set
            {
                selectedFollowItem = value;
                OnPropertyChanged();
                if (selectedFollowItem == null)
                    return;

                LaunchBrowserCommand.Execute(selectedFollowItem.FollowItemCommandProperty);
                SelectedFollowItem = null;
            }
        }


        public AboutAppViewModel(INavigation nav) : base(nav)
        {
            FollowItems.AddRange(new[]
               {
                    new FollowItem { ProviderLabel = "Twitter", ProviderValue = "twitter", FollowItemCommandProperty="https://twitter.com/Raidzen10" },
                    //new FollowItem { ProviderLabel = "Email", ProviderValue = "email" },
            });
        }
        
        public class FollowItem
        {
            public String ProviderLabel { get; set; }
            public string ProviderValue { get; set; }
            public ICommand FollowItemCommand { get; set; }
            public String FollowItemCommandProperty { get; set; }
        }
    }
}
