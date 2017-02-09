using Nearby.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Nearby.Pages
{
    public partial class AboutApp : ContentPage
    {
        AboutAppViewModel ViewModel => vm ?? (vm = BindingContext as AboutAppViewModel);
        AboutAppViewModel vm;

        public AboutApp()
        {
            InitializeComponent();
            BindingContext = vm = new AboutAppViewModel();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            vm = null;
        }
    }
}
